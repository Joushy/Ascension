using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor
{
    public string floorType;
    public string id;
    public string lockedKey;
    public string key;
    public string nameOfVisualParentGameObject;
    public string enemyName;
    public string nextRoom;
    public string reward;
    public int rewardAmount;
    public string text;
    public bool terminalRoom;
    public int difficultyFlag;
    public Floor() { }
    public bool UnlockCheck(string key)
    {
        if (key.Equals(lockedKey)  && !key.Equals("")) { return true; }
        else { return false; }
    }
    public virtual void ReceiveInput() { }
    public virtual void Start() { }
}
