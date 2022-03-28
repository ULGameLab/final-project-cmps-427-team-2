using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RangeAttackState1 : AiState
{
    int randomNumber;
    
    public float timeRemaining = Random.Range(2, 4);
    public float currentTimeRemaining;
    public void Enter(AiAgent agent)
    {
       
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateID GetID()
    {
        return AiStateID.RangeAttack;
    }

    public void Update(AiAgent agent)
    {
        Debug.Log(randomNumber);
        

        if (agent.animator.GetCurrentAnimatorStateInfo(0).IsTag("Arrow")) //if animation with the tag Arrow is running returns true
        {
            agent.animationRunning = true;
        }
        else
        {
            agent.animationRunning = false;
        }

        if (agent.animator.GetCurrentAnimatorStateInfo(1).IsTag("MovingArrow")) //if animation with the tag MvoingArrow is running returns true
        {
            agent.arrowRunning = true;
        }
        else
        {
            agent.arrowRunning = false;
        }


        agent.transform.LookAt(agent.playerTransform);
        
        if ((agent.distanceFromPlayer > agent.config.rangeAttackDistance) && !agent.meleeCharacter && (!agent.animationRunning) && !agent.randomNumberSet) agent.stateMachine.ChangeState(AiStateID.chasePlayer);

        if(timeRemaining <= 0)
        {

            if (!agent.randomNumberSet)
            {
                randomNumber = Random.Range(1, 4);
                agent.randomNumberSet = true;
            }
            
            

            if (agent.enemyTransform.tag == "GoblinArcher" && randomNumber == 2 && !agent.animationRunning && agent.randomNumberSet)
                {
                    agent.arrowRunning = true;
                    agent.StartCoroutine(Wait(.5f));
                    agent.animator.SetTrigger("RangeAttack" + randomNumber);
                    timeRemaining = Random.Range(2, 4);
                    agent.stateMachine.ChangeState(AiStateID.chasePlayer);
                 }
            else if(agent.randomNumberSet && !agent.animationRunning)
                {
                    agent.StartCoroutine(Wait(.5f));
                    agent.animator.SetTrigger("RangeAttack" + randomNumber);
                    timeRemaining = Random.Range(2, 4);
                    agent.randomNumberSet = false;
                    agent.animationRunning = true;
                }
        }

        else
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    public IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    

}
