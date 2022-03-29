using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttack stateData;
    protected float randomTimeBetweenAttacks;
    protected float currentTime;
    protected bool randomNumberSet;
    protected int randomNumber;

    
    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MeleeAttack stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //randomTimeBetweenAttacks = Random.Range(stateData.lowerRandomTimeBetweenAttacksNumber, stateData.upperRandomTimeBetweenAttacksNumber);
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
            if(randomNumberSet)
            {
                entity.anim.SetTrigger("Attack" + randomNumber);
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
