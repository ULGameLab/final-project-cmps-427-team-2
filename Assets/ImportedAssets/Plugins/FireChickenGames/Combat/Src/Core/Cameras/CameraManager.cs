namespace FireChickenGames.Combat.Core.Cameras
{
    using System.Collections;
    using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using UnityEngine;

    public class CameraManager
    {
        public static IEnumerator ChangeFieldOfView(float fieldOfView, float duration = 0.5f)
        {
            var camera = HookCamera.Instance.Get<Camera>();
            if (Mathf.Approximately(duration, 0f))
            {
                camera.fieldOfView = fieldOfView;
                yield break;
            }

            var easedFieldOfView = 0f;
            var initialFieldOfView = camera.fieldOfView;
            while (easedFieldOfView <= 1f)
            {
                easedFieldOfView += Time.deltaTime / duration;
                camera.fieldOfView = Easing.ExpoInOut(initialFieldOfView, fieldOfView, easedFieldOfView);
                yield return null;
            }

            camera.fieldOfView = fieldOfView;
            yield return 0;
        }
    }
}
