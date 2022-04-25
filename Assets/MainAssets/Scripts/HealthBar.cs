using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameCreator.Variables;
using GameCreator.Characters;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public float maxHealth = 100f;
    public float currentHealth;
    public PlayerCharacter PlayerController;
    private GameObject player;

    private void Start()
    {
        slider = GetComponent<Slider>();
        SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            VariablesManager.SetLocal(player, "alive", false, false);
            PlayerController.enabled = false;
            PlayerPrefs.SetInt("SavedInteger", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(0);

        }
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        currentHealth = slider.value;
    }
    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        currentHealth = slider.value;
    }

}
