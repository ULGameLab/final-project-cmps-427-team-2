using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTornado : MonoBehaviour
{
    Transform moveTo;
    // Update is called once per frame

    private void Start()
    {
        moveTo = GameObject.Find("tornadoMoveTo").transform;
    }

    void Update()
    {
      
        transform.position = Vector3.MoveTowards(transform.position, moveTo.forward * 34, 6f * Time.deltaTime);
    }
}
