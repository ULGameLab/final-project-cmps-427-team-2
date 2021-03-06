using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{

    float timeForGoblinRunArcherAttack = 3.04f;
    float timer = 0f;

    public void Enter(AiAgent agent)
    {
        
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateID GetID()
    {
        return AiStateID.chasePlayer;
    }

    public void Update(AiAgent agent)
    {
        timeForGoblinRunArcherAttack -= Time.deltaTime;

        if(timeForGoblinRunArcherAttack < 0 && agent.arrowRunning)
        {
            timeForGoblinRunArcherAttack = 3.04f;
            agent.arrowRunning = false;
            agent.randomNumberSet = false;
            agent.stateMachine.ChangeState(AiStateID.RangeAttack);

        }


        agent.navMeshAgent.speed = agent.chaseSpeed;

        //conditions to change states
        if ((agent.distanceFromPlayer < agent.config.rangeAttackDistance) && !agent.meleeCharacter) agent.stateMachine.ChangeState(AiStateID.RangeAttack);
        if ((agent.distanceFromPlayer < agent.config.meleeAttackDistance) && agent.meleeCharacter) agent.stateMachine.ChangeState(AiStateID.MeleeAttack);
        if (agent.distanceFromPlayer > agent.config.maxSightDistance) agent.stateMachine.ChangeState(AiStateID.WanderState);
        


        if (!agent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;

            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    agent.navMeshAgent.destination = agent.playerTransform.position;
                }
            }
            timer = agent.config.maxTime;
        }
    }
}
