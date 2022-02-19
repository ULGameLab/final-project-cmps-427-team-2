using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public void Enter(AiAgent agent)
    {
       agent.ragdoll.ActivateRagdoll();
       agent.ui.gameObject.SetActive(false);
       agent.aILocomotion.enabled = false;
       agent.mesh.updateWhenOffscreen = true;
    }

    public void Exit(AiAgent agent)
    {
        
    }

    public AiStateID GetID()
    {
        return AiStateID.deathState;
    }

    public void Update(AiAgent agent)
    {
        
    }
}
