using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AttackState : AiState
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
        return AiStateID.Attack;
    }

    public void Update(AiAgent agent)
    {
        agent.enemyTransform.LookAt(agent.playerTransform);

        if (agent.distanceFromPlayer > agent.config.attackDistance)
        {
            agent.stateMachine.ChangeState(AiStateID.chasePlayer);
        }
        if(timeRemaining <= 0)
        {
            int randomNumber = Random.Range(1, 4);
            Debug.Log(randomNumber);
            agent.animator.SetTrigger("RangeAttack" + randomNumber);
            timeRemaining = 2f;
        }
        else
        {
            timeRemaining -= Time.deltaTime;
        }
        

    }

}
