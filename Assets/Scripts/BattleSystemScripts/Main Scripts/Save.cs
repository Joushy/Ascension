using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int saveID;
    public int maxBoxes;
    public int potionCount;
    public int freezeHealCount;
    public int burnHealCount;
    public int poisonHealCount;
    public int numHeroesUnlocked;
    public int numEnemies;
    public string[] EnemyNames;
    public string currentArea;
    public float[] currentAreaXYZ;
    public string lastCheckpoint;
    public float[] lastCheckpointXYZ;
    public string nextArea;
    public float[] nextAreaXYZ;
    public float coin;
    public string[] doNotLoadTheseEnemies;
}
