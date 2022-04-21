using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinQuest1 : MonoBehaviour
{
    public GameObject[] goblins;
    public QuestManager questManager;
    public BookBehavior BookHandler;
    

    public void Update()
    {
        if (checkAllDead())
        {
            questManager.QuestCheck(2, 2);
            this.enabled = false;
        }
    }

    public bool checkAllDead()
    {
        int goblinDead = 0;
        List<int> indexesDead = new List<int>();
        for(int i = 0; i < goblins.Length && goblinDead != goblins.Length; i++)
        {
            if (indexesDead.Contains(i))
            {
                continue;
            }

            if (!goblins[i].activeInHierarchy)
            {
                goblinDead++;
                indexesDead.Add(i);
                char[] numOfDeadGoblins = goblinDead.ToString().ToCharArray();
                char[] numOfGoblins = questManager.Quest2Des.ToCharArray();
                
                numOfGoblins[numOfGoblins.Length - 4] = numOfDeadGoblins[0];
                BookHandler.UpdateQuestText(new string(numOfGoblins), questManager.Quest2Title);
            }
        }
        if(goblinDead == goblins.Length)
        {
            return true;
        }
        return false;
    }

}
