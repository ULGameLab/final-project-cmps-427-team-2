using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC3 : MonoBehaviour
{
    public QuestManager QuestManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("The object has collided with player.");
        QuestManager.startQuest5();
    }
    void OnTriggerStay(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {

    }
}
