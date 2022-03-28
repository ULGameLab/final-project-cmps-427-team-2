using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSpells : MonoBehaviour
{
    public GameObject[] spells;



    public void Activate(int spellNum)
    {
        for(int i = 0; i < spells.Length; i++)
        {
            spells[i].SetActive(false);
        }
        spells[spellNum].SetActive(true);
    }

    public void DeactivateSpell(int spellNum)
    {
        spells[spellNum].SetActive(false);
    }

    
}
