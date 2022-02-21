using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public enum AiStateID{
        chasePlayer,
        deathState,
        Idle,
        Attack,
        WanderState

    }

public interface AiState
{
    AiStateID GetID();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
