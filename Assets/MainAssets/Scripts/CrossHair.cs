using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public RectTransform AimCrossHair;
    private void Update()
    {
        Ray rayForCrossHair = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Did Hit");

            AimCrossHair.transform.position = hit.point;
            
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            AimCrossHair.position = rayForCrossHair.GetPoint(100f);
        }
    }
}
