using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RetrievalQuest : MonoBehaviour
{
    public GameObject itemInHand;
    public GameObject[] enemies;

    public Material materialToChange;
    private Renderer ObjectInHand;
    private CapsuleCollider trigger;

    private Health enemyHealth;

    public int random;

    private void Start()
    {
        random = Random.Range(0, enemies.Length);
        print(random);
        ObjectInHand = itemInHand.GetComponent<Renderer>();
        ObjectInHand.material = materialToChange;
        trigger = itemInHand.GetComponent<CapsuleCollider>();
        enemyHealth = enemies[random].GetComponent<Health>();
        
    }

    private void Update()
    {
        if (enemyHealth.currentHealth <= 0)
        {
            DropItemInEnemyHand(itemInHand);
            trigger.enabled = true;
            this.enabled = false;
        }
    }

    public void DropItemInEnemyHand(GameObject itemInHand)
    {
      itemInHand.transform.parent = null;
    }
}
