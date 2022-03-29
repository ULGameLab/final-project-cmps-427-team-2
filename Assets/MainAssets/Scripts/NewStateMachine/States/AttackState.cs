using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{

    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;
    protected bool performShortRangeAction;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //entity.animationToStateMachine.attackState = this;
        //isAnimationFinished = false;
        entity.SetSpeed(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        DoChecks();
       


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        DoChecks();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerINMinAgroRange();
        performShortRangeAction = entity.CheckPlayerInCloseRange();

    }

    public virtual void TriggerAttack()
    {
        isAnimationFinished = false;
       
    }

    public virtual void FinishAttack()
    {
        isAnimationFinished = true;
    }
}
