using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_IdleState idleState { get; private set; }
    public E2_MoveState moveState { get; private set; }
    public E2_ChaseState chaseState { get; private set; }
    public E2_MeleeAttackState meleeAttackState { get; private set; }
    public E2_RangeAttackState rangeAttackState { get; private set; }

    public GameObject uiHealthBar;

    private Health health;



    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_Chase chasePlayerData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_RangeAttack rangeAttackStateData;
    public override void Start()
    {
        base.Start();

        health = GetComponent<Health>();
        moveState = new E2_MoveState(this, stateMachine, "move", moveStateData, this);
        chaseState = new E2_ChaseState(this, stateMachine, "PlayerDetected", chasePlayerData, this);
        meleeAttackState = new E2_MeleeAttackState(this, stateMachine, "meleeAtack", meleeAttackStateData, this);
        rangeAttackState = new E2_RangeAttackState(this, stateMachine, "rangeAttack", rangeAttackStateData, this);

        stateMachine.Initialize(moveState);

    }
    public override void Update()
    {
        base.Update();
        if (health.currentHealth <= 0)
        {
            uiHealthBar.SetActive(false);
            RagdollDeath();
            this.enabled = false;
        }
    }
}
