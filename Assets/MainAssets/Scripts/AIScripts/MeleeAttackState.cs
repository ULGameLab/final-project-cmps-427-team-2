using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AiState
{
    float timeBetweenAttacks = Random.Range(2, 4);

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
        agent.enemyTransform.LookAt(agent.playerTransform);

        if ((agent.distanceFromPlayer > agent.config.rangeAttackDistance) && !agent.meleeCharacter) agent.stateMachine.ChangeState(AiStateID.chasePlayer);
        if ((agent.distanceFromPlayer > agent.config.meleeAttackDistance) && agent.meleeCharacter) agent.stateMachine.ChangeState(AiStateID.chasePlayer);

        if (timeBetweenAttacks <= 0)
        {
            int randomNumber = 1;          //picks a random number to choose a random attack from the animator 
            Debug.Log(randomNumber);
            agent.animator.SetTrigger("Attack" + randomNumber);
            timeBetweenAttacks = 2f;
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }
    }
}
