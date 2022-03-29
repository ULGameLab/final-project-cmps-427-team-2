using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_MeleeAttackState : MeleeAttackState
{

    private Enemy1 enemy;

    public E1_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MeleeAttack stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        Debug.Log(entity.AnimatorIsPlaying("MeleeAttacks"));
        if (!entity.AnimatorIsPlaying("MeleeAttacks"))
        {
            if (entity.distanceFromPlayer > entity.entityData.meleeAttackDistance)
            {
                if (isPlayerInMinAgroRange && entity.distanceFromPlayer > entity.entityData.meleeAttackDistance)
                {
                    stateMachine.ChangeState(enemy.chaseState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.moveState);
                }
            }
        }

        MeleeAttackPlayer();
        entity.enemy.transform.LookAt(entity.player.transform.position);

        var rot = entity.enemy.transform.eulerAngles;
        entity.enemy.transform.rotation = Quaternion.Euler(new Vector3(0, rot.y, rot.z));
           
        
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
