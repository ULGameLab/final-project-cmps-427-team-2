using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CrossHair : MonoBehaviour
{
    public RectTransform crosshair;
    public Camera cam;
    Vector3 defaultPostion;

    private void Start()
    {
        defaultPostion = crosshair.transform.position;
    }

    private void Update()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, Camera.main.transform.forward * hit.distance, Color.red);
            crosshair.transform.position = cam.WorldToScreenPoint(hit.point);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, Camera.main.transform.forward * 1000, Color.white);
            crosshair.transform.position = defaultPostion;
            Debug.Log("Did not Hit");
        }
    }
}
