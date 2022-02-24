using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RangeAttackState : AiState
{
    float timeRemaining = Random.Range(2, 4);
    public void Enter(AiAgent agent)
    {
       
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateID GetID()
    {
        return AiStateID.RangeAttack;
    }

    public void Update(AiAgent agent)
    {
        agent.enemyTransform.LookAt(agent.playerTransform);
        if ((agent.distanceFromPlayer > agent.config.rangeAttackDistance) && !agent.meleeCharacter) agent.stateMachine.ChangeState(AiStateID.chasePlayer);

        if(timeRemaining <= 0)
        {
            int randomNumber = Random.Range(1, 4);
            agent.animator.SetTrigger("RangeAttack" + randomNumber);
            timeRemaining = Random.Range(2, 4); 
        }
        else
        {
            timeRemaining -= Time.deltaTime;
        }
        

    }

}
