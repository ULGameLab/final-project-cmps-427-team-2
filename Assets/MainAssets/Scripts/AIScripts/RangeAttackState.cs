using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RangeAttackState : AiState
{
    
    float timeRemaining = Random.Range(2, 4);
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
        
        if ((agent.distanceFromPlayer > agent.config.rangeAttackDistance) && !agent.meleeCharacter && (!agent.animationRunning)) agent.stateMachine.ChangeState(AiStateID.chasePlayer);

        if(timeRemaining <= 0)
        {

            
            int randomNumber = Random.Range(1, 4);
            

            if (agent.enemyTransform.tag == "GoblinArcher" && randomNumber == 2 && !agent.animationRunning)
                {
                    agent.arrowRunning = true;
                    agent.animator.SetTrigger("RangeAttack" + randomNumber);
                    timeRemaining = Random.Range(2, 4);
                    
                   
                
            }
                else
                {
                
                agent.animator.SetTrigger("RangeAttack" + randomNumber);
                timeRemaining = Random.Range(2, 4);
                }
        }

        else
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    public void Attacks(AiAgent agent)
    {

    }

    

}
