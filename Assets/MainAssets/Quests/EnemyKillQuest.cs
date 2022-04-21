using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillQuest : MonoBehaviour
{
    public GameObject[] enemies;
    public QuestManager questManager;
    public BookBehavior BookHandler;
    public int questNum;
    public int rewardSpellNum;

    public void Update()
    {
        if (checkAllDead())
        {
            questManager.QuestCheck(questNum, rewardSpellNum);
            this.enabled = false;
        }
    }

    public bool checkAllDead()
    {
        int enemyDead = 0;
        List<int> indexesDead = new List<int>();
        for(int i = 0; i < enemies.Length && enemyDead != enemies.Length; i++)
        {
            if (indexesDead.Contains(i))
            {
                continue;
            }

            if (!enemies[i].activeInHierarchy)
            {
                enemyDead++;
                indexesDead.Add(i);
                char[] numOfDeadEnemies = enemyDead.ToString().ToCharArray();
                char[] numOfEnemies = questManager.getDes(questNum).ToCharArray();
                
                numOfEnemies[numOfEnemies.Length - 4] = numOfDeadEnemies[0];
                BookHandler.UpdateQuestText(new string(numOfEnemies), questManager.Quest2Title);
            }
        }
        if(enemyDead == enemies.Length)
        {
            return true;
        }
        return false;
    }

}
