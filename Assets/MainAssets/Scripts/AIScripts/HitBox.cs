using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;

    public void OnHit(ProjectileMoveScript spell)
    {
        health.takeDmg(spell.damage);
    }
    public void OnHeadHit(ProjectileMoveScript spell)
    {
        health.takeDmg(spell.damageHead);
    }
    public void OnLegsHit(ProjectileMoveScript spell)
    {
        health.takeDmg(spell.damageLegs);
    }

}
