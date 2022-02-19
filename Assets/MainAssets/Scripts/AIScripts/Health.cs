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
    public AiAgent agent;

    // Start is called before the first frame update
    void Start()
    {
  
        currentHealth = maxHealth;
        
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
        AiDeathState deathState = agent.stateMachine.GetState(AiStateID.deathState) as AiDeathState;
        agent.stateMachine.ChangeState(AiStateID.deathState);
    }

}
