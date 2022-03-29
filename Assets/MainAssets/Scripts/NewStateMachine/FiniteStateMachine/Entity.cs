using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public NavMeshAgent navMeshAgent;

    public BoxCollider wanderBounds;

    public Animator anim {get; private set;}
    public AILocomotion aILocomotion { get; private set; }
    public GameObject player { get; private set; }
    public Transform enemy { get; private set; }

    public AnimationToStateMachine animationToStateMachine { get; private set; }

    [HideInInspector]
    public float distanceFromPlayer;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        aILocomotion = GetComponent<AILocomotion>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animationToStateMachine = GetComponent<AnimationToStateMachine>();
        enemy = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        stateMachine = new FiniteStateMachine();

        distanceFromPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

    }

    public virtual void Update()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
       
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    public virtual bool CheckPlayerINMinAgroRange()
    {
        if (distanceFromPlayer < entityData.minSightDistance)
        {
            return true;
        }
        return false;
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        if (distanceFromPlayer < entityData.maxSightDistance)
        {
            return true;
        }
        return false;
    }

    public virtual bool CheckPlayerInCloseRange()
    {
        if(distanceFromPlayer < entityData.meleeAttackDistance)
        {
            return true;
        }
        return false;
    }

    public virtual bool CheckPlayerInFarAttackRange()
    {
        if (distanceFromPlayer < entityData.rangeAttackDistance)
        {
            return true;
        }
        return false;
    }

    //checks to see if the animation with specific tag is playing
    public virtual bool AnimatorIsPlaying(string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag(stateName))
        {
            return true;
        }
        return false;
    }
}
