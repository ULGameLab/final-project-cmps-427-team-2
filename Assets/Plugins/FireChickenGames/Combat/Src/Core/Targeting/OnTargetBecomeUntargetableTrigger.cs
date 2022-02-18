namespace FireChickenGames.Combat.Core.Targeting
{
    using GameCreator.Core;
    using UnityEngine;

    [AddComponentMenu("")]
    public class OnTargetBecomeUntargetableTrigger : Igniter 
	{
		#if UNITY_EDITOR
        public new static string NAME = "Fire Chicken Games/Combat/On Target Become Untargetable";
        public new static string COMMENT = "Note: this trigger will execute multiple times if the target becomes targetable again after it is untargetable.";
        #endif

        public Targetable targetable;
        private bool isTriggered = false;

        private void Update()
        {
            if (targetable == null)
                return;

            var canBeTargeted = targetable.CanBeTargeted();

            if (isTriggered && canBeTargeted)
                // If target becomes targetable (again) after it is untargetable, mark as not triggered so the trigger will fire again.
                isTriggered = false;

            if (!isTriggered && !canBeTargeted)
            {
                ExecuteTrigger(gameObject);
                isTriggered = true;
            }
        }
	}
}