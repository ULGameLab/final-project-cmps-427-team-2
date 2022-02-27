namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class InstantiateAsChildAction : IAction
	{
		public GameObject objectToInstantiate;
		public Transform locationToInstantiateAsChild;

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			GameObject instantiatedObject = Instantiate(objectToInstantiate, locationToInstantiateAsChild);
			
			return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Custom/InstantiateAsChildAction";
		#endif
	}
}
