using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCreator.Variables;

public class ManaBar : MonoBehaviour
{
    public float maxMana = 100f;
    public float currentMana;
    public float regenRate = 5f;
    public float rechargeDelay = 2;

    public float currentTimeDelay = 0;

    private GameObject player;
    private Slider slider;

    [HideInInspector]
    public bool usingMana;

    private void Start()
    {
        slider = GetComponent<Slider>();
        SetMaxManna(maxMana);
        currentMana = maxMana;
        player = GameObject.Find("Player");
        currentTimeDelay = rechargeDelay;

    }

    private void Update()
    {
        if (currentMana <= 0)
        {
            VariablesManager.SetLocal(player, "hasMana", false, false);
        }
        else
        {
            VariablesManager.SetLocal(player, "hasMana", true, true);
        }

        currentTimeDelay -= Time.deltaTime;

        if (!usingMana && currentTimeDelay <= 0)
        {
            RegenMana(regenRate);
        }
        
       
    }

    public void RegenMana(float regenAmount)
    {
        currentMana += ((regenRate / 100) * maxMana);
        SetManna(currentMana);
    }
    public void SetManna(float manna)
    {
        slider.value = manna;
        currentMana = slider.value;
    }

    public void SetMaxManna(float manna)
    {
        slider.maxValue = manna;
        slider.value = manna;
        currentMana = slider.value;
    }

    public void UseMana(float mana)
    {
        currentMana -= mana;
        SetManna(currentMana);
    }
}
