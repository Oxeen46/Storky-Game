using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace Storky
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class StorkPrototype : MonoBehaviour
    {
        // Sahne bağlantıları ve hareket ayarları
        public Transform visual;
        public Animator animator;
        public Camera followCamera;
        [Header("Ground and flight tuning")]
        public float walkSpeed = 1.4f, flightSpeed = 14f, climbSpeed = 4f;
        public float walkCycleSpeed = .805f;
        public float maxAirSpeed = 62f, stallSpeed = 8f, pitchRate = 68f, turnRate = 65f, rollRate = 220f;
        public float staminaCapacity = 100f, flapCost = 16f, glideRecovery = 1.2f, groundRecovery = 65f;
        public float flapSpeedImpulse = 5.5f, flapLiftImpulse = 3.2f, seaLevel;
        public float recoverySpeedImpulse = 11f, recoveryLift = 9f, recoveryDuration = 2f;
        public LayerMask environmentMask = 1;

        // Oyuncunun okunabilir durumu
        public string Mode => mode;
        public string DeviceFamily => family;
        public float DistanceTravelled => distance;
        public float Stamina => stamina;
        public float AirSpeed => airSpeed;
        public Vector3 Velocity => velocity;
        public bool Airborne => airborne;
        public bool Paused => paused;
        public float RollAngle => rollAngle;
        public float FlightPitch => flightPitch;
        public float GroundSpeed => groundSpeed;
        public float WalkPlaybackRate => animator && mode=="Walk" ? animator.speed : 0;
        public float DiveCharge => diveCharge;
        public float RecoveryRemaining => recoveryRemaining;
        public Vector3 VisualBodyCenter => bodyBone ? bodyBone.position : transform.position;

        // Giriş, hareket ve sahne durumu
        InputActionMap controls;
        InputAction move, look, flap, dive, roll, pause;
        CharacterController body;
        Vector3 velocity, spawn, visualRestPosition;
        Transform bodyBone;
        Quaternion initialVisual;
        float spawnYaw, heading, flightPitch, rollAngle, cameraYaw, cameraPitch = 17f;
        float airSpeed, stamina, distance, flapCooldown, flapVisual, launchTime, liftImpulse;
        float updraftAcceleration, cameraLength = 6f;
        float groundSpeed, rollVelocity, diveCharge, recoveryRemaining, recoveryLiftAmount;
        string mode = "Idle", state = "", family = "Keyboard";
        bool airborne, paused, previousCursorVisible;
        CursorLockMode previousCursorLock;

        // Dayanıklılık göstergesinin durumu
        VisualElement staminaRing;
        PanelSettings panelSettings;
        float displayedStamina = 1f;
        float warningTime;
        Renderer[] characterRenderers;
        Vector2 staminaRingPosition;
        bool hasStaminaRingPosition;

        // Kamera, duraklatma ve animasyon durumu
        FlightSettings settings;
        FlightPauseMenu menu;
        Vector3 cameraAnchor;
        bool anchorReady;
        float priorTimeScale;
        bool priorAudioPause;
        int diveHash, flyHash, glideHash, walkHash, takeoffHash, idleHash;

        // Unity yaşam döngüsü ve giriş kurulumu
        void Awake()
        {
            body = GetComponent<CharacterController>();
            spawn = transform.position; spawnYaw = transform.eulerAngles.y; heading = spawnYaw; cameraYaw = spawnYaw;
            if (!followCamera) followCamera = Camera.main;
            if (visual) {initialVisual = visual.localRotation; visualRestPosition=visual.localPosition;}
            if (animator)
                foreach(var t in animator.GetComponentsInChildren<Transform>())
                    if(t.name=="Body") {bodyBone=t;break;}
            stamina = staminaCapacity;
            CreateStaminaRing();
            controls = new InputActionMap("Stork");
            move = controls.AddAction("Move", InputActionType.Value);
            move.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
            move.AddBinding("<Gamepad>/leftStick").WithProcessor("stickDeadzone(min=0.16,max=0.95)");
            look = controls.AddAction("Look", InputActionType.Value);
            look.AddBinding("<Mouse>/delta"); look.AddBinding("<Gamepad>/rightStick").WithProcessor("stickDeadzone(min=0.16,max=0.95)");
            flap = Button("Flap", "<Keyboard>/space", "<Gamepad>/buttonSouth");
            dive = Button("Dive", "<Keyboard>/leftShift", "<Gamepad>/buttonEast");
            roll = controls.AddAction("AirRoll", InputActionType.Value);
            roll.AddCompositeBinding("1DAxis").With("Negative", "<Keyboard>/q").With("Positive", "<Keyboard>/e");
            roll.AddCompositeBinding("1DAxis").With("Negative", "<Gamepad>/leftShoulder").With("Positive", "<Gamepad>/rightShoulder");
            pause = Button("Pause", "<Keyboard>/escape", "<Gamepad>/start");
            settings = FlightSettings.Load();
            menu = new FlightPauseMenu(staminaRing.parent, settings, controls, () => SetPaused(false), PreviewCamera);
            foreach (var a in controls.actions) a.performed += DetectDevice;
            InputSystem.onDeviceChange += DeviceChanged;
            if (Gamepad.current != null) SetDevice(Gamepad.current);
            if (animator) animator.applyRootMotion = false;
            idleHash=Animator.StringToHash("Idle"); walkHash=Animator.StringToHash("Walk");
            takeoffHash=Animator.StringToHash("Takeoff"); flyHash=Animator.StringToHash("Fly");
            glideHash=Animator.StringToHash("Glide"); diveHash=Animator.StringToHash("Dive");
        }

        InputAction Button(string name, params string[] bindings)
        {
            var action = controls.AddAction(name, InputActionType.Button);
            foreach (string binding in bindings) action.AddBinding(binding);
            return action;
        }
        void OnEnable()
        {
            controls?.Enable(); previousCursorLock=Cursor.lockState; previousCursorVisible=Cursor.visible;
            if (staminaRing != null) staminaRing.visible = true;
            SetCursor(true);
        }
        void OnDisable()
        {
            controls?.Disable(); if (paused) SetPaused(false);
            if (staminaRing != null) staminaRing.visible = false;
            Cursor.lockState=previousCursorLock; Cursor.visible=previousCursorVisible;
        }
        void OnDestroy() { InputSystem.onDeviceChange -= DeviceChanged; controls?.Dispose(); Destroy(panelSettings); }
        void OnApplicationFocus(bool focused) { if(focused && !paused) SetCursor(true); }

        // İmleç ve giriş cihazı
        void SetCursor(bool locked) { Cursor.lockState=locked?CursorLockMode.Locked:CursorLockMode.None; Cursor.visible=!locked; }
        void DeviceChanged(InputDevice device, InputDeviceChange change)
        {
            if(device is Gamepad && (change==InputDeviceChange.Removed || change==InputDeviceChange.Disconnected)) family="Keyboard";
        }
        void DetectDevice(InputAction.CallbackContext context)
        {
            if(context.control.device is Mouse && context.ReadValueAsObject() is Vector2 v && v.sqrMagnitude<1) return;
            SetDevice(context.control.device);
        }
        void SetDevice(InputDevice device)
        {
            if (!(device is Gamepad)) {family="Keyboard";return;}
            string id=(device.layout+" "+device.description.product+" "+device.description.manufacturer).ToLowerInvariant();
            family=id.Contains("dual")||id.Contains("playstation")?"PlayStation":id.Contains("switch")||id.Contains("nintendo")?"Switch":"Xbox";
        }

        // Kare başına giriş ve hareket
        void Update()
        {
            if (paused)
            {
                menu.Tick(pause.WasPressedThisFrame());
                return;
            }
            if (pause.WasPressedThisFrame()) { SetPaused(true); return; }
            float dt=Mathf.Min(Time.deltaTime,.05f);
            if(transform.position.y < seaLevel+.12f) {ResetPlayer();return;}
            Vector2 cameraInput=look.ReadValue<Vector2>();
            cameraInput *= look.activeControl?.device is Gamepad ? settings.stickSensitivity : settings.mouseSensitivity;
            if (settings.invertY) cameraInput.y = -cameraInput.y;
            if(look.activeControl?.device is Gamepad) {cameraYaw+=cameraInput.x*125*dt;cameraPitch-=cameraInput.y*95*dt;}
            else {cameraYaw+=cameraInput.x*.12f;cameraPitch-=cameraInput.y*.12f;}
            cameraPitch=Mathf.Clamp(cameraPitch,-55-settings.angle+17,78-settings.angle+17);
            Vector2 input=Vector2.ClampMagnitude(move.ReadValue<Vector2>(),1);
            flapCooldown=Mathf.Max(0,flapCooldown-dt);flapVisual=Mathf.Max(0,flapVisual-dt);launchTime=Mathf.Max(0,launchTime-dt);
            bool grounded=body.isGrounded;
            if(!airborne && !grounded && velocity.y < -2.5f) // Walking off a roof enters glide.
            {airborne=true;airSpeed=Mathf.Max(stallSpeed, new Vector2(velocity.x,velocity.z).magnitude);flightPitch=-8;}
            if(flap.WasPressedThisFrame()) TryFlap();
            Vector3 before=transform.position;
            if(airborne) SimulateFlight(input,dt);
            else SimulateGround(input,grounded,dt);
            var flags=body.Move(velocity*dt);
            Vector3 displacement=transform.position-before;
            distance+=displacement.magnitude;
            groundSpeed=airborne?0:Vector3.ProjectOnPlane(displacement,Vector3.up).magnitude/Mathf.Max(dt,.0001f);
            // Charge only from actual descent, never from tapping the dive button.
            if(airborne && recoveryRemaining<=0 && flightPitch < -20 && displacement.y < 0)
                diveCharge=Mathf.Min(1,diveCharge-displacement.y/35f);
            else if(airborne && flightPitch > -10) diveCharge=Mathf.Max(0,diveCharge-.045f*dt);
            if(airborne && launchTime<=0 && velocity.y<=0 && (flags & CollisionFlags.Below)!=0)
            {
                airborne=false;velocity.y=-2;airSpeed=0;rollAngle=0;rollVelocity=0;flightPitch=0;liftImpulse=0;
                diveCharge=0;recoveryRemaining=0;recoveryLiftAmount=0;
            }
            // Walls are forgiving: shed speed and slide instead of killing the bird.
            if(airborne && (flags & CollisionFlags.Sides)!=0) airSpeed=Mathf.Max(stallSpeed*.65f,airSpeed-18*dt);
            if(!airborne) stamina=Mathf.Min(staminaCapacity,stamina+groundRecovery*dt);
            transform.rotation=Quaternion.Euler(0,heading,0);
            UpdateAnimation(input.magnitude);
        }

        // Yürüyüş, uçuş ve kanat vuruşu
        void SimulateGround(Vector2 input,bool grounded,float dt)
        {
            Vector3 direction=Quaternion.Euler(0,cameraYaw,0)*new Vector3(input.x,0,input.y);
            velocity.x=direction.x*walkSpeed;velocity.z=direction.z*walkSpeed;
            velocity.y=grounded?-2:velocity.y-18*dt;
            if(direction.sqrMagnitude>.01f) heading=Mathf.LerpAngle(heading,Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg,1-Mathf.Exp(-12*dt));
            mode=input.sqrMagnitude>.01f?"Walk":"Idle";
        }
        void SimulateFlight(Vector2 input,float dt)
        {
            bool diving=dive.IsPressed() && launchTime<=0;
            float rollInput=roll.ReadValue<float>();
            rollVelocity=Mathf.MoveTowards(rollVelocity,rollInput*rollRate,1000*dt);
            rollAngle=Mathf.Repeat(rollAngle+rollVelocity*dt+180,360)-180;
            // W/left stick forward lowers the nose. Looking around never steers the bird.
            flightPitch-=input.y*pitchRate*dt;
            if(Mathf.Abs(input.y)<.08f && !diving)flightPitch=Mathf.MoveTowards(flightPitch,-4,6*dt);
            if(diving)flightPitch=Mathf.MoveTowards(flightPitch,-64,75*dt);
            flightPitch=Mathf.Clamp(flightPitch,-78,65);
            float authority=Mathf.Lerp(.45f,1,Mathf.InverseLerp(stallSpeed,22,airSpeed));
            // Air Roll rotates around the torso/forward axis; it never adds an orbit or yaw.
            heading+=input.x*turnRate*authority*dt;
            recoveryRemaining=Mathf.Max(0,recoveryRemaining-dt);
            if(!diving && recoveryRemaining<=0 && diveCharge>=.15f && airSpeed>12 && (input.y<-.1f || flightPitch>-12))
            {
                float charge=diveCharge;diveCharge=0;
                airSpeed=Mathf.Min(maxAirSpeed,airSpeed+charge*recoverySpeedImpulse);
                recoveryLiftAmount=charge*recoveryLift;recoveryRemaining=recoveryDuration;
            }
            // Gravity exchanges height for airspeed; drag dissipates energy.
            float gravityAlongNose=-Mathf.Sin(flightPitch*Mathf.Deg2Rad)*9.81f;
            if(gravityAlongNose<0 && recoveryRemaining>0)gravityAlongNose*=.55f;
            airSpeed+=(gravityAlongNose-.06f-.00035f*airSpeed*airSpeed)*dt;
            airSpeed=Mathf.Clamp(airSpeed,4,maxAirSpeed);
            float stall=1-Mathf.InverseLerp(4,stallSpeed,airSpeed);
            if(stall>.1f)flightPitch=Mathf.MoveTowards(flightPitch,-22,stall*30*dt);
            Vector3 forward=Quaternion.Euler(-flightPitch,heading,0)*Vector3.forward;
            float sink=Mathf.Lerp(1.1f,.2f,Mathf.InverseLerp(14,32,airSpeed))+stall*6;
            liftImpulse=Mathf.MoveTowards(liftImpulse,0,3*dt);
            float recoverySupport=recoveryLiftAmount*Mathf.Clamp01(recoveryRemaining/Mathf.Max(.01f,recoveryDuration));
            Vector3 desired=forward*airSpeed+Vector3.up*(liftImpulse+recoverySupport-sink);
            velocity=Vector3.Lerp(velocity,desired,1-Mathf.Exp(-5*dt));
            velocity.y+=updraftAcceleration*dt;
            if(flapVisual<=0)stamina=Mathf.Min(staminaCapacity,stamina+glideRecovery*dt);
            mode=launchTime>0?"Takeoff":diving?"Dive":flapVisual>0?"Fly":"Glide";
        }
        public bool TryFlap()
        {
            if (paused) return false;
            if (stamina < flapCost)
            {
                warningTime = .48f;
                return false;
            }
            if (flapCooldown > 0) return false;
            stamina-=flapCost;flapCooldown=.38f;flapVisual=.42f;
            if(!airborne)
            {
                airborne=true;launchTime=.65f;airSpeed=flightSpeed+flapSpeedImpulse*.35f;flightPitch=15;rollAngle=0;rollVelocity=0;
                velocity=Quaternion.Euler(0,heading,0)*Vector3.forward*airSpeed+Vector3.up*5;
                liftImpulse=5;
            }
            else
            {
                float addedSpeed=Mathf.Min(flapSpeedImpulse,maxAirSpeed-airSpeed);
                airSpeed+=addedSpeed;liftImpulse=Mathf.Min(6,liftImpulse+flapLiftImpulse);
                velocity+=Quaternion.Euler(-flightPitch,heading,0)*Vector3.forward*(addedSpeed*.65f);
            }
            return true;
        }
        public void AddUpdraft(float acceleration) {updraftAcceleration=Mathf.Clamp(acceleration,0,2);}
        public void ClearUpdraft() {updraftAcceleration=0;}

        // Animasyon ve kare sonu güncellemesi
        void UpdateAnimation(float input)
        {
            if(!animator)return;
            if(!airborne)mode=groundSpeed>.04f?"Walk":"Idle";
            int hash=mode=="Walk"?walkHash:mode=="Takeoff"?takeoffHash:mode=="Dive"?diveHash:mode=="Fly"?flyHash:mode=="Glide"?glideHash:idleHash;
            if(state!=mode){animator.CrossFadeInFixedTime(hash,.16f);state=mode;}
            // Use distance actually travelled: blocked movement does not keep walking in place.
            animator.speed=mode=="Walk"?groundSpeed/Mathf.Max(.01f,walkCycleSpeed):mode=="Takeoff"?3.2f:1;
        }
        void LateUpdate()
        {
            ApplyBodyPose();
            if(!followCamera || paused)return;
            PositionCamera(false);
            UpdateStaminaRing();
        }

        // Duraklatma ve devam etme
        void SetPaused(bool value)
        {
            paused = value;
            if (value)
            {
                priorTimeScale = Time.timeScale;
                priorAudioPause = AudioListener.pause;
            }
            Time.timeScale = value ? 0 : priorTimeScale;
            AudioListener.pause = value || priorAudioPause;
            SetCursor(!value);
            staminaRing.visible = !value;
            if (value) menu.Open(); else menu.Close();
        }

        // Kamera takibi ve önizleme
        void PreviewCamera()
        {
            if (paused && followCamera) PositionCamera(true);
        }

        void PositionCamera(bool preview)
        {
            float dt=Mathf.Min(Time.deltaTime,.05f);
            Vector3 target=transform.position+Vector3.up*settings.Height;
            const float followRate = 69;
            cameraAnchor = !anchorReady || preview ? target : Vector3.Lerp(cameraAnchor, target, 1-Mathf.Exp(-followRate*dt));
            anchorReady = true;
            Vector3 back=Quaternion.Euler(Mathf.Clamp(cameraPitch+settings.angle-17,-55,78),cameraYaw,0)*Vector3.back;
            float speedFraction = airborne ? Mathf.InverseLerp(flightSpeed,maxAirSpeed,airSpeed) : 0;
            Vector3 offset = cameraAnchor-target + back*(settings.Distance + speedFraction*settings.SpeedDistance);
            float desiredLength=offset.magnitude;
            if (desiredLength > .001f) back = offset / desiredLength;
            if(Physics.SphereCast(target,.22f,back,out RaycastHit hit,desiredLength,environmentMask,QueryTriggerInteraction.Ignore))desiredLength=Mathf.Max(.4f,hit.distance-.15f);
            cameraLength=preview || desiredLength<cameraLength?desiredLength:Mathf.Lerp(cameraLength,desiredLength,1-Mathf.Exp(-7*dt));
            Vector3 pos=target+back*cameraLength;
            pos.y=Mathf.Max(seaLevel+.3f,pos.y);
            Vector3 forward = target - pos;
            followCamera.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(forward.sqrMagnitude > .000001f ? forward : -back));
            float targetFov = 62 + (airborne ? Mathf.Lerp(3,16,airSpeed/maxAirSpeed) : 0);
            followCamera.fieldOfView=preview ? targetFov : Mathf.Lerp(followCamera.fieldOfView,targetFov,1-Mathf.Exp(-3*dt));
        }

        // Gövde pozu
        void ApplyBodyPose()
        {
            if(!visual)return;
            // Animator has now evaluated. Anchor the current torso pose while rotating its mesh.
            visual.localPosition=visualRestPosition;visual.localRotation=initialVisual;
            if(!airborne)return;
            Vector3 pivot=bodyBone?visual.InverseTransformPoint(bodyBone.position):new Vector3(0,.76f,-.14f);
            Quaternion orientation=Quaternion.AngleAxis(-flightPitch,Vector3.right)*Quaternion.AngleAxis(-rollAngle,Vector3.forward);
            visual.localRotation=orientation*initialVisual;
            visual.localPosition=visualRestPosition+initialVisual*pivot-visual.localRotation*pivot;
        }

        // Başlangıç konumuna dönüş
        public void ResetPlayer()
        {
            anchorReady=false;
            body.enabled=false;transform.SetPositionAndRotation(spawn,Quaternion.Euler(0,spawnYaw,0));body.enabled=true;
            velocity=Vector3.zero;airborne=false;launchTime=0;airSpeed=0;rollAngle=0;flightPitch=0;liftImpulse=0;updraftAcceleration=0;
            groundSpeed=0;rollVelocity=0;diveCharge=0;recoveryRemaining=0;recoveryLiftAmount=0;
            if(visual){visual.localPosition=visualRestPosition;visual.localRotation=initialVisual;}
            stamina=staminaCapacity;flapCooldown=0;flapVisual=0;heading=spawnYaw;cameraYaw=spawnYaw;cameraPitch=17;distance=0;mode="Idle";
        }

        // Dayanıklılık göstergesi
        void CreateStaminaRing()
        {
            characterRenderers = GetComponentsInChildren<Renderer>(true);
            var hud = new GameObject("Stamina HUD");
            hud.transform.SetParent(transform, false);
            panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            panelSettings.themeStyleSheet = Resources.Load<ThemeStyleSheet>("StorkyHudTheme");
            var document = hud.AddComponent<UIDocument>();
            document.panelSettings = panelSettings;
            var root = document.rootVisualElement;
            root.pickingMode = PickingMode.Ignore;
            staminaRing = new VisualElement { name = "stamina-ring", pickingMode = PickingMode.Ignore };
            staminaRing.style.position = Position.Absolute;
            staminaRing.generateVisualContent += DrawStaminaRing;
            root.Add(staminaRing);
        }

        void UpdateStaminaRing()
        {
            Vector3 point = followCamera.WorldToScreenPoint(VisualBodyCenter);
            float scale = Mathf.Clamp(Screen.height / 900f, .7f, 1.5f);
            float size = 34f * scale;
            float left = point.x, right = point.x, top = point.y, bottom = point.y;
            foreach (var mesh in characterRenderers)
            {
                if (!mesh || !mesh.enabled || !mesh.gameObject.activeInHierarchy) continue;
                Bounds bounds = mesh.bounds;
                for (int i = 0; i < 8; i++)
                {
                    Vector3 corner = new Vector3((i & 1) == 0 ? bounds.min.x : bounds.max.x,
                        (i & 2) == 0 ? bounds.min.y : bounds.max.y,
                        (i & 4) == 0 ? bounds.min.z : bounds.max.z);
                    Vector3 screen = followCamera.WorldToScreenPoint(corner);
                    if (screen.z <= 0) continue;
                    left = Mathf.Min(left, screen.x);
                    right = Mathf.Max(right, screen.x);
                    top = Mathf.Max(top, screen.y);
                    bottom = Mathf.Min(bottom, screen.y);
                }
            }
            float gap = 9f * scale;
            Rect silhouette = Rect.MinMaxRect(left - gap, Screen.height - top - gap,
                right + gap, Screen.height - bottom + gap);
            float maxX = Screen.width - size - gap;
            float maxY = Screen.height - size - gap;
            Vector2 bodyPoint = new Vector2(point.x, Screen.height - point.y);
            float preferredY = bodyPoint.y - size * 1.55f;
            Vector2 target = new Vector2(Mathf.Clamp(bodyPoint.x - size * 1.65f, gap, maxX),
                Mathf.Clamp(preferredY, gap, maxY));
            if (RingOverlaps(target, size, silhouette))
            {
                target.y = silhouette.yMin - size;
                if (target.y < gap)
                {
                    target = new Vector2(silhouette.xMax, Mathf.Clamp(preferredY, gap, maxY));
                    if (target.x > maxX) target.x = silhouette.xMin - size;
                    if (target.x < gap)
                        target = new Vector2(Mathf.Clamp(bodyPoint.x - size / 2, gap, maxX), silhouette.yMax);
                }
            }
            bool fits = point.z > 0 && FitsScreen(target, gap, maxX, maxY);
            staminaRing.style.display = fits ? DisplayStyle.Flex : DisplayStyle.None;
            if (fits)
            {
                float blend = 1f - Mathf.Exp(-18f * Time.deltaTime);
                Vector2 next = Vector2.Lerp(staminaRingPosition, target, blend);
                // A fast roll can put a wing in the interpolation path.
                if (!hasStaminaRingPosition || RingOverlaps(next, size, silhouette) ||
                    !FitsScreen(next, gap, maxX, maxY))
                    next = target;
                staminaRingPosition = next;
                staminaRing.style.left = next.x;
                staminaRing.style.top = next.y;
            }
            hasStaminaRingPosition = fits;
            staminaRing.style.width = size;
            staminaRing.style.height = size;
            float fraction = staminaCapacity > 0 ? Mathf.Clamp01(stamina / staminaCapacity) : 0;
            if (fraction != displayedStamina || warningTime > 0)
            {
                displayedStamina = Mathf.MoveTowards(displayedStamina, fraction, Time.deltaTime * .42f);
                warningTime = Mathf.Max(0, warningTime - Time.deltaTime);
                staminaRing.MarkDirtyRepaint();
            }
        }

        static bool RingOverlaps(Vector2 position, float size, Rect silhouette)
        {
            return new Rect(position.x, position.y, size, size).Overlaps(silhouette);
        }

        static bool FitsScreen(Vector2 position, float min, float maxX, float maxY)
        {
            return position.x >= min && position.x <= maxX &&
                   position.y >= min && position.y <= maxY;
        }

        void DrawStaminaRing(MeshGenerationContext context)
        {
            var painter = context.painter2D;
            float size = staminaRing.contentRect.width;
            Vector2 center = Vector2.one * (size * .5f);
            float radius = size * .38f;
            bool exhausted = stamina < flapCost;
            float flash = warningTime > 0 ? .5f + .5f * Mathf.Cos((.48f - warningTime) * Mathf.PI * 8f) : 0;
            painter.lineWidth = size * .21f;
            painter.strokeColor = exhausted
                ? Color.Lerp(new Color(.45f, .07f, .09f), new Color(1f, .32f, .26f), flash)
                : new Color(.07f, .13f, .13f, .94f);
            painter.BeginPath();
            painter.Arc(center, radius, -90, 270);
            painter.Stroke();
            if (displayedStamina <= 0) return;
            painter.lineCap = LineCap.Round;
            painter.lineWidth = size * .135f;
            painter.strokeColor = exhausted ? new Color(1f, .25f, .20f)
                : stamina < 2f * flapCost ? new Color(1f, .65f, .19f) : new Color(.49f, .96f, .49f);
            painter.BeginPath();
            painter.Arc(center, radius, -90, -90 + 360 * displayedStamina);
            painter.Stroke();
        }
    }
}
