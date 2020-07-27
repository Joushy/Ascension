using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{

    // Name of the npc speaking
    public string npcName;

    // Boolean to see if they are currently speaking
    public bool speaking = false;

    // check
    public int inputBuffer = 0;

    [TextArea(3,10)]
    public string[] sentences;

}
