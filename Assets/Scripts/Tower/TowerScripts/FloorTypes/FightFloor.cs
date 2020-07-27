using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FightFloor : Floor
{
    GameManager gameManager;
    Tower tower;
    public FightFloor(Floor f)
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
        tower = GameObject.FindGameObjectWithTag("Tower").GetComponent<Tower>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //Create references to GameObjects here
        gameManager.EncounterBegin(GetEnemies(enemyName));
    }

    //Called in Update.  Any input should be handled here
    public override void ReceiveInput()
    {
        //All Input for Battles is Handled in GameManager
    }

    IEnumerator CheckForBattleEnd()
    {
        //check for End of Battle
        yield return new WaitForSeconds(2f);
        while (!gameManager.CheckEndCase())
        {
            yield return new WaitForSeconds(0.5f);
        }
        //transition to next room
        tower.CompleteFloor();
    }



    //Choosing Related Enemies
    public string[] GetEnemies(string enemy)
    {
        List<string> ChosenEnemies = new List<string>();
        List<string> Hat = new List<string>();
        List<EnemyLotteryTicket> ticketsClassList = new List<EnemyLotteryTicket>();
        int maxEnemies;
        //add initial enemy to list
        ChosenEnemies.Add(enemy);

        //read in spawn chances from JSON
        //TODO get correct path
        using (StreamReader sr = new StreamReader("Assets/Scripts/BattleSystemScripts/Tower/" + enemyName + "/Lottery.JSON"))
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
        return ChosenEnemies.ToArray();
    }
}
