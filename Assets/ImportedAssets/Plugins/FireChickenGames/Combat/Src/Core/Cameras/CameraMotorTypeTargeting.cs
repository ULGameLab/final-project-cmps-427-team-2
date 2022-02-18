namespace FireChickenGames.Combat.Core.Cameras
{
    using UnityEngine;
    using GameCreator.Core;
    using GameCreator.Camera;
    
    public class CameraMotorTypeTargeting : ICameraMotorType
    {
        public static new string NAME = "Combat Targeting Camera";

        private GameObject pivot;
        public Vector3 pivotOffset = Vector3.zero;
        private bool motorEnabled;

        public float anchorDistance = 3.0f;
        public float horizontalOffset = 0f;
        public TargetGameObject anchor = new TargetGameObject(TargetGameObject.Target.Player);
        public Vector3 anchorOffset;
        public TargetPosition target = new TargetPosition(TargetPosition.Target.Position);

        private WallDetector wallAvoider;
        public bool avoidWallClip = true;
        public float wallClipRadius = 0.4f;
        public LayerMask wallClipLayerMask = ~4;

        private readonly float zoomLimitY = 10f;
        private readonly float zoomLimitX = 1f;
        private float wallConstrainZoom;

        private void Start()
        {
            wallAvoider = new WallDetector();
        }

        void Awake()
        {
            if (pivot == null)
                pivot = new GameObject(gameObject.name + " Pivot");
            pivot.transform.SetParent(transform);

            Vector3 pivotPosition = (Vector3.forward) + pivotOffset;

            pivot.transform.localRotation = Quaternion.identity;
            pivot.transform.localPosition = pivotPosition;
        }

        void FixedUpdate()
        {
            wallConstrainZoom = zoomLimitY;

            if (!motorEnabled || !avoidWallClip)
                return;

            wallConstrainZoom = wallAvoider.GetDistance(
                zoomLimitX,
                zoomLimitY,
                wallClipRadius,
                wallClipLayerMask,
                pivot.transform.TransformDirection(Vector3.forward),
                transform.position,
                anchor.GetTransform(gameObject)
            );
        }

        /**
         * Public API
         */
        public override void EnableMotor()
        {
            wallConstrainZoom = zoomLimitY;
            motorEnabled = true;
        }

        public override void DisableMotor()
        {
            motorEnabled = false;
        }

        public override void UpdateMotor()
        {
            var anchorTransform = anchor.GetTransform(gameObject);
            if (anchorTransform == null)
                return;

            var anchorPosition = anchorTransform.position + anchorTransform.TransformDirection(anchorOffset);
            var targetPosition = target.GetPosition(gameObject);

            transform.position = anchorPosition;
            transform.LookAt(targetPosition);

            var forwardDir = (anchorPosition - targetPosition).normalized;
            var lateralDir = transform.right;

            transform.position += forwardDir * (wallConstrainZoom < anchorDistance ? wallConstrainZoom : anchorDistance);
            transform.position += lateralDir * horizontalOffset;
        }

        public override Vector3 GetPosition(CameraController camera, bool withoutSmoothing = false)
        {
            return transform.position;
        }

        public override Vector3 GetDirection(CameraController camera, bool withoutSmoothing = false)
        {
            return transform.TransformDirection(Vector3.forward);
        }
    }
}
