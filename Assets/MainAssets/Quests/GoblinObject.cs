using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinObject : MonoBehaviour
{
    public QuestManager QuestManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("The object has collided with road.");
        QuestManager.startQuest2();
    }
    
    private void EndQuest()
    {
        if (QuestManager.goblinskilled == 1)
        {
            Debug.Log("The quest is finished.");
            QuestManager.Quest2Check();
        }

    }
}



