using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class swishaudio : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

    }


// Update is called once per frame
void Update()
    {
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource in the player NULL!");
        }
        if (Input.GetKeyDown("e"))
        {
            _audioSource.Play();
        }
    }
}
