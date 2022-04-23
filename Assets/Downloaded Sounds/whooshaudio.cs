using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class whooshaudio : MonoBehaviour
{
    private AudioSource _audioSource1;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource1 = GetComponent<AudioSource>();

    }


// Update is called once per frame
void Update()
    {
        if (_audioSource1 == null)
        {
            Debug.LogError("The AudioSource in the player NULL!");
        }
        if (Input.GetMouseButtonDown(0))
        {
            _audioSource1.Play();
        }
    }
}
