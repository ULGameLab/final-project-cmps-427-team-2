using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RaycastFromObject : MonoBehaviour
{
    private void Update()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, Camera.main.transform.forward * hit.distance, Color.red);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, Camera.main.transform.forward * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
