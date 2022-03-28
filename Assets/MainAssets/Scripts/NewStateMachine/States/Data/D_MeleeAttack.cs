using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newAttackState", menuName = "Data/State Data/Melee Attack State")]
public class D_MeleeAttack : ScriptableObject
{
    public float upperRandomTimeBetweenAttacksNumber = 2f;
    public float lowerRandomTimeBetweenAttacksNumber = 5f;
    public int numberOfMeleeAttacks = 3;
    public float[] attackDamage;
}
