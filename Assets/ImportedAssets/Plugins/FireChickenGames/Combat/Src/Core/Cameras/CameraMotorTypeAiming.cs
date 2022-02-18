namespace FireChickenGames.Combat.Core.Cameras
{
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;
    using GameCreator.Camera;
    using System.Linq;

    public class CameraMotorTypeAiming : ICameraMotorType
    {
        public static new string NAME = "Combat Aiming Camera";

        private const string INPUT_MOUSE_X = "Mouse X";
        private const string INPUT_MOUSE_Y = "Mouse Y";
        private const string INPUT_MOUSE_W = "Mouse ScrollWheel";

        private GameObject pivot;
        private bool motorEnabled;
        private float targetRotationX;
        private float targetRotationY;
        private Vector3 _velocityPosition = Vector3.zero;

        public TargetGameObject target = new TargetGameObject(TargetGameObject.Target.Player);
        public Vector3 targetOffset = Vector3.up;
        public Vector3 pivotOffset = Vector3.zero;

        // Mouse orbit.
        public bool allowOrbitInput = true;
        public CameraMotorTypeAdventure.OrbitInput orbitInput = CameraMotorTypeAdventure.OrbitInput.MouseMove;
        public float orbitSpeed = 25.0f;
        public float orbitInputTime = -1000f;

        [Range(0.0f, 180f)] public float maxPitch = 120f;
        public Vector2Property sensitivity = new Vector2Property(Vector2.one * 10f);

        // Zoom.
        public bool allowZoom = true;
        public float zoomSpeed = 25.0f;
        public float initialZoom = 3.0f;
        [Range(1f, 20f)]
        public float zoomSensitivity = 5.0f;
        public Vector2 zoomLimits = new Vector2(1f, 10f);

        private float desiredZoom;
        private float currentZoom;
        private float targetZoom;

        // Wall avoidance.
        private WallDetector wallAvoider;
        private float wallConstrainZoom;
        public bool avoidWallClip = true;
        public float wallClipRadius = 0.4f;
        public LayerMask wallClipLayerMask = ~4;

        private CursorLockMode cursorLock = CursorLockMode.None;

        void Start()
        {
            var targetGameObject = target.GetGameObject(gameObject);
            if (targetGameObject != null)   
            {
                var targetTransform = targetGameObject.transform;
                targetRotationX = targetTransform.rotation.eulerAngles.y + 180f;
                targetRotationY = targetTransform.rotation.eulerAngles.x;
                transform.position = targetTransform.position + targetOffset;
                transform.rotation = Quaternion.Euler(targetRotationY, targetRotationX, 0f);
            }
            wallAvoider = new WallDetector();
        }

        void Awake()
        {
            if (pivot == null)
            {
                pivot = new GameObject(gameObject.name + " Pivot");
                pivot.transform.SetParent(transform);
            }

            pivot.transform.localRotation = Quaternion.identity;
            pivot.transform.localPosition = (Vector3.forward * initialZoom) + pivotOffset;
        }

        void FixedUpdate()
        {
            wallConstrainZoom = zoomLimits.y;

            if (!motorEnabled || !avoidWallClip)
                return;

            wallConstrainZoom = wallAvoider.GetDistance(
                zoomLimits.x,
                zoomLimits.y,
                wallClipRadius,
                wallClipLayerMask,
                pivot.transform.TransformDirection(Vector3.forward),
                transform.position,
                target.GetTransform(gameObject)
            );
        }

        float GetMobileRotation(float delta, float sensitivity)
        {
            return delta / Screen.width * sensitivity * 10f * Time.timeScale;
        }

        void RotationInput(ref float rotationX, ref float rotationY)
        {
            var sensitivityValue = sensitivity.GetValue(gameObject);

            if (Application.isMobilePlatform)
            {
                if (Input.touchCount == 0)
                    return;

                var mobileRect = CameraMotorTypeAdventure.MOBILE_RECT;
                var screenRect = new Rect(
                    Screen.width * mobileRect.x,
                    Screen.height * mobileRect.y,
                    Screen.width * mobileRect.width,
                    Screen.height * mobileRect.height
                );
                var touch = Input.touches.FirstOrDefault(x => x.phase == TouchPhase.Moved && screenRect.Contains(x.position));
                rotationX = GetMobileRotation(touch.deltaPosition.x, sensitivityValue.x);
                rotationY = GetMobileRotation(touch.deltaPosition.y, sensitivityValue.y);
                orbitInputTime = Time.time;
                return;
            }

            var hasInput = orbitInput == CameraMotorTypeAdventure.OrbitInput.MouseMove ||
                (orbitInput == CameraMotorTypeAdventure.OrbitInput.HoldLeftMouse && Input.GetMouseButton(0)) ||
                (orbitInput == CameraMotorTypeAdventure.OrbitInput.HoldRightMouse && Input.GetMouseButton(1)) ||
                (orbitInput == CameraMotorTypeAdventure.OrbitInput.HoldMiddleMouse && Input.GetMouseButton(2));

            if (!hasInput)
                return;

            var axisX = Input.GetAxisRaw(INPUT_MOUSE_X);
            var axisY = Input.GetAxisRaw(INPUT_MOUSE_Y);

            if (cursorLock != Cursor.lockState && !Mathf.Approximately(axisX + axisY, 0f))
            {
                cursorLock = Cursor.lockState;
                return;
            }

            rotationX = axisX * sensitivityValue.x * Time.timeScale;
            rotationY = axisY * sensitivityValue.y * Time.timeScale;

            if (!Mathf.Approximately(axisX, 0f) || !Mathf.Approximately(axisY, 0f))
                orbitInputTime = Time.time;
        }

        /**
         * Public API
         */
        public override void EnableMotor()
        {
            transform.rotation = Quaternion.Euler(targetRotationY, targetRotationX, 0f);

            desiredZoom = initialZoom;
            currentZoom = initialZoom;
            wallConstrainZoom = zoomLimits.y;

            motorEnabled = true;
            cursorLock = Cursor.lockState;
        }

        public override void DisableMotor()
        {
            motorEnabled = false;
        }

        public override void UpdateMotor()
        {
            float rotationX = 0.0f;
            float rotationY = 0.0f;

            if (allowOrbitInput)
                RotationInput(ref rotationX, ref rotationY);

            targetRotationX += rotationX;
            targetRotationY += rotationY;

            targetRotationX %= 360f;
            targetRotationY %= 360f;

            targetRotationY = Mathf.Clamp(targetRotationY, -maxPitch / 2.0f, maxPitch / 2.0f);

            var smoothTime = HookCamera.Instance != null ? HookCamera.Instance.Get<CameraController>().cameraSmoothTime.positionDuration : 0.1f;
            var targetTransform = target.GetTransform(gameObject);

            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetTransform.TransformPoint(targetOffset),
                ref _velocityPosition,
                smoothTime
            );

            var targetRotation = Quaternion.Euler(targetRotationY, targetRotationX, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * orbitSpeed);

            if (allowZoom)
                desiredZoom = Mathf.Clamp(desiredZoom - Input.GetAxis(INPUT_MOUSE_W) * zoomSensitivity, zoomLimits.x, zoomLimits.y);

            currentZoom = Mathf.Max(zoomLimits.x, desiredZoom);
            currentZoom = Mathf.Min(currentZoom, wallConstrainZoom);
            targetZoom = Mathf.Lerp(targetZoom, currentZoom, Time.deltaTime * zoomSpeed);

            var pivotPosition = (Vector3.forward * targetZoom) + pivotOffset;
            pivot.transform.localPosition = pivotPosition;
        }

        public override Vector3 GetPosition(CameraController camera, bool withoutSmoothing = false)
        {
            return pivot.transform.position;
        }

        public override Vector3 GetDirection(CameraController camera, bool withoutSmoothing = false)
        {
            return transform.TransformDirection(-Vector3.forward);
        }

        public override bool UseSmoothPosition()
        {
            return false;
        }

        public override bool UseSmoothRotation()
        {
            return false;
        }

        public void SetTargetRotation(float x, float y)
        {
            targetRotationX = x;
            targetRotationY = y;
        }
    }
}
