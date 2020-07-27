using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;

public class OverworldGameManager : MonoBehaviour
{

    public string savePath;
    public List<Fighter> fighters;
    public Save newSave;
    GameObject heroObject;

    // Start is called before the first frame update
    void Start()
    {
        heroObject = GameObject.FindGameObjectWithTag("TownPlayer");

        fighters = new List<Fighter>();
        using (StreamReader sr = new StreamReader(savePath))
        {
            for (int i = 0; i < 4; i++)
            {
                string fighterString = sr.ReadLine();
                fighters.Add(JsonUtility.FromJson<Fighter>(fighterString));
            }
            string remainderOfSave = sr.ReadToEnd();
            newSave = JsonUtility.FromJson<Save>(remainderOfSave);
        }
        if (newSave.nextArea.Equals(newSave.currentArea))
        {
            //look for defeated enemies and get rid of them
            foreach(string deadEnemy in newSave.doNotLoadTheseEnemies)
            {
                GameObject.Find(deadEnemy).SetActive(false);
            }
        } else
        {
            newSave.doNotLoadTheseEnemies = new string[] { };
        }
        newSave.currentArea = newSave.nextArea;
        SetHeroPosition(newSave.nextAreaXYZ[0], newSave.nextAreaXYZ[1], newSave.nextAreaXYZ[2]);
    }

    public bool RestoreHealth()
    {
        for(int i = 0; i < fighters.Count; i++)
        {
            fighters[i].HP = fighters[i].maxHP;
        }
        return true;
    }
    public void ChooseNextArea(string nextArea, float nextX, float nextY, float nextZ)
    {
        heroObject = GameObject.FindGameObjectWithTag("TownPlayer");
        newSave.currentAreaXYZ = new float[] { heroObject.transform.position.x, heroObject.transform.position.y, heroObject.transform.position.z };
        newSave.nextArea = nextArea;
        newSave.nextAreaXYZ = new float[] { nextX, nextY, nextZ };
    }
    public void TransitionToNextArea()
    {
        using (StreamWriter sw = new StreamWriter(savePath))
        {
            sw.WriteLine(JsonUtility.ToJson(fighters[0]));
            sw.WriteLine(JsonUtility.ToJson(fighters[1]));
            sw.WriteLine(JsonUtility.ToJson(fighters[2]));
            sw.WriteLine(JsonUtility.ToJson(fighters[3]));
            sw.WriteLine(JsonUtility.ToJson(newSave));
        }
        SceneManager.LoadScene(newSave.nextArea);
    }
    public void SetHeroPosition(float x, float y, float z)
    {
        newSave.currentAreaXYZ = new float[] { x, y, z };
        heroObject.transform.position = new Vector3(x, y, z);
    }

    void GetRelatedEnemies(string enemyName)
    {
        List<string> ChosenEnemies = new List<string>();
        List<string> Hat = new List<string>();
        List<EnemyLotteryTicket> ticketsClassList = new List<EnemyLotteryTicket>();
        int maxEnemies;
        //add initial enemy to list
        ChosenEnemies.Add(enemyName);

        //read in spawn chances from JSON
        //TODO get correct path
        using (StreamReader sr = new StreamReader("Assets/Scripts/BattleSystemScripts/AreaRelatedEnemies/" +newSave.currentArea + "/" + enemyName + "/Lottery.JSON"))
        {
            //Get Max Enemies at start of file
            maxEnemies = int.Parse(sr.ReadLine());
            //Get Ticket Numbers
            while (sr.Peek() > -1)
            {
                ticketsClassList.Add(JsonUtility.FromJson<EnemyLotteryTicket>(sr.ReadLine()));
            }
        }
        //add name to hat, as many times as spawn chances
        for (int i = 0; i < ticketsClassList.Count; i++)
        {
            for (int k = 0; k < ticketsClassList[i].ticketNumbers; k++)
            {
                Hat.Add(ticketsClassList[i].EnemyName);
            }
        }
        //draw from hat
        for (int i = 0; i < maxEnemies - 1; i++)
        {
            int roll = (int)((Random.value * Hat.Count) % Hat.Count);
            ChosenEnemies.Add(Hat[roll]);
            Hat.RemoveAt(roll);
        }
        newSave.EnemyNames = ChosenEnemies.ToArray();

        newSave.numEnemies = ChosenEnemies.Count;
    }
    public void Encounter(string area, string enemyName)
    {
        string[] tempList = new string[newSave.doNotLoadTheseEnemies.Length + 1];
        for (int i = 0; i < tempList.Length-1; i++) { tempList[i] = newSave.doNotLoadTheseEnemies[i]; }
        tempList[tempList.Length - 1] = enemyName;
        newSave.doNotLoadTheseEnemies = tempList;
        GetRelatedEnemies(Regex.Replace(enemyName, @"[\d-]", string.Empty));
        ChooseNextArea(area, -1, -1, -1);
        // TransitionToNextArea();
    }
    public void ReachedCheckpoint()
    {
        newSave.lastCheckpoint = newSave.currentArea;
        newSave.lastCheckpointXYZ = new float[] { heroObject.transform.position.x, heroObject.transform.position.y, heroObject.transform.position.z };
    }
}
