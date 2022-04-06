namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class StopUsingMana : IAction
	{
		private ManaBar manaBar;

        private void Awake()
        {
			manaBar = GameObject.Find("ManaBar").GetComponent<ManaBar>();
        }

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			manaBar.usingMana = false;
            return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Custom/StopUsingMana";
		#endif
	}
}
