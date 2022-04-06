namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Variables;

	[AddComponentMenu("")]
	public class HealPlayer : IAction
	{
		public float HealPCT = 10;
		public float manaCost = 10f;
		private HealthBar healthBar;
		private ManaBar manaBar;

		private void Awake()
        {
			healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
			manaBar = GameObject.Find("ManaBar").GetComponent<ManaBar>();

		}

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			Heal(HealPCT);
			manaBar.UseMana(manaCost);
            return true;
        }

		public void Heal(float HealPCT)
		{
			healthBar.currentHealth += ((HealPCT / 100) * healthBar.maxHealth);
			healthBar.SetHealth(healthBar.currentHealth);
		}

		

#if UNITY_EDITOR
		public static new string NAME = "Custom/HealPlayer";
		#endif
	}
}
