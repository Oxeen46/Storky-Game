using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Storky
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class StorkPrototype : MonoBehaviour
    {
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
        public event Action InteractionRequested;

        InputActionMap controls;
        InputAction move, look, flap, dive, roll, reset, interact, pause;
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
        GUIStyle titleStyle, textStyle;
        int diveHash, flyHash, glideHash, walkHash, takeoffHash, idleHash;

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
            reset = Button("Reset", "<Keyboard>/r", "<Gamepad>/select");
            interact = Button("Interact", "<Keyboard>/f", "<Gamepad>/buttonWest");
            pause = Button("Pause", "<Keyboard>/escape", "<Gamepad>/start");
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
            SetCursor(true);
        }
        void OnDisable()
        {
            controls?.Disable(); if (paused) {Time.timeScale=1; paused=false;}
            Cursor.lockState=previousCursorLock; Cursor.visible=previousCursorVisible;
        }
        void OnDestroy() { InputSystem.onDeviceChange -= DeviceChanged; controls?.Dispose(); }
        void OnApplicationFocus(bool focused) { if(focused && !paused) SetCursor(true); }
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

        void Update()
        {
            if (pause.WasPressedThisFrame())
            {
                paused=!paused;Time.timeScale=paused?0:1;SetCursor(!paused);
                return;
            }
            if(paused)return;
            float dt=Mathf.Min(Time.deltaTime,.05f);
            if(reset.WasPressedThisFrame() || transform.position.y < seaLevel+.12f) {ResetPlayer();return;}
            if(interact.WasPressedThisFrame()) InteractionRequested?.Invoke();
            Vector2 cameraInput=look.ReadValue<Vector2>();
            if(look.activeControl?.device is Gamepad) {cameraYaw+=cameraInput.x*125*dt;cameraPitch-=cameraInput.y*95*dt;}
            else {cameraYaw+=cameraInput.x*.12f;cameraPitch-=cameraInput.y*.12f;}
            cameraPitch=Mathf.Clamp(cameraPitch,-55,78);
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
            if(paused || stamina<flapCost || flapCooldown>0)return false;
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
            float dt=Mathf.Min(Time.deltaTime,.05f);
            Vector3 target=transform.position+Vector3.up*.85f;
            Vector3 back=Quaternion.Euler(cameraPitch,cameraYaw,0)*Vector3.back;
            float desiredLength=airborne?7.5f:5.5f;
            if(Physics.SphereCast(target,.22f,back,out RaycastHit hit,desiredLength,environmentMask,QueryTriggerInteraction.Ignore))desiredLength=Mathf.Max(.4f,hit.distance-.15f);
            cameraLength=desiredLength<cameraLength?desiredLength:Mathf.Lerp(cameraLength,desiredLength,1-Mathf.Exp(-7*dt));
            Vector3 pos=target+back*cameraLength;
            pos.y=Mathf.Max(seaLevel+.3f,pos.y);
            followCamera.transform.position=pos;followCamera.transform.LookAt(target);
            followCamera.fieldOfView=Mathf.Lerp(followCamera.fieldOfView,airborne?Mathf.Lerp(65,78,airSpeed/maxAirSpeed):62,1-Mathf.Exp(-3*dt));
        }
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
        public void ResetPlayer()
        {
            body.enabled=false;transform.SetPositionAndRotation(spawn,Quaternion.Euler(0,spawnYaw,0));body.enabled=true;
            velocity=Vector3.zero;airborne=false;launchTime=0;airSpeed=0;rollAngle=0;flightPitch=0;liftImpulse=0;updraftAcceleration=0;
            groundSpeed=0;rollVelocity=0;diveCharge=0;recoveryRemaining=0;recoveryLiftAmount=0;
            if(visual){visual.localPosition=visualRestPosition;visual.localRotation=initialVisual;}
            stamina=staminaCapacity;flapCooldown=0;flapVisual=0;heading=spawnYaw;cameraYaw=spawnYaw;cameraPitch=17;distance=0;mode="Idle";
        }
        void OnGUI()
        {
            if(titleStyle==null){titleStyle=new GUIStyle(GUI.skin.label){fontSize=20,fontStyle=FontStyle.Bold};textStyle=new GUIStyle(GUI.skin.label){fontSize=14,wordWrap=true};}
            float scale=Mathf.Clamp(Screen.height/900f,.65f,1.5f);Matrix4x4 old=GUI.matrix;GUI.matrix=Matrix4x4.Scale(Vector3.one*scale);
            GUI.Box(new Rect(18,18,480,paused?130:192),"");
            GUI.Label(new Rect(32,25,450,28),paused?"STORKY / DURAKLATILDI":"STORKY / KUS ADASI",titleStyle);
            if(paused){GUI.Label(new Rect(32,60,445,60),"Devam: Esc / Start\nKamera fareyle serbestce doner.",textStyle);GUI.matrix=old;return;}
            GUI.Label(new Rect(32,55,445,24),$"{family} | {mode} | Hiz: {velocity.magnitude:0.0} m/s",textStyle);
            GUI.Box(new Rect(32,84,440,16),"");GUI.Box(new Rect(34,86,436*Mathf.Clamp01(stamina/staminaCapacity),12),"");
            GUI.Label(new Rect(32,104,440,22),$"Stamina: {stamina:0}/{staminaCapacity:0}",textStyle);
            string hint;
            if(family=="Keyboard")hint="WASD: yuru / burun ve donus | Q/E: Air Roll\nSpace: kanat cirp | Shift: dalis | Fare: kamera\nF: etkilesim (hazir) | R: geri don | Esc: duraklat";
            else {string south=family=="PlayStation"?"Carpi":family=="Switch"?"B":"A";string east=family=="PlayStation"?"Daire":family=="Switch"?"A":"B";hint=$"Sol cubuk: hareket | Sag cubuk: kamera\n{south}: kanat cirp | {east}: dalis | L1/R1: Air Roll\nX/Kare: etkilesim | View/Share: don | Start: duraklat";}
            GUI.Label(new Rect(32,130,445,70),hint,textStyle);GUI.matrix=old;
        }
    }
}
