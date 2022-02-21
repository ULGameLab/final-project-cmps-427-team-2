using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWanderState : AiState
{
    float wanderRadius = 30;
    float wanderTimer = 5;
    Vector3 movePoint;
    bool movePointSet = false;

    private float timer;

    public void Enter(AiAgent agent)
    {
        
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateID GetID()
    {
        return AiStateID.WanderState;
    }

    public void Update(AiAgent agent)
    {

        agent.navMeshAgent.speed = 2f;
        if(agent.distanceFromPlayer < agent.config.maxSightDistance) agent.stateMachine.ChangeState(AiStateID.chasePlayer);

        Vector3 newPos = RandomNavSphere(agent.enemyTransform.position, wanderRadius, -1);

        if (agent.wanderBounds.bounds.Contains(newPos))
        {
            movePoint = newPos;
            movePointSet = true;
        }
        else
        {
            newPos = RandomNavSphere(agent.enemyTransform.position, wanderRadius, -1);
            movePointSet = false;
        }

            timer += Time.deltaTime;
            if((timer >= wanderTimer) && movePointSet)
            {
            agent.navMeshAgent.destination = movePoint;
            timer = 0;
            movePointSet = false;
            }

      
        
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(20, distance);

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
