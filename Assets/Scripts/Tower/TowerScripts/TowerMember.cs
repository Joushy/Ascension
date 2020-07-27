using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMember
{
    //TODO add room functionality
    public FloorSet set;
    public int setIndex;
    public TowerMember(FloorSet s, int i) {
        set = s;
        setIndex = i;
    }
    public bool IsCompleted()
    {
        return set.floors[setIndex].terminalRoom;
    }
    public void CompleteRoom()
    {
        set.CompleteFloor(setIndex);
    }
    public void UnlockRelatedFloor()
    {
        set.UnlockRelatedFloor(setIndex);
    }
}
