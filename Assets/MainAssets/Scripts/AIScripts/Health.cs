using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public Animator animator;
    public GameObject uiHealthBarGameObject;
    
    public float maxHealth;
    public float currentHealth;
    public UIHealthBar healthbar;
    public float healthBarDistance = 0;

    private float distnaceFromPlayer;

    private Transform player;
    private Transform enemy;

    public GameObject uiHealthBar;
    private Ragdoll ragdoll;
    private AILocomotion aILocomotion;
    public AiAgent aiAgent;

    // Start is called before the first frame update
    void Start()
    {
  
        currentHealth = maxHealth;
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemy = GetComponent<Transform>();
        aILocomotion = GetComponent<AILocomotion>();
        ragdoll = GetComponent<Ragdoll>();
        
        
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidBodies)
        {
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.health = this;
        }
    }
    private void Update()
    {
        distnaceFromPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (distnaceFromPlayer > healthBarDistance)
        {
            uiHealthBarGameObject.SetActive(false);
        }
        else if (distnaceFromPlayer < healthBarDistance)
        {
            uiHealthBarGameObject.SetActive(true);
        }
        

    }


    public void takeDmg(float dmg)
    {
        currentHealth -= dmg;
        //animator.SetTrigger("takeDmg");
        healthbar.setHealthBarPercentage(currentHealth / maxHealth);
    }

  

}
