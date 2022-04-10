using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Speaker_S : MonoBehaviour
{
    //public Image portrait;
    public TextMeshProUGUI fullName;
    public TextMeshProUGUI dialog_S;

    private Character speaker;
    public Character Speaker
    {
        get { return speaker; }
        set
        {
            speaker = value;
            //potrait.sprite = speaker.potrait;
            fullName.text = speaker.fullName;
        }
    }
    public string Dialogue
    {
        get { return dialog_S.text; }
        set { dialog_S.text = value; }
    }
    public bool HasSpeaker()
    {
        return speaker != null;
    }
    public bool SpeakerIs(Character character)
    {
        return speaker == character;
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
