namespace FireChickenGames.Combat.Core.Cameras
{
    using System.Linq;
    using GameCreator.Core.Hooks;
    using UnityEngine;

    public class WallDetector
    {
        public RaycastHit[] HitsBuffer { get; internal set; } = new RaycastHit[50];

        public float GetDistance(
            float zoomLimitX,
            float zoomLimitY,
            float wallClipRadius,
            LayerMask wallClipLayerMask,
            Vector3 direction,
            Vector3 cameraPosition,
            Transform targetTransform)
        {
            if (HookCamera.Instance == null)
                return zoomLimitY;

            var hitCount = Physics.SphereCastNonAlloc(
                cameraPosition + (direction * wallClipRadius),
                wallClipRadius,
                direction,
                HitsBuffer,
                zoomLimitY,
                wallClipLayerMask,
                QueryTriggerInteraction.Ignore
            );

            if (hitCount == 0)
                return zoomLimitY;

            var orderedHitDistance = HitsBuffer
                .Where(x => x.collider != null && x.collider.transform != null && x.collider.transform != targetTransform && !x.collider.transform.IsChildOf(targetTransform))
                .Select(x => x.distance + wallClipRadius)
                .OrderBy(x => x <= zoomLimitY);

            return orderedHitDistance.Any() ?
                Mathf.Clamp(orderedHitDistance.FirstOrDefault(), zoomLimitX, zoomLimitY) :
                zoomLimitY;
        }
    }
}
