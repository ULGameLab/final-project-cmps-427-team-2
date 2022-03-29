using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSpells : MonoBehaviour
{
    public GameObject[] quickSpells;
    public GameObject[] comboSpells;



    public void ActivateQuickSpells(int spellNum)
    {
        for(int i = 0; i < quickSpells.Length; i++)
        {
            quickSpells[i].SetActive(false);
        }

        if(spellNum < quickSpells.Length)
        {
            quickSpells[spellNum].SetActive(true);
        }
    }

    public void ActivateComboSpells(int spellNum)
    {
        if(spellNum < comboSpells.Length)
        {
            comboSpells[spellNum].SetActive(true);
        }
    }

    
}
