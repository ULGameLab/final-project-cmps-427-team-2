using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{

    public float damagePCT;
    private ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> particleCollisionEvents;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleCollisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        //print("Hello");
        ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, other, particleCollisionEvents);
        for(int i = 0; i < particleCollisionEvents.Count; i++)
        {
            var collider = particleCollisionEvents[i].colliderComponent;
            if (collider.CompareTag("Player"))
            {
                print("Player Hit");
            }
        }
    }
}
