using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_MoveState : MoveState
{

    private Enemy1 enemy;


    public E1_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        Wander();
        if (isPlayerInMinAgroRange)
        {
            
            stateMachine.ChangeState(enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    public void Wander()
    {
        Vector3 newPos = RandomNavSphere(entity.enemy.position, stateData.wanderRadius, -1);

        if (entity.wanderBounds.bounds.Contains(newPos))
        {
            movePoint = newPos;
            movePointSet = true;
        }
        else
        {
            newPos = RandomNavSphere(entity.enemy.position, stateData.wanderRadius, -1);
            movePointSet = false;
        }

        timer += Time.deltaTime;
        if ((timer >= stateData.wanderTimer) && movePointSet)
        {
            entity.navMeshAgent.destination = movePoint;
            timer = 0;
            movePointSet = false;
        }

    }


}
