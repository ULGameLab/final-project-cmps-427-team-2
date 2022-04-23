namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    [AddComponentMenu("")]
    public class Death : Igniter 
	{
        private HealthBar healthBar;

        private void Awake()
        {
            healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        }

#if UNITY_EDITOR
        public new static string NAME = "My Igniters/Death";
        //public new static string COMMENT = "Uncomment to add an informative message";
        //public new static bool REQUIRES_COLLIDER = true; // uncomment if the igniter requires a collider
        #endif

        private void Update()
        {
            if (healthBar.currentHealth <= 0)
            {
                this.ExecuteTrigger(gameObject);
            }
        }
       
    }
}