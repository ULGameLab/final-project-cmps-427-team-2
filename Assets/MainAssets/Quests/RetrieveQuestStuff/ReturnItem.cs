using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnItem : MonoBehaviour
{
    public InteractableQuestWeapon weapon;
    public QuestManager questManager;
    public int questNum;
    public int spellUnlockNum;
    private void OnTriggerEnter(Collider other)
    {
        questManager.QuestCheck(questNum, spellUnlockNum);
        weapon.gameObject.SetActive(false);
    }
}
