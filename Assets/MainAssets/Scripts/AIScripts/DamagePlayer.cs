using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public float damagePCT = 10;
    private float damageAmount;

    [HideInInspector]
    public HealthBar healthBar;


    private void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            TakeDamage((damagePCT/100) * healthBar.maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        healthBar.currentHealth -= damage;
        healthBar.SetHealth(healthBar.currentHealth);
    }

    private void OnParticleCollision(GameObject other)
    {
        print("collide");
        if(other.gameObject.name == "Player")
        {
            TakeDamage((damagePCT/100) * healthBar.maxHealth);
        }
    }

}
