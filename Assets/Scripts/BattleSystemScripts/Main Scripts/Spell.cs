using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell
{
    public string Name;
    public string Type;
    public string TargetType;
    public string Combo;
    public float[] Arg1;
    public float[] Arg2;
    public string Description;
    public string[] StatEffected;
    public string InGameCost;
    public string CastAnim;
    public string ReceiveAnim;

    public string ParticleEffect;

    public Spell() { }

    override
    public string ToString()
    {
        string testArray = "";
        for(int i = 0; i < StatEffected.Length; i++)
        {
            testArray = testArray + StatEffected[i] + Arg1[i] + Arg2[i] + CastAnim + "\n";
        }
        return testArray;
    }
}
