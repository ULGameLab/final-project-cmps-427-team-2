using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableQuestWeapon : QuestManager
{
    public Vector3 locationOfObjectPickedUp = new Vector3(-0.0260000005f, 0.843741179f, -0.356999993f);
    public Vector3 rotationOfObjectPickedUp = new Vector3(35.7758713f, 96.6970825f, 359.417511f);
    private GameObject parentWeapon;
    private Rigidbody rb;

    [HideInInspector]
    public bool isPickedUp;

    private void Start()
    {
        parentWeapon = GameObject.FindGameObjectWithTag("PlayerPelvis");
        BookHandler = GameObject.FindGameObjectWithTag("BookHandler").GetComponent<BookBehavior>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.transform.parent = parentWeapon.transform;
            gameObject.transform.localPosition = locationOfObjectPickedUp;
            gameObject.transform.localEulerAngles = rotationOfObjectPickedUp;
            BookHandler.UpdateQuestText("Deliver the bow back to Dustin.", Quest3Title);
            rb.isKinematic = true;
            
            isPickedUp = true;
        }
    }
}
