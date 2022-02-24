using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstationEffects : MonoBehaviour
{


    public GameObject volcanicSpikeAttack;
    public GameObject novaAttack;
    public GameObject tornadoAttack;




    public void InstantiateVolcanicSpike(Vector3 spikeLocation)
    {
        GameObject instatiatedSpikes = Instantiate(volcanicSpikeAttack, spikeLocation, Quaternion.Euler(-90, 0, 0));
        
        
        Destroy(instatiatedSpikes, 3);
    }


}
