using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayerState : AiState
{
    
    
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
        agent.navMeshAgent.speed = 3f;
        if (agent.distanceFromPlayer < agent.config.attackDistance)
        {
            agent.stateMachine.ChangeState(AiStateID.Attack);
        }
        if (agent.distanceFromPlayer > agent.config.maxSightDistance)
        {
            agent.stateMachine.ChangeState(AiStateID.WanderState);
        }

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
