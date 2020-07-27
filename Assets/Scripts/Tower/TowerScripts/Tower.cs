using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] string TowerFolderPath;
    int numFloors = 10;
    int currentFloor;
    int previousFloor;
    GameObject floorObject;
    TowerMember[] tower;
    Dictionary<string, Floor> floorDictionary;

    //For Testin
    Floor empty;
    Floor treasure;
    Floor emptyTreasure;
    Floor enemy1;
    Floor enemy2;
    Floor boss1;
    Floor wiseman;
    Floor puzzle;

    FloorSet bossSet;
    FloorSet puzzleSet;
    FloorSet enemy1Set;
    FloorSet enemy2Set;
    FloorSet treasureSet;
    FloorSet treasureSet2;
    FloorSet emptySet;

    // Start is called before the first frame update
    //TODO
    //Load in All floors, sort by terminal and nonterminal
    //Load in Sets
    //Load Tower randomly

    void Start()
    {
        currentFloor = 0;
        previousFloor = -1;
        //Floor instantiation and population
        floorDictionary = new Dictionary<string, Floor>();
        /*
        empty = new Floor();
        treasure = new Floor();
        emptyTreasure = new Floor();
        enemy1 = new Floor();
        enemy2 = new Floor();
        boss1 = new Floor();
        wiseman = new Floor();
        puzzle = new Floor();

        empty.id = "Empty";
        empty.terminalRoom = true;
        empty = new EmptyFloor(empty);
        floorDictionary.Add(empty.id, empty);

        treasure.id = "Treasure";
        treasure.terminalRoom = false;
        treasure.nextRoom = emptyTreasure;
        floorDictionary.Add(treasure.id, treasure);

        emptyTreasure.id = "Empty Treasure";
        emptyTreasure.terminalRoom = true;
        floorDictionary.Add(emptyTreasure.id, emptyTreasure);

        enemy1.id = "Enemy 1";
        enemy1.terminalRoom = false;
        enemy1.nextRoom = empty;
        floorDictionary.Add(enemy1.id, enemy1);

        enemy2.id = "Enemy 2";
        enemy2.terminalRoom = false;
        enemy2.nextRoom = treasure;
        floorDictionary.Add(enemy2.id, enemy2);


        boss1.id = "Boss 1";
        boss1.terminalRoom = false;
        boss1.nextRoom = treasure;
        floorDictionary.Add(boss1.id, boss1);

        wiseman.id = "Wiseman";
        wiseman.terminalRoom = true;
        floorDictionary.Add(wiseman.id, wiseman);

        puzzle.id = "Puzzle";
        puzzle.terminalRoom = false;
        puzzle.nextRoom = treasure;
        floorDictionary.Add(puzzle.id, puzzle);



        //Set Instantiation and Population
        bossSet = new FloorSet();
        puzzleSet = new FloorSet();
        enemy1Set = new FloorSet();
        enemy2Set = new FloorSet();
        treasureSet = new FloorSet();
        treasureSet2 = new FloorSet();
        emptySet = new FloorSet();

        bossSet.name = "Boss 1 Set";
        bossSet.floorIDs = new string[] { "Boss 1", "Empty"};
        bossSet.FindFloorsInSet();

        puzzleSet.name = "Puzzle Set";
        puzzleSet.floorIDs = new string[] { "Puzzle", "Wiseman" };
        puzzleSet.FindFloorsInSet();

        enemy1Set.name = "Enemy 1 Set";
        enemy1Set.floorIDs = new string[] { "Enemy 1" };
        enemy1Set.FindFloorsInSet();
        
        enemy2Set.name = "Enemy 2 Set";
        enemy2Set.floorIDs = new string[] { "Enemy 2" };
        enemy2Set.FindFloorsInSet();

        treasureSet.name = "Treasure Set";
        treasureSet.floorIDs = new string[] { "Treasure" };
        treasureSet.FindFloorsInSet();

        treasureSet2.name = "Treasure Set2";
        treasureSet2.floorIDs = new string[] { "Treasure" };
        treasureSet2.FindFloorsInSet();


        emptySet.name = "Empty Set";
        emptySet.floorIDs = new string[] { "Empty" };
        emptySet.FindFloorsInSet();

        

        //Build Tower
        tower[8] = new TowerMember(bossSet, 0);
        tower[7] = new TowerMember(bossSet, 1);
        tower[6] = new TowerMember(puzzleSet, 0);
        tower[5] = new TowerMember(puzzleSet, 1);
        tower[4] = new TowerMember(treasureSet2, 0);
        tower[3] = new TowerMember(treasureSet, 0);
        tower[2] = new TowerMember(enemy1Set, 0);
        tower[1] = new TowerMember(enemy2Set, 0);
        tower[0] = new TowerMember(emptySet, 0);
        */



        tower = new TowerMember[numFloors];


        //Read in Floors from Folder
        string[] allFloorFolders = Directory.GetDirectories(TowerFolderPath + "/Floors/");
        for (int j = 0; j < allFloorFolders.Length; j++)
        {
            //Read in Floors from Subfolder
            string[] floorStrings = Directory.GetFiles(allFloorFolders[j], "*.json");
            for (int i = 0; i < floorStrings.Length; i++)
            {
                Debug.Log(floorStrings[i]);
                using (StreamReader sr = new StreamReader(floorStrings[i]))
                {

                    Floor temp = JsonUtility.FromJson<Floor>(sr.ReadToEnd());
                    switch (temp.floorType)
                    {
                        case "FightFloor":
                            temp = new FightFloor(temp);
                            break;
                        case "EmptyFloor":
                            temp = new EmptyFloor(temp);
                            break;
                        case "TreasureFloor":
                            temp = new TreasureFloor(temp);
                            break;
                        case "LobbyFloor":
                            temp = new LobbyFloor(temp);
                            break;
                        case "BossFloor":
                            temp = new BossFloor(temp);
                            break;
                        case "PuzzleFloor":
                            temp = new PuzzleFloor(temp);
                            break;
                        case "RestFloor":
                            temp = new RestFloor(temp);
                            break;
                        case "WisemanFloor":
                            temp = new WisemanFloor(temp);
                            break;
                    }
                    floorDictionary.Add(temp.id, temp);
                }
            }
        }

        //CreateCollectionsOfAllSets
        //OrderByWorld
        List<Queue<FloorSet>> allSets = new List<Queue<FloorSet>>();

        //Read in Sets from Folder and Construct Tower
        int previousCap = -1;
        string[] allSetFolders = Directory.GetDirectories(TowerFolderPath + "/Sets/");
        //Sort Folders by World
        for (int i = 0; i < allSetFolders.Length; i++)
        {
            for (int j = 0; j < allSetFolders.Length; j++)
            {
                if (allSetFolders[i].CompareTo(allSetFolders[j]) > 0)
                {
                    string temp = allSetFolders[i];
                    allSetFolders[i] = allSetFolders[j];
                    allSetFolders[j] = temp;
                }
            }
        }

        for (int j = 0; j < allSetFolders.Length; j++)
        {
            allSets.Add(new Queue<FloorSet>());
            //Read in Floors from Subfolder
            string[] setStrings = Directory.GetFiles(allSetFolders[j], "*.json");
            for (int i = 0; i < setStrings.Length; i++)
            {
                using (StreamReader sr = new StreamReader(setStrings[i]))
                {
                    FloorSet temp = JsonUtility.FromJson<FloorSet>(sr.ReadToEnd());
                    temp.FindFloorsInSet();
                    allSets[j].Enqueue(temp);
                }
            }


            //Construct World in Tower
            //Current Implementation does not consider set floors for Boss/Lobby or Game-Wide Sets
            //
            //Worlds
            //0 - Floors 0-9
            //1 - Floors 10-29
            //2 - Floors 30-59
            //3 - Floors 60-99
            int remainingFloors = (j + 1) * 10;
            //Construct Hat to Draw Floors from
            List<int> prehat = new List<int>();
            Queue<int> hat = new Queue<int>();
            for (int i = previousCap + 1; i <= previousCap + (j + 1) * 10; i++)
            {
                prehat.Add(i);
            }
            while (prehat.Count > 0)
            {
                int roll = Random.Range(0, prehat.Count);
                hat.Enqueue(prehat[roll]);
                prehat.RemoveAt(roll);
            }

            while (allSets[j].Count > 0 && remainingFloors > 0)
            {
                FloorSet tryAdd = allSets[j].Dequeue();
                if (remainingFloors >= tryAdd.floors.Count)
                {
                    // 33% chance of set being added to tower, else it goes in the back of the line.  Ensures variety through different runs
                    int roll = (int)(Random.value * 3) % 3;
                    if (roll == 2 || remainingFloors == tryAdd.floors.Count)
                    {
                        int setIndex = 0;
                        while (setIndex < tryAdd.floors.Count)
                        {
                            int myFloor = hat.Dequeue();
                            tower[myFloor] = new TowerMember(tryAdd, setIndex);
                            setIndex++;
                            remainingFloors--;
                            Debug.Log("Floor added at : " + myFloor);
                        }
                    }
                    else
                    {
                        allSets[j].Enqueue(tryAdd);
                    }
                }
            }
            previousCap += (j + 1) * 10;
        }
        try
        {
            floorObject = GameObject.Find(tower[currentFloor].set.floors[tower[currentFloor].setIndex].nameOfVisualParentGameObject);
            floorObject.SetActive(true);
        }
        catch { }

        PrintTower();
    }

    // Update is called once per frame
    void Update()
    {
        //Handling Floor Changes

        previousFloor = currentFloor;
        

        //Handling Input, which is different for each type of floor
        tower[currentFloor].set.floors[tower[currentFloor].setIndex].ReceiveInput();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PrintTower();
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            CompleteFloor();
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            SwapFloor();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            currentFloor = ++currentFloor % numFloors;
            Debug.Log(currentFloor);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            currentFloor = --currentFloor % numFloors;
            Debug.Log(currentFloor);
        }
        if (currentFloor != previousFloor)
        {
            tower[currentFloor].set.floors[tower[currentFloor].setIndex].Start();
            Debug.Log("Floor Changed");
            //try
            //{
            //    floorObject.SetActive(false);
            //    floorObject = GameObject.Find(tower[currentFloor].set.floors[tower[currentFloor].setIndex].nameOfVisualParentGameObject);
            //    floorObject.SetActive(true);

            //}
            //catch { }
        }

    }
    //swap floors within set
    public Floor GetFloor(string key)
    {
        Floor floor;
        floorDictionary.TryGetValue(key, out floor);
        return floor;
    }

    public void PrintTower()
    {
        string towerText = "";
        for (int i = numFloors - 1; i >= 0; i--)
        {
            try
            {
                towerText = towerText + "Floor     " + i + "     Set:     " + tower[i].set.name + "     Floor     " + tower[i].set.floors[tower[i].setIndex].id + "\n";
            }
            catch
            {
                towerText = towerText + "Dead Floor\n";
            }
        }
        Debug.Log(towerText);
    }

    public void CompleteFloor()
    {
        tower[currentFloor].CompleteRoom();
    }
    public void SwapFloor()
    {
        tower[currentFloor].set.SwapFloors(tower[currentFloor].setIndex);
    }

}
