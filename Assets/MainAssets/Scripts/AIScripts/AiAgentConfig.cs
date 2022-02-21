using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1;
    public float maxSightDistance = 5f;
    public float rangeAttackDistance = 3f;
    public float meleeAttackDistance = 1f;
}
