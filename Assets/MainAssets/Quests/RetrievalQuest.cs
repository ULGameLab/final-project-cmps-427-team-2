using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RetrievalQuest : QuestManager
{
    public GameObject[] enemies;

    public Material materialToChange;
    private CapsuleCollider trigger;
    private GameObject childBow;

    private Health enemyHealth;

    private int random;

    private void Start()
    {
        random = Random.Range(0, enemies.Length);
        enemyHealth = enemies[random].GetComponent<Health>();
        childBow = enemies[random].transform.GetComponentInChildren<InteractableQuestWeapon>().gameObject;
        childBow.GetComponent<Renderer>().material = materialToChange;
        trigger = childBow.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (enemyHealth.currentHealth <= 0)
        {
            DropItemInEnemyHand(childBow);
            trigger.enabled = true;
            this.enabled = false;
        }
    }

    public void DropItemInEnemyHand(GameObject itemInHand)
    {
      itemInHand.transform.parent = null;
    }
}
