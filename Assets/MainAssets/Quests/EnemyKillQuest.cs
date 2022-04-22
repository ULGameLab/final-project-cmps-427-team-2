using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillQuest : QuestManager
{
    public GameObject[] enemies;
    public int questNum;
    public int rewardSpellNum;

    public void Update()
    {
        if (checkAllDead())
        {
            QuestCheck(questNum, rewardSpellNum);
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
                char[] numOfEnemies = getDes(questNum).ToCharArray();
                
                numOfEnemies[numOfEnemies.Length - 4] = numOfDeadEnemies[0];
                BookHandler.UpdateQuestText(new string(numOfEnemies), Quest2Title);
            }
        }
        if(enemyDead == enemies.Length)
        {
            return true;
        }
        return false;
    }

}
