using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyFloor : Floor
{
    GameObject chest;
    public EmptyFloor(Floor f)
    {
        this.floorType = f.floorType;
        this.id = f.id;
        this.lockedKey = f.lockedKey;
        this.key = f.key;
        this.enemyName = f.enemyName;
        this.nextRoom = f.nextRoom;
        this.reward = f.reward;
        this.rewardAmount = f.rewardAmount;
        this.text = f.text;
        this.terminalRoom = f.terminalRoom;
        this.difficultyFlag = f.difficultyFlag;
        this.nameOfVisualParentGameObject = f.nameOfVisualParentGameObject;
    }

    //Called Whenever Switching Floors
    public override void Start()
    {
        //Create references to GameObjects here
        chest = GameObject.Find("Chest");
    }

    //Called in Update.  Any input should be handled here
    public override void ReceiveInput()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("This is an Empty Room");
        }
    }
}
