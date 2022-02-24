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
        private float currentElapsedTime;

        public KeyCode[] keysInCombo;

        bool[] buttons;

        private int start = 0;

       

        private void Start()
        {
            buttons = new bool[keysInCombo.Length];
            currentElapsedTime = elapse;
        }

        private void Update()
        {
            currentElapsedTime -= Time.deltaTime;
            KeyCombo(keysInCombo, keysInCombo.Length);
        }

        public bool areAllTrue(bool[] buttons)
        {
            for (int i = 0; i<buttons.Length; i++) if (buttons[i] == false) return false;
            return true;
        }

        public void KeyCombo(KeyCode[] keys, int numKeys)
        {
            
            if(start < numKeys)
            {
                if (Input.GetKeyDown(keys[start]) && currentElapsedTime != 0)
                {
                    buttons[start] = true;
                    start++;
                    currentElapsedTime = elapse;
                }
                if (currentElapsedTime <= 0)
                {
                    buttons[start] = false;
                    
                }

                bool buttonsAllTrue = areAllTrue(buttons);
                if (buttonsAllTrue)
                {
                    start = 0;
                    print("hello");
                    this.ExecuteTrigger();
                    for(int i =0; i <buttons.Length; i++)
                    {
                        buttons[i] = false;
                    }

                }

            }
            


            
        }
       
                
               
            
        }
	}