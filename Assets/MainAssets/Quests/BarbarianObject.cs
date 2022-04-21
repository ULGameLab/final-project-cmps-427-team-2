using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianObject : MonoBehaviour
{
    public QuestManager QuestManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("The object has collided with road.");
        QuestManager.startQuest4();
    }

    private void EndQuest()
    {
        if (QuestManager.barbarianskilled == 5)
        {
            Debug.Log("The quest is finished.");
            QuestManager.QuestCheck(4, 1);
        }

    }
}
