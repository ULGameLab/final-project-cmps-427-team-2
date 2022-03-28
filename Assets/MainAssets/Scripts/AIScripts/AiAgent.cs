using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AiAgent : MonoBehaviour
{

    public AiStateMachine stateMachine;
    public AiStateID initializeState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public Ragdoll ragdoll;
    public UIHealthBar health;
    public SkinnedMeshRenderer mesh;
    public UIHealthBar ui;
    public AILocomotion aILocomotion;
    public Transform playerTransform;
    public Transform enemyTransform;
    public Animator animator;
    public float distanceFromPlayer;
    public BoxCollider wanderBounds;
    public float chaseSpeed;
    public bool meleeCharacter;
    public bool animationRunning;
    public bool arrowRunning;
    public bool randomNumberSet;

    //points around the player the melee characters will go to so they dont all flock together on top of each other
    public GameObject[] meleeFollowLocations; 
    public bool[] meleeLocationsOpen;



    // Start is called before the first frame update
    void Start()
    {
        EmptySpots();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        aILocomotion = GetComponent<AILocomotion>();
        ragdoll = GetComponent<Ragdoll>();
        animator = GetComponent<Animator>();
        health = GetComponent<UIHealthBar>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new RangeAttackState1());
        stateMachine.RegisterState(new AIWanderState());
        stateMachine.RegisterState(new MeleeAttackState1());
        stateMachine.ChangeState(initializeState);
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(playerTransform.position, enemyTransform.position);
        stateMachine.Update();
    }

    public void EmptySpots()
    {
        for(int i =0; i < meleeLocationsOpen.Length; i++)
        {
            meleeLocationsOpen[i] = false;
        }
    }

}
