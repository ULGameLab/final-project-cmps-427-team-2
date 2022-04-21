using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.SceneManagement;


public class Dialogue_Display : MonoBehaviour
{
    public Convo_1 conversation;
    public Convo_1 defaultConversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private Speaker_S speakerUILeft;
    private Speaker_S speakerUIRight;

    public int activeLines = 0;
    public bool conversationStarted = false;

    void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<Speaker_S>();
        speakerUIRight = speakerRight.GetComponent<Speaker_S>();

        
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            DisplayLine();
        }else if (Input.GetKeyDown("x"))
        {
            EndConversation();
        }
    }
    void EndConversation()
    {
        conversation = defaultConversation;
        conversationStarted = false;
        speakerUILeft.Hide();
        speakerUIRight.Hide();
    }
    public void Initialize()
    {
        conversationStarted = true;
        activeLines = 0;
        

        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;
        DisplayLine();

    }
    void AdvanceConversation()
    {
        if (activeLines < conversation.lines.Length)
        {
            DisplayLine();
            activeLines += 1;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            activeLines = 0;
        }
    }
    void DisplayLine()
    {
        Line line = conversation.lines[activeLines];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialogue(speakerUILeft, speakerUIRight, line);
            activeLines += 1;
        }
        else if(speakerUIRight.SpeakerIs(character))
        {
            SetDialogue(speakerUIRight, speakerUILeft, line);
            activeLines += 1;
        }
        

    }
    void SetDialogue( Speaker_S activeSpeakerUI, Speaker_S inactiveSpeakerUI, Line line)
    {
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();

        activeSpeakerUI.Dialogue = " ";
        activeSpeakerUI.Show();

        StopAllCoroutines();
        StartCoroutine(EffectTypeWriter(line.text, activeSpeakerUI));
    }

    private IEnumerator EffectTypeWriter(string text, Speaker_S controller)
    {
        foreach(char character in text.ToCharArray())
        {
            controller.Dialogue += character;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
    

