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
        public float walkSpeed = 3f, flightSpeed = 10f, climbSpeed = 4f;
        public string Mode => mode;
        public string DeviceFamily => family;
        public float DistanceTravelled => distance;
        InputActionMap controls;
        InputAction move, look, orbit, takeoff, ascend, descend, glide, dive, reset;
        CharacterController body;
        Vector3 velocity, spawn;
        float yaw, pitch = 20, launchTimer, distance;
        string mode = "Ground", state = "", family = "Keyboard";
        string deviceName = "Klavye / Fare";
        bool airborne;
        GUIStyle titleStyle, textStyle;

        void Awake()
        {
            body = GetComponent<CharacterController>(); spawn = transform.position; if (!followCamera) followCamera = Camera.main;
            controls = new InputActionMap("Stork");
            move = controls.AddAction("Move", InputActionType.Value);
            move.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
            move.AddBinding("<Gamepad>/leftStick");
            look = controls.AddAction("Look", InputActionType.Value);
            look.AddBinding("<Mouse>/delta"); look.AddBinding("<Gamepad>/rightStick");
            orbit = Button("Orbit", "<Mouse>/rightButton");
            takeoff = Button("Takeoff", "<Keyboard>/space", "<Gamepad>/buttonSouth");
            ascend = Button("Ascend", "<Keyboard>/space", "<Gamepad>/rightTrigger");
            descend = Button("Descend", "<Keyboard>/leftCtrl", "<Gamepad>/leftTrigger");
            glide = Button("Glide", "<Keyboard>/leftShift", "<Gamepad>/buttonWest");
            dive = Button("Dive", "<Keyboard>/e", "<Gamepad>/buttonEast");
            reset = Button("Reset", "<Keyboard>/r", "<Gamepad>/select");
            foreach (var a in controls.actions) a.performed += DetectDevice;
            InputSystem.onDeviceChange += DeviceChanged;
            if (Gamepad.current != null) SetDevice(Gamepad.current);
            if (animator != null) animator.applyRootMotion = false;
        }
        InputAction Button(string name, params string[] bindings)
        {
            var a = controls.AddAction(name, InputActionType.Button);
            foreach (var b in bindings) a.AddBinding(b);
            return a;
        }
        void OnEnable() => controls?.Enable();
        void OnDisable() => controls?.Disable();
        void OnDestroy() { InputSystem.onDeviceChange -= DeviceChanged; controls?.Dispose(); }
        void DeviceChanged(InputDevice d, InputDeviceChange change)
        {
            if (d is Gamepad && (change == InputDeviceChange.Disconnected || change == InputDeviceChange.Removed))
            { family = "Keyboard"; deviceName = "Klavye / Fare"; }
        }
        void DetectDevice(InputAction.CallbackContext ctx)
        {
            if (ctx.control.device is Mouse && (!orbit.IsPressed() || ctx.ReadValueAsObject() is Vector2 v && v.sqrMagnitude < 1)) return;
            SetDevice(ctx.control.device);
        }
        void SetDevice(InputDevice d)
        {
            if (!(d is Gamepad)) { family = "Keyboard"; deviceName = "Klavye / Fare"; return; }
            string id = (d.layout + " " + d.description.product + " " + d.description.manufacturer).ToLowerInvariant();
            family = id.Contains("dual") || id.Contains("playstation") ? "PlayStation" : id.Contains("switch") || id.Contains("nintendo") ? "Switch" : id.Contains("xinput") || id.Contains("xbox") ? "Xbox" : "Gamepad";
            deviceName = d.displayName;
        }
        void Update()
        {
            float dt = Time.deltaTime;
            if (reset.WasPressedThisFrame() || transform.position.y < -20) ResetPlayer();
            Vector2 cameraInput = look.ReadValue<Vector2>();
            if (look.activeControl?.device is Gamepad) { yaw += cameraInput.x * 110 * dt; pitch -= cameraInput.y * 85 * dt; }
            else if (orbit.IsPressed()) { yaw += cameraInput.x * .15f; pitch -= cameraInput.y * .15f; }
            pitch = Mathf.Clamp(pitch, -15, 65);
            Vector2 input = Vector2.ClampMagnitude(move.ReadValue<Vector2>(), 1);
            Quaternion heading = Quaternion.Euler(0, yaw, 0);
            Vector3 direction = heading * new Vector3(input.x, 0, input.y);
            bool grounded = body.isGrounded;
            if (!airborne && grounded && takeoff.WasPressedThisFrame()) { airborne = true; launchTimer = .9f; velocity.y = 3f; }
            if (airborne)
            {
                launchTimer = Mathf.Max(0, launchTimer - dt);
                bool diving = dive.IsPressed() && launchTimer <= 0;
                bool gliding = glide.IsPressed() && !diving && launchTimer <= 0;
                mode = launchTimer > 0 ? "Takeoff" : diving ? "Dive" : gliding ? "Glide" : "Fly";
                Vector3 forward = transform.forward;
                Vector3 desired = direction.sqrMagnitude > .01f ? direction.normalized : forward;
                float speed = flightSpeed * (diving ? 1.5f : gliding ? 1.1f : 1);
                Vector3 horizontal = Vector3.Lerp(new Vector3(velocity.x, 0, velocity.z), desired * speed * Mathf.Lerp(.55f, 1, input.magnitude), 1 - Mathf.Exp(-3 * dt));
                velocity.x = horizontal.x; velocity.z = horizontal.z;
                float vertical = launchTimer > 0 ? 3f : diving ? -8f : gliding ? -.7f : (ascend.ReadValue<float>() - descend.ReadValue<float>()) * climbSpeed;
                velocity.y = Mathf.Lerp(velocity.y, vertical, 1 - Mathf.Exp(-5 * dt));
                if (direction.sqrMagnitude > .01f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1 - Mathf.Exp(-4 * dt));
            }
            else
            {
                mode = input.sqrMagnitude > .01f ? "Walk" : "Idle";
                velocity.x = direction.x * walkSpeed; velocity.z = direction.z * walkSpeed;
                velocity.y = grounded ? -2f : velocity.y - 18f * dt;
                if (direction.sqrMagnitude > .01f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1 - Mathf.Exp(-12 * dt));
            }
            Vector3 before = transform.position;
            var collisions = body.Move(velocity * dt);
            distance += Vector3.Distance(before, transform.position);
            if (airborne && launchTimer <= 0 && (collisions & CollisionFlags.Below) != 0 && velocity.y <= 0) { airborne = false; velocity.y = -2; mode = input.sqrMagnitude > .01f ? "Walk" : "Idle"; }
            // Prevent leaving the training area or climbing indefinitely.
            Vector3 clamped = transform.position;
            clamped.x = Mathf.Clamp(clamped.x, -145, 145); clamped.z = Mathf.Clamp(clamped.z, -145, 145); clamped.y = Mathf.Min(clamped.y, 55);
            if (clamped != transform.position) { body.enabled = false; transform.position = clamped; body.enabled = true; }
            if (animator != null)
            {
                if (state != mode) { animator.CrossFadeInFixedTime(mode, .18f); state = mode; }
                animator.speed = mode == "Takeoff" ? 2.4f / .9f : mode == "Walk" ? Mathf.Max(.3f, input.magnitude) : 1;
            }
        }
        void LateUpdate()
        {
            if (!followCamera) return;
            Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 target = transform.position + Vector3.up * .9f;
            Vector3 desired = target + orbitRotation * new Vector3(0, 0, airborne ? -7 : -5);
            // Keep camera above the flat training floor.
            desired.y = Mathf.Max(.35f, desired.y);
            followCamera.transform.position = Vector3.Lerp(followCamera.transform.position, desired, 1 - Mathf.Exp(-8 * Time.deltaTime));
            followCamera.transform.LookAt(target);
        }
        public void ResetPlayer()
        {
            body.enabled = false; transform.SetPositionAndRotation(spawn, Quaternion.identity); body.enabled = true;
            velocity = Vector3.zero; airborne = false; launchTimer = 0; yaw = 0; pitch = 20; distance = 0;
        }
        void OnGUI()
        {
            if (titleStyle == null) { titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold }; textStyle = new GUIStyle(GUI.skin.label) { fontSize = 16, wordWrap = true }; }
            float scale = Mathf.Clamp(Screen.height / 900f, .65f, 1.5f);
            Matrix4x4 old = GUI.matrix; GUI.matrix = Matrix4x4.Scale(Vector3.one * scale);
            GUI.Box(new Rect(18, 18, 540, 218), "");
            GUI.Label(new Rect(34, 28, 510, 35), "STORKY / UCUS TEST ALANI", titleStyle);
            GUI.Label(new Rect(34, 66, 510, 30), $"{family} - {deviceName}  |  {mode}", textStyle);
            string buttons;
            if (family == "Keyboard") buttons = "WASD: hareket   |   Sag fare + fare: kamera\nSpace: kalkis / yuksel   |   Sol Ctrl: alcal\nSol Shift basili: suzul   |   E basili: dalis\nR: baslangica don";
            else { string south = family == "PlayStation" ? "Cross" : family == "Switch" ? "B" : family == "Xbox" ? "A" : "Alt tus"; string west = family == "PlayStation" ? "Square" : family == "Switch" ? "Y" : family == "Xbox" ? "X" : "Sol tus"; string east = family == "PlayStation" ? "Circle" : family == "Switch" ? "A" : family == "Xbox" ? "B" : "Sag tus"; string triggers = family == "PlayStation" ? "R2 / L2" : family == "Switch" ? "ZR / ZL" : "RT / LT"; buttons = $"Sol cubuk: hareket   |   Sag cubuk: kamera\n{south}: kalkis   |   {triggers}: yuksel / alcal\n{west} basili: suzul   |   {east} basili: dalis\nView / Share / -: baslangica don"; }
            GUI.Label(new Rect(34, 100, 510, 110), buttons, textStyle);
            GUI.Label(new Rect(34, 207, 510, 25), $"Yukseklik: {Mathf.Max(0, transform.position.y):0.0} m    Yol: {distance:0} m    Izgara: 1 m / kalin: 10 m", textStyle);
        }
    }
}


