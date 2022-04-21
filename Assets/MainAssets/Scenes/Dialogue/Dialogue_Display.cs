using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.SceneManagement;


public class Dialogue_Display : MonoBehaviour
{
    private Convo_1 defaultConversation;
    public Convo_1[] conversations;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private Speaker_S speakerUILeft;
    private Speaker_S speakerUIRight;



    private int[] activeLines;
    private int convoNum ;

    void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<Speaker_S>();
        speakerUIRight = speakerRight.GetComponent<Speaker_S>();
        activeLines = new int[conversations.Length];

        
    }
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            AdvanceConversation() ;
        }else if (Input.GetKeyDown("x"))
        {
            EndConversation();
        }
    }
    void EndConversation()
    {
        conversations[convoNum] = defaultConversation;
        speakerUILeft.Hide();
        speakerUIRight.Hide();
    }
    public void Initialize(string convoNum)
    {
                this.convoNum = int.Parse(convoNum) - 1;
                activeLines[this.convoNum] = 0;
                speakerUILeft.Speaker = conversations[this.convoNum].speakerLeft;
                speakerUIRight.Speaker = conversations[this.convoNum].speakerRight;

                AdvanceConversation();
            
        
      
    }



    void AdvanceConversation()
    {

               
                    if (activeLines[convoNum] < conversations[convoNum].lines.Length) 
                    {
                        DisplayLine();
                        activeLines[convoNum] += 1;
                    }
                    else
                    {
                        speakerUILeft.Hide();
                        speakerUIRight.Hide();
                        activeLines[convoNum] = 0;
                    }
                
            
   
    }


    void DisplayLine()
    {
            
                
                    Line line = conversations[convoNum].lines[activeLines[convoNum]];
                    Character character = line.character;

                    if (speakerUILeft.SpeakerIs(character))
                    {
                        SetDialogue(speakerUILeft, speakerUIRight, line);
                    }
                    else if (speakerUIRight.SpeakerIs(character))
                    {
                        SetDialogue(speakerUIRight, speakerUILeft, line);
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
    
    

