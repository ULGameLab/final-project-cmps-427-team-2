namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class DestroyHealingAuraAction : IAction
	{
		

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

			var healingObject = GameObject.FindGameObjectWithTag("Healing");

			if ( healingObject != null)
            {
				Destroy(healingObject);
            }
            return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Custom/DestroyHealingAuraAction";
		#endif
	}
}
