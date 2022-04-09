using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_MeleeAttackState : MeleeAttackState
{

    private Enemy3 enemy;

    public E3_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MeleeAttack stateData, Enemy3 enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        randomTimeBetweenAttacks = 0;
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

        MeleeAttackPlayer();
        entity.enemy.transform.LookAt(entity.player.transform.position);

        var rot = entity.enemy.transform.eulerAngles;
        entity.enemy.transform.rotation = Quaternion.Euler(new Vector3(0, rot.y, rot.z));

        if (randomTimeBetweenAttacks >= 0 && entity.distanceFromPlayer > entity.entityData.meleeAttackDistance)
        {
            stateMachine.ChangeState(enemy.rangeAttackState);
        }
        else if(entity.distanceFromPlayer > entity.entityData.maxSightDistance)
        {
            stateMachine.ChangeState(enemy.chaseState);
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

    public void MeleeAttackPlayer()
    {

        if (randomTimeBetweenAttacks <= 0)
        {
            if (!randomNumberSet)
            {
                randomNumber = Random.Range(1, stateData.numberOfMeleeAttacks + 1);
                randomNumberSet = true;
            }
            if (randomNumberSet)
            {
                entity.anim.SetTrigger("MeleeAttack" + randomNumber);
                randomTimeBetweenAttacks = Random.Range(stateData.lowerRandomTimeBetweenAttacksNumber, stateData.upperRandomTimeBetweenAttacksNumber);
                randomNumberSet = false;
            }

        }
        else
        {
            randomTimeBetweenAttacks -= Time.deltaTime;

        }
    }
}
