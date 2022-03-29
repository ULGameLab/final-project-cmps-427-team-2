using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : AttackState
{
    protected D_RangeAttack stateData;
    protected float randomTimeBetweenAttacks;
    protected float currentTime;
    protected bool randomNumberSet;
    protected int randomNumber;

    public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RangeAttack stateData) : base(entity, stateMachine, animBoolName)
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

    public void RangeAttack()
    {
        if (randomTimeBetweenAttacks <= 0)
        {
            if (!randomNumberSet)
            {
                randomNumber = Random.Range(1, stateData.numberOfRangeAttacks + 1);
                randomNumberSet = true;
            }
            if (randomNumberSet)
            {
                entity.anim.SetTrigger("RangeAttack" + randomNumber);
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
