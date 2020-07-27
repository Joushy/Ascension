using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloorSet
{
    public int priority;
    public string name;
    public int world;
    int numFloors;
    public string[] floorIDs;
    public List<Floor> floors;
    Tower tm;

    public FloorSet() { }

    public void FindFloorsInSet()
    {
        floors = new List<Floor>();
        tm = GameObject.FindGameObjectWithTag("Tower").GetComponent<Tower>();
        for(int i = 0; i < floorIDs.Length; i++)
        {
            floors.Add(tm.GetFloor(floorIDs[i]));
        }
        numFloors = floors.Count;
    }

    public void SwapFloors(int current)
    {
        if (numFloors > 1)
        {
            int toSwap;
            do
            {
                toSwap = (int)((Random.value * numFloors) % numFloors);
            } while (toSwap == current);
            string temp = floors[current].id;
            Debug.Log(temp);
            floors[current] = floors[toSwap];
            floors[toSwap] = tm.GetFloor(temp);
        }
    }
    public void SwapFloors(int current, int toSwap)
    {
        if (numFloors > 1 && toSwap < numFloors)
        {
            string temp = floors[current].id;
            floors[current] = floors[toSwap];
            floors[toSwap] = tm.GetFloor(temp);
        }
    }
    public void CompleteFloor(int i)
    {
        if (!floors[i].terminalRoom)
        {
            floors[i] = tm.GetFloor(floors[i].nextRoom);
        }
        UnlockRelatedFloor(i);
    }
    public void UnlockRelatedFloor(int j)
    {
        string key = floors[j].key;
        for (int i = 0; i < floors.Count; i++)
        {
            if (floors[i].UnlockCheck(key))
            {
                if (!floors[i].terminalRoom)
                {
                    floors[i] = tm.GetFloor(floors[i].nextRoom);
                }
            }
        }
    }
}
