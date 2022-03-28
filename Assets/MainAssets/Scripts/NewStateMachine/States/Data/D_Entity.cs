using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1;
  
    public float meleeAttackDistance = 1f;
    public float rangeAttackDistance = 4f;

    public float minSightDistance = 5f;
    public float maxSightDistance = 7f;


}
