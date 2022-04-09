using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class E3_ChaseState : ChasePlayerState
{
    private Enemy3 enemy;
    private float timeToMove;
    public E3_ChaseState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Chase stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        timeToMove = 2f;
      
        //timer = 0;


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        timeToMove -= Time.time;
        if(timeToMove <= 0)
        {
            entity.SetSpeed(stateData.chaseSpeed);
            ChasePlayer();
            if (entity.distanceFromPlayer < entity.entityData.rangeAttackDistance && !performShortRangeAction)
            {
                stateMachine.ChangeState(enemy.rangeAttackState);
            }

            if (performShortRangeAction && entity.distanceFromPlayer < entity.entityData.meleeAttackDistance)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }

            if (!isPlayerInMaxAgroRange)
            {
                enemy.stateMachine.ChangeState(enemy.moveState);
            }
        }
       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void ChasePlayer()
    {
        if (!enemy.navMeshAgent.enabled)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (!enemy.navMeshAgent.hasPath)
        {
            enemy.navMeshAgent.destination = enemy.player.transform.position;
        }

        if (timer < 0.0f)
        {
            Vector3 direction = (enemy.player.transform.position - enemy.navMeshAgent.destination);
            direction.y = 0;

            if (direction.sqrMagnitude > enemy.entityData.maxDistance * enemy.entityData.maxDistance)
            {
                if (enemy.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                {
                    enemy.navMeshAgent.destination = enemy.player.transform.position;
                }
            }
            timer = enemy.entityData.maxTime;
        }
    }
}
