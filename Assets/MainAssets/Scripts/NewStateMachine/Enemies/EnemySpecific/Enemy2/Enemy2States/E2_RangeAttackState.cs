using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_RangeAttackState : RangeAttackState
{
    private Enemy2 enemy;
    public E2_RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RangeAttack stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        timeToAttack -= Time.time;
        if(timeToAttack <= 0)
        {
            RangeAttackPlayer();
            entity.enemy.transform.LookAt(entity.player.transform.position);

            var rot = entity.enemy.transform.eulerAngles;
            entity.enemy.transform.rotation = Quaternion.Euler(new Vector3(0, rot.y, rot.z));

            if (entity.distanceFromPlayer > entity.entityData.meleeAttackDistance)
            {
                if (entity.distanceFromPlayer > entity.entityData.rangeAttackDistance)
                {
                    stateMachine.ChangeState(enemy.chaseState);
                }
                else if (entity.distanceFromPlayer > entity.entityData.maxSightDistance)
                {
                    stateMachine.ChangeState(enemy.moveState);
                }
            }
            else if (entity.distanceFromPlayer < entity.entityData.meleeAttackDistance)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
        }
      
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
