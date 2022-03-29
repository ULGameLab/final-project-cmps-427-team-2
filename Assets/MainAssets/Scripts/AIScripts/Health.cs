using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public Animator animator;
    
    public float maxHealth;
    public float currentHealth;
    public UIHealthBar healthbar;
    public GameObject healthCanvaas;

    private AILocomotion locomotion;
    private NavMeshAgent agent;
    private Ragdoll ragdoll;


    // Start is called before the first frame update
    void Start()
    {
  
        currentHealth = maxHealth;
        ragdoll = GetComponent<Ragdoll>();
        locomotion = GetComponent<AILocomotion>();
        agent = GetComponent<NavMeshAgent>();
        
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidBodies)
        {
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.health = this;
        }
    }


    public void takeDmg(float dmg)
    {
        currentHealth -= dmg;
        animator.SetTrigger("takeDmg");
        healthbar.setHealthBarPercentage(currentHealth / maxHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ragdoll.ActivateRagdoll();
        locomotion.enabled = false;
        healthCanvaas.SetActive(false);
        agent.enabled = false;
        //skinnedMesh.updateWhenOffscreen = true;
        this.enabled = false;
    }

}
