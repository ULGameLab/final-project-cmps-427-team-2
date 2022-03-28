using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newMoveStateData", menuName = "Data/State Data/Move State")]
public class D_MoveState : ScriptableObject
{
    public float walkSpeed = 6f;
    public float wanderRadius = 30f;
    public float wanderTimer = 5f;

}
