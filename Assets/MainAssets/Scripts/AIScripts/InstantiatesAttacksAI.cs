using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatesAttacksAI : MonoBehaviour
{
    public Transform characterTranform;

    //FireDemon attack Locations
    public Transform volcanicSpikeLocation;
    public Transform novaLoction;
    public Transform tornadoLocation;
    public Transform moveToTornado;

    //Fire Demon AttackObjects
    public GameObject volcanicSpikeAttack;
    public GameObject novaAttack;
    public GameObject tornadoAttack;

    //GoblinArcher attack Location
    public Transform arrowSpawnLocation;

    //goblin attack objects
    public GameObject arrowShot;

    //CuteDevil AttackObjects
    public GameObject fireBallAttack;
    public GameObject radioActiveBallAttack;

    //CuteDevilAttackLocations
    public Transform fireBallPosition;
    public Transform radioActiveBallPosition;



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
        instatiatedObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
        Destroy(instatiatedObject, 6);
    }

    public void InstantiateArrowWithForce()
    {
        GameObject instatiatedObject = Instantiate(arrowShot, arrowSpawnLocation.position, Quaternion.Euler(90, characterTranform.transform.eulerAngles.y, Quaternion.identity.z));
        instatiatedObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
        Destroy(instatiatedObject, 6);
    }

    public void InstantiateFireBall()
    {
        GameObject instatiatedObject = Instantiate(fireBallAttack, fireBallPosition.position, Quaternion.Euler(0, 0, 0));
        instatiatedObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
    }

    public void InstantiateRadioActiveBall()
    {
        GameObject instatiatedObject = Instantiate(fireBallAttack, fireBallPosition.position, Quaternion.Euler(0, 0, 0));
        instatiatedObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50f, ForceMode.Impulse);
        Destroy(instatiatedObject, 3);
    }

}
