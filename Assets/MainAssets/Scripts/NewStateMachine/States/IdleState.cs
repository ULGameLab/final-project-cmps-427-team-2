using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;
    protected bool isPlayerInMinAgroRange;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        entity.SetSpeed(0f);
        isPlayerInMinAgroRange = entity.CheckPlayerINMinAgroRange();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        isPlayerInMinAgroRange = entity.CheckPlayerINMinAgroRange();

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

}
