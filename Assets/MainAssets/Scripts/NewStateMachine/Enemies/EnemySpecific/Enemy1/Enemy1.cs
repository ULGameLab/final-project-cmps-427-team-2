using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
  public E1_IdleState idleState { get; private set; }
  public E1_MoveState moveState { get; private set; }
  public E1_ChaseState chaseState { get; private set; }
  public E1_MeleeAttackState meleeAttackState { get; private set; }

    public GameObject uiHealthBar;
    public GameObject MapIcon;
    private Health health;


    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_Chase chasePlayerData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    public override void Start()
    {
        base.Start();
        moveState = new E1_MoveState(this, stateMachine, "move", moveStateData, this);
        chaseState = new E1_ChaseState(this, stateMachine, "PlayerDetected", chasePlayerData, this);
        meleeAttackState = new E1_MeleeAttackState(this, stateMachine, "meleeAtack", meleeAttackStateData, this);
        health = GetComponent<Health>();

        stateMachine.Initialize(moveState);
       
    }

    public override void Update()
    {
        base.Update();
        if(health.currentHealth <= 0)
        {
            uiHealthBar.SetActive(false);
            MapIcon.SetActive(false);
            RagdollDeath();
            this.enabled = false;
        }
    }
}
