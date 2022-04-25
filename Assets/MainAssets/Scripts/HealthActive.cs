using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthActive : MonoBehaviour
{
    public GameObject Health;
    public GameObject Healthy;

    bool active = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (active == true)
            {
                Health.SetActive(false);
                Healthy.SetActive(false);
                active = false;
            }
            else
            {
                Health.SetActive(true);
                Healthy.SetActive(true);
                active = true;
            }
        }
    }
}
