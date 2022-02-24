using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Variables;

public class MagicCombo1 : MonoBehaviour
{

    public float elapse = 5f;
    private float currentElapsedTime;


    private bool b1 = false;
    private bool b2 = false;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        currentElapsedTime = elapse;
    }

    // Update is called once per frame
    void Update()
    {

        currentElapsedTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F) && currentElapsedTime != 0)
        {
            currentElapsedTime = elapse;
            b1 = true;
            Debug.Log("Hello");
            
        }
        if (Input.GetKeyDown(KeyCode.C) && currentElapsedTime != 0 && b1)
        {
            Debug.Log("combo");
            currentElapsedTime = elapse;
            b2 = true;
        }

        if (b1 && b2)
        {

            
            b1 = false;
            b2 = false;
            currentElapsedTime = elapse;
        }
    }
}
