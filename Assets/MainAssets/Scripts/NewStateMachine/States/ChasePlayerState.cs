using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerState : State
{

    protected D_Chase stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performShortRangeAction;
    protected bool performLongRangeAction;

    protected float timer;

    public ChasePlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Chase stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        DoChecks();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        DoChecks();

    }


    public override void DoChecks() {
        isPlayerInMinAgroRange = entity.CheckPlayerINMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();

        performShortRangeAction = entity.CheckPlayerInCloseRange();
        performLongRangeAction = entity.CheckPlayerInFarAttackRange();
    }

}
