using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AiState
{
    
    float timeBetweenAttacks = Random.Range(2, 4);
    float timeBeforeStandStillAttacks = 9;
    float currentTime;

    public void Enter(AiAgent agent)
    {
        
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateID GetID()
    {
        return AiStateID.MeleeAttack;
    }

    public void Update(AiAgent agent)
    {
        //currentTime += Time.deltaTime;
        //agent.enemyTransform.LookAt(agent.playerTransform);
        
        //was trying to get the player to only do certain attacks when the player barely moves around 
        //I failed may come back

        //if ((agent.playerTransform.position.x > 5 || agent.playerTransform.position.z > 5) && currentTime >= timeBeforeStandStillAttacks)
        //{
        //    //do standstillAttacks
        //    int randomNumber = 1;
        //    agent.animator.SetTrigger("StandStillAttack" + randomNumber);
        //    timeBetweenAttacks = Random.Range(2, 4);
        //    currentTime = 0;
        //}
        

        if ((agent.distanceFromPlayer > agent.config.rangeAttackDistance) && !agent.meleeCharacter) agent.stateMachine.ChangeState(AiStateID.chasePlayer);
        if ((agent.distanceFromPlayer > agent.config.meleeAttackDistance) && agent.meleeCharacter) agent.stateMachine.ChangeState(AiStateID.chasePlayer);

        if (timeBetweenAttacks <= 0)
        {
            int randomNumber = Random.Range(1,4);          //picks a random number to choose a random attack from the animator 
            agent.animator.SetTrigger("Attack" + randomNumber);
            timeBetweenAttacks = Random.Range(3, 5);
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }
    }
}
