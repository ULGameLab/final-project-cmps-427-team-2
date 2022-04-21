using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    public QuestManager QuestManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("The object has collided with road.");
        QuestManager.Quest5Check();
    }
    void OnTriggerStay(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {

    }
}
