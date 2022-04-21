using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Variables;

public class NPC : MonoBehaviour
{
    private Collider NPCTrigger;
    private string numNPC;
    private QuestManager questManager;
    private Dialogue_Display dDisplay;

    private void Start()
    {
        NPCTrigger = GetComponent<CapsuleCollider>();
        string name = gameObject.name;
        numNPC = name[^1].ToString();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        dDisplay = GameObject.Find("Dial_S").GetComponent<Dialogue_Display>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            questManager.StartQuest(numNPC);
            dDisplay.Initialize(numNPC);
            NPCTrigger.enabled = false;
            VariablesManager.SetLocal(dDisplay.player, "StopMovement", true, true);
        }
    }
    public void OnTriggerStay(Collider other)
    {
    }
    public void OnTriggerExit(Collider other)
    {
        //QuestManager.startQuest1();
    }
}
