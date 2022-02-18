namespace FireChickenGames.Combat.Core.Targeting
{
    using GameCreator.Core;
    using UnityEngine;

    [AddComponentMenu("")]
    public class OnTargetChangedTrigger : Igniter
    {
#if UNITY_EDITOR
        public new static string NAME = "Fire Chicken Games/Combat/On Target Changed";
#endif

        void Start()
        {
            Targetable.AddListenerOnTargetChanged(OnReceiveEvent);
        }

        private void OnDestroy()
        {
            Targetable.RemoveListenerOnTargetChanged(OnReceiveEvent);
        }

        private void OnReceiveEvent(GameObject gameObject)
        {
            ExecuteTrigger(gameObject);
        }
    }
}