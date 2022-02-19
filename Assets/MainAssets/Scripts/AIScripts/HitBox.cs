using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MagicShot")
        {
            health.takeDmg(10);
        }
    }

}
