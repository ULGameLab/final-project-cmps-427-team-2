using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road1 : MonoBehaviour
{
    public QuestManager QuestManager;
    // Once road is collided the quest is finished.
    // A certain point has to have a colllider.
    // Once that collder is met then boom quest ends.
    // Start is called before the first frame update
   
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("The object has collided with road.");
        QuestManager.Quest1Check();
    }

    void OnTriggerStay(Collider other)
    {

    }
    void OnTriggerExit(Collider other)
    {

    }
     
}
