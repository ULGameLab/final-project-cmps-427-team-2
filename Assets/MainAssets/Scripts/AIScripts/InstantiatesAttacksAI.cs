using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatesAttacksAI : MonoBehaviour
{

    
    public Transform volcanicSpikeLocation;
    public Transform novaLoction;
    public Transform tornadoLocation;
    public Transform moveToTornado;

    public GameObject volcanicSpikeAttack;
    public GameObject novaAttack;
    public GameObject tornadoAttack;

    private void Start()
    {
        
    }

    
    public void InstantiateVolcanicSpike()
    {
        GameObject instatiatedSpikes = Instantiate(volcanicSpikeAttack, volcanicSpikeLocation.position, Quaternion.Euler(-90, 0, 0));

        Destroy(instatiatedSpikes, 3);
    }

    public void InstantiateNovaAttack()
    {
        GameObject instatiatedObject = Instantiate(novaAttack, novaLoction.position, Quaternion.Euler(-90, 0, 0));

        Destroy(instatiatedObject, 3);
    }

    public void InstantiateTornadoAttack()
    {
        GameObject instatiatedObject = Instantiate(tornadoAttack, tornadoLocation.position, Quaternion.Euler(0, 0, 0));
        Destroy(instatiatedObject, 6);
    }

  
}
