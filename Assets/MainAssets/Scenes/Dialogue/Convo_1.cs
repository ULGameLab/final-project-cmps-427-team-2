using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Line
{
    public Character character;
    public string text;

}
[CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation")]
public class Convo_1 : ScriptableObject
{
    public Character speakerLeft;
    public Character speakerRight;
    public Line[] lines;
}
