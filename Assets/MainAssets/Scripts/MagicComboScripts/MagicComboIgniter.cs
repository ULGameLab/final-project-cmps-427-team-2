namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    [AddComponentMenu("")]
    public class MagicComboIgniter : Igniter 
	{
		#if UNITY_EDITOR
        public new static string NAME = "MagicCombo/MagicCombo";
        //public new static string COMMENT = "Uncomment to add an informative message";
        //public new static bool REQUIRES_COLLIDER = true; // uncomment if the igniter requires a collider
#endif
        public float elapse = 5f;
        public float coolDown = 5f;

        private float currentElapsedTime;
        private float currentCoolDown;

        public KeyCode[] keysInCombo;

        bool[] buttons;

        private int currentKey = 0;

       

        private void Start()
        {
            buttons = new bool[keysInCombo.Length];
            currentElapsedTime = elapse;
            currentCoolDown = coolDown;
        }

        private void Update()
        {
            KeyCombo(keysInCombo);
        }

        public bool areAllTrue(bool[] buttons)
        {
            for (int i = 0; i<buttons.Length; i++) if (buttons[i] == false) return false;
            return true;
        }

        public void setAllButtonsFalse(bool[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++) buttons[i] = false;
            
        }

        public void KeyCombo(KeyCode[] keys)
        {
            currentElapsedTime -= Time.deltaTime;
            currentCoolDown -= Time.deltaTime;

            if (currentKey < keys.Length && currentCoolDown <= 0)
            {
                if (Input.GetKeyDown(keys[currentKey]) && currentElapsedTime >= 0)
                {
                    buttons[currentKey] = true;
                    currentKey++;
                    currentElapsedTime = elapse;
                }
                if (currentElapsedTime <= 0)
                {
                    setAllButtonsFalse(buttons);
                    currentKey = 0;
                    currentElapsedTime = elapse;
                    
                }

                bool buttonsAllTrue = areAllTrue(buttons);
                if (buttonsAllTrue && currentCoolDown < 0)
                {
                    currentKey = 0;
                    currentCoolDown = coolDown;
                    currentElapsedTime = elapse;
                    this.ExecuteTrigger();
                    setAllButtonsFalse(buttons);

                }
            }
            


            
        }
       
                
               
            
        }
	}