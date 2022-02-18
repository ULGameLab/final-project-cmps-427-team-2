namespace FireChickenGames.Combat.Core.Targeting
{
    using GameCreator.Core;
    using UnityEngine;

    [AddComponentMenu("")]
    public class OnContinuingToBeTargetedTrigger : Igniter
    {
#if UNITY_EDITOR
        public new static string NAME = "Fire Chicken Games/Combat/On Continuing To Be Targeted";
#endif

        void Start()
        {
            Targetable.AddListenerOnContinuingToBeTargeted(OnReceiveEvent);
        }

        private void OnDestroy()
        {
            Targetable.RemoveListenerOnContinuingToBeTargeted(OnReceiveEvent);
        }

        private void OnReceiveEvent(GameObject gameObject)
        {
            ExecuteTrigger(gameObject);
        }
    }
}