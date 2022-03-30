using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newRangeAttackStateData", menuName = "Data/State Data/RangeAttack State")]
public class D_RangeAttack : ScriptableObject
{
    public float upperRandomTimeBetweenAttacksNumber = 2f;
    public float lowerRandomTimeBetweenAttacksNumber = 5f;
    public int numberOfRangeAttacks = 3;
    public float[] attackDamage;
}
