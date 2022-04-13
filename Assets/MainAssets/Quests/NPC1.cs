using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : MonoBehaviour
{
    public QuestManager QuestManager;
    public Dialogue_Display dDisplay;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "p1")
        {
            Debug.Log("The object has collided with player.");
            //QuestManager.startQuest1();
            dDisplay.Initialize();

        }
    }
    public void OnTriggerStay(Collider other)
    {
    }
    public void OnTriggerExit(Collider other)
    {
        //QuestManager.startQuest1();
    }

}
