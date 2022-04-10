using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public NPC1 np1;
    public GameObject gb;

    public void TriggerDialogue()
    {
        //FindObjectOfType<NPC1>().OnTriggerEnter(gb.gameObject);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        //FindObjectOfType<NPC1>().OnTriggerEnter(np1);
    }
}
