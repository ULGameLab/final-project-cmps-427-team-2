using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public float damagePCT = 10;
    private float damageAmount;

    private ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> particleCollisionEvents;

    [HideInInspector]
    public HealthBar healthBar;


    private void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        particleSystem = GetComponent<ParticleSystem>();
        particleCollisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            TakeDamage((damagePCT/100) * healthBar.maxHealth);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, other, particleCollisionEvents);
        for (int i = 0; i < particleCollisionEvents.Count; i++)
        {
            var collider = particleCollisionEvents[i].colliderComponent;
            if (collider.CompareTag("Player"))
            {
                TakeDamage((damagePCT / 100) * healthBar.maxHealth);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        healthBar.currentHealth -= damage;
        healthBar.SetHealth(healthBar.currentHealth);
    }
    }

