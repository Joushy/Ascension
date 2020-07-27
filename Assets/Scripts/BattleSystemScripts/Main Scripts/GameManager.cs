using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //for spells
    Dictionary<string, Spell> Char0Spells;
    Dictionary<string, Spell> Char1Spells;
    Dictionary<string, Spell> Char2Spells;
    Dictionary<string, Spell> Char3Spells;
    List<string> Char0_activeSpells;
    List<string> Char1_activeSpells;
    List<string> Char2_activeSpells;
    List<string> Char3_activeSpells;
    public string char0_path;
    public string char1_path;
    public string char2_path;
    public string char3_path;

    //save info
    public string savePath;
    List<Fighter> safeFighters;
    public List<Fighter> tempFighters;
    public Save newSave;


    //for battles
    public bool battleGo;
    public string enemiesPath;
    float[] statusTimeStamps;
    public bool[] defending;
    public int numHeroes;
    public int selectedCharacter;
    public int enemyCursor;
    public int maxBoxes;
    public int numEnemy;
    public string[] inputStrings;
    public List<float> cooldowns;
    string[] char0_spells;
    string[] char1_spells;
    string[] char2_spells;
    string[] char3_spells;
    public bool[] enableInput;
    public int castSpeed;
    public int[] lastTarget;
    List<EnemyAI> enemyAIs;
    LevelChanger levelChanger;
    public bool isBossFight;

    //spawn locations
    List<Transform> allSpawnLocations; //heroes 0-3, enemies 4-7
    Transform spawnLocOne;
    Transform spawnLocTwo;
    Transform spawnLocThree;
    Transform spawnLocFour;
    Transform enemySpawnLocOne;
    Transform enemySpawnLocTwo;
    Transform enemySpawnLocThree;
    Transform enemySpawnLocFour;

    Object char1;
    Object char2;
    Object char3;
    Object char4;

    List<Object> enemyLoaders;
    List<Vector3> enemyPositions;

    public List<Animator> fighterAnimators;

    //change to correspond with controller inputs
    string[] inputs = { "A", "B", "X", "Y", "U", "D", "L", "R" }; // u - up    d - down

    // Controller shoulder buttons to move which character you are currently selected on
    string enemyUp = "R2";
    string enemyDown = "R1";
    string heroUp = "L2";
    string heroDown = "L1";

    private void Awake()
    {
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();

        allSpawnLocations = new List<Transform>();
        spawnLocOne = GameObject.Find("AllySpawn1").transform;
        allSpawnLocations.Add(spawnLocOne);
        spawnLocTwo = GameObject.Find("AllySpawn2").transform;
        allSpawnLocations.Add(spawnLocTwo);
        spawnLocThree = GameObject.Find("AllySpawn3").transform;
        allSpawnLocations.Add(spawnLocThree);
        spawnLocFour = GameObject.Find("AllySpawn4").transform;
        allSpawnLocations.Add(spawnLocFour);

        enemySpawnLocOne = GameObject.Find("EnemySpawnPoint1").transform;
        allSpawnLocations.Add(enemySpawnLocOne);
        enemySpawnLocTwo = GameObject.Find("EnemySpawnPoint2").transform;
        allSpawnLocations.Add(enemySpawnLocTwo);
        enemySpawnLocThree = GameObject.Find("EnemySpawnPoint3").transform;
        allSpawnLocations.Add(enemySpawnLocThree);
        enemySpawnLocFour = GameObject.Find("EnemySpawnPoint4").transform;
        allSpawnLocations.Add(enemySpawnLocFour);

        char1 = Instantiate(Resources.Load("Klaus", typeof(GameObject)), spawnLocOne.position, Quaternion.LookRotation(Vector3.left, Vector3.up));
        char2 = Instantiate(Resources.Load("Ferox", typeof(GameObject)), spawnLocTwo.position, Quaternion.LookRotation(Vector3.left, Vector3.up));
        char3 = Instantiate(Resources.Load("Meira", typeof(GameObject)), spawnLocThree.position, Quaternion.LookRotation(Vector3.left, Vector3.up));
        char4 = Instantiate(Resources.Load("Calid", typeof(GameObject)), spawnLocFour.position, Quaternion.LookRotation(Vector3.left, Vector3.up));

        enemyPositions = new List<Vector3>();
        enemyPositions.Add(enemySpawnLocOne.transform.position);
        enemyPositions.Add(enemySpawnLocTwo.transform.position);
        enemyPositions.Add(enemySpawnLocThree.transform.position);
        enemyPositions.Add(enemySpawnLocFour.transform.position);

        enemyLoaders = new List<Object>();

        fighterAnimators.Add(GameObject.Find(char1.name).GetComponent<Animator>());
        fighterAnimators.Add(GameObject.Find(char2.name).GetComponent<Animator>());
        fighterAnimators.Add(GameObject.Find(char3.name).GetComponent<Animator>());
        fighterAnimators.Add(GameObject.Find(char4.name).GetComponent<Animator>());

        LoadHeroes();
        ResetBattle();
    }

    // Start is called before the first frame update
    void Start()
    {
        //for spells
        Char0Spells = GetAllSpells(char0_path);
        Char1Spells = GetAllSpells(char1_path);
        Char2Spells = GetAllSpells(char2_path);
        Char3Spells = GetAllSpells(char3_path);

        //for battles

        castSpeed = 4;
    }

    public void EncounterBegin(string[] enemies)
    {
        ResetBattle();
        LoadEnemies(enemies);
        StartCoroutine(StartBattleCo());
    }

    IEnumerator StartBattleCo()
    {
        yield return new WaitForSeconds(2f);
        battleGo = true;
        // Debug.Log("started!");
    }

    // Update is called once per frame
    void Update()
    {

        //for battles
        if (battleGo)
        {
            Battle();
        }
    }


    //************************************************************************************************************************************
    //Before Battle

    //for spells

    //returns dictionary of all spells in folder path
    Dictionary<string, Spell> GetAllSpells(string path)
    {
        Dictionary<string, Spell> result = new Dictionary<string, Spell>();
        Spell sp;
        StreamReader reader;
        string spellString;
        string[] a = Directory.GetFiles(path);
        foreach (string spell in a)
        {
            if (!spell.Contains("meta"))
            {
                reader = new StreamReader(spell);
                spellString = reader.ReadToEnd();
                //Debug.Log("Pure String: " + spellString);
                sp = JsonUtility.FromJson<Spell>(spellString);
                //Debug.Log("Spell: " + sp.ToString());
                result.Add(sp.Combo, sp);
                reader.Close();
            }
        }
        return result;
    }

    //for battles
    void LoadEnemies(string[] enemies)
    {
        enemyAIs = new List<EnemyAI>();
        battleGo = false;
        numEnemy = enemies.Length;
        for (int i = 0; i < enemies.Length; i++)
        {
            tempFighters.Add(GetEnemy(enemies[i], i));

            if (isBossFight)
            {
                Object temp = Instantiate(Resources.Load("Enemies/" + enemies[i], typeof(GameObject)), enemyPositions[i], Quaternion.LookRotation(Vector3.forward, Vector3.up));
                enemyLoaders.Add(temp);
            }
            else
            {
                Object temp = Instantiate(Resources.Load("Enemies/" + enemies[i], typeof(GameObject)), enemyPositions[i], Quaternion.LookRotation(Vector3.right, Vector3.up));
                enemyLoaders.Add(temp);
            }


        }
    }
    void ResetBattle()
    {
        battleGo = false;
        Char0_activeSpells = tempFighters[0].unlockedSpells.OfType<string>().ToList();
        Char1_activeSpells = tempFighters[1].unlockedSpells.OfType<string>().ToList();
        Char2_activeSpells = tempFighters[2].unlockedSpells.OfType<string>().ToList();
        Char3_activeSpells = tempFighters[3].unlockedSpells.OfType<string>().ToList();
        // numberOfEnemies;
        enableInput = new bool[] { false, false, false, false };
        selectedCharacter = 0;
        enemyCursor = 4;
        inputStrings = new string[] { "", "", "", "" };

        for (int i = 0; i < 4; i++)
        {
            cooldowns[i] = Random.value * 3;
            lastTarget[i] = 4;
        }
        for (int i = 0; i < 8; i++)
        {
            
            defending[i] = false;
        }
        while (cooldowns.Count > 4)
        {
            cooldowns.RemoveAt(cooldowns.Count - 1);
        }
        while (tempFighters.Count > 4)
        {
            tempFighters.RemoveAt(tempFighters.Count - 1);
        }
    }
    void LoadHeroes()
    {
        battleGo = false;
        // --------- TODO ------
        lastTarget = new int[4];
        defending = new bool[8];
        // --------- TODO ------
        safeFighters = new List<Fighter>();
        tempFighters = new List<Fighter>();
        using (StreamReader sr = new StreamReader(savePath))
        {
            for (int i = 0; i < 4; i++)
            {
                string fighterString = sr.ReadLine();
                safeFighters.Add(JsonUtility.FromJson<Fighter>(fighterString));
                tempFighters.Add(safeFighters[i]);
                defending[i] = false;
            }
            string remainderOfSave = sr.ReadToEnd();
            newSave = JsonUtility.FromJson<Save>(remainderOfSave);
        }

        Char0_activeSpells = tempFighters[0].unlockedSpells.OfType<string>().ToList();
        Char1_activeSpells = tempFighters[1].unlockedSpells.OfType<string>().ToList();
        Char2_activeSpells = tempFighters[2].unlockedSpells.OfType<string>().ToList();
        Char3_activeSpells = tempFighters[3].unlockedSpells.OfType<string>().ToList();

        statusTimeStamps = new float[8];
        maxBoxes = 4;
        numHeroes = 4;
        enableInput = new bool[] { false, false, false, false };
        selectedCharacter = 0;
        enemyCursor = 4;
        cooldowns = new List<float>();
        inputStrings = new string[] { "", "", "", "" };

        for (int i = 0; i < 4; i++)
        {
            cooldowns.Add(Random.value * 3);
        }



    }
    Fighter GetEnemy(string enemyName, int i)
    {
        cooldowns.Add(Random.value * 3);
        enemyAIs.Add(new EnemyAI(enemyName, 4 + i, numEnemy, numHeroes));

        //Load in enemies 


        //Debug.Log(enemiesPath + "/" + enemyName + ".json");
        using (StreamReader sr = new StreamReader(enemiesPath + "/" + enemyName + "/" + enemyName + ".json"))
        {
            string fighterString = sr.ReadToEnd();
            return JsonUtility.FromJson<Fighter>(fighterString);
        }

    }


    //************************************************************************************************************************************
    //Battle Loop

    void Battle()
    {
        battleGo = CheckEndCase();
        
        //Status Effects
        for (int i = 0; i < 4 + numEnemy; i++)
        {
            switch (tempFighters[i].status)
            {
                case "freeze":
                    cooldowns[i] -= Time.deltaTime;
                    if (Time.time - statusTimeStamps[i] > 30)
                    {
                        tempFighters[i].status = "";
                    }
                    break;
                case "sleep":
                    cooldowns[i] -= Time.deltaTime;
                    if (Time.time - statusTimeStamps[i] > 120)
                    {
                        tempFighters[i].status = "";
                    }
                    break;
                case "sloth":
                    cooldowns[i] -= Time.deltaTime / 2;
                    if (Time.time - statusTimeStamps[i] > 45)
                    {
                        tempFighters[i].status = "";
                    }
                    break;
                case "poison":
                    if (Time.time - statusTimeStamps[i] > 2)
                    {
                        if (tempFighters[i].HP > 0)
                        {
                            tempFighters[i].UpdateHP(-10);
                            statusTimeStamps[i] = Time.time;
                        }
                    }
                    break;
                case "burn":
                    if (Time.time - statusTimeStamps[i] > 6)
                    {
                        if (tempFighters[i].HP > 0)
                        {
                            tempFighters[i].UpdateHP(-40);
                            statusTimeStamps[i] = Time.time;
                        }
                    }
                    break;
            }
        }


        battleGo = CheckEndCase();
        //update timeline and cast spells
        for (int i = 4; i < 4 + numEnemy; i++)
        {
            if (tempFighters[i].HP > 0)
            {
                cooldowns[i] += Time.deltaTime;
                if (cooldowns[i] >= tempFighters[i].speed + castSpeed)
                {
                    enemyAIs[i - 4].CheckCondition(tempFighters, cooldowns);
                    Spell sp = enemyAIs[i - 4].GetSpell();
                    int target = enemyAIs[i - 4].GetTarget(sp);
                    PerformSpell(sp, i, target);
                    cooldowns[i] = 0;
                    battleGo = CheckEndCase();
                }
            }
            //check end cases
        }
        for (int i = 0; i < numHeroes; i++)
        {
            if (tempFighters[i].HP > 0)
            {
                cooldowns[i] += Time.deltaTime;
                if (cooldowns[i] >= tempFighters[i].speed + castSpeed)
                {
                    //Debug.Log(i + " Attack!    " + cooldowns[i]);
                    PerformSpell(DecodeSpell(inputStrings[i], i), i, lastTarget[i]);
                    inputStrings[i] = "";
                    cooldowns[i] = 0;

                    fighterAnimators[i].SetBool("Ready", true); // FIX THIS LINE ITS ALMOST DONE
                    enableInput[i] = false;

                    //Auto Cursor on Exit
                    if (selectedCharacter == i)
                    {
                        float maxInCast = 0;
                        int maxId = -1;
                        for (int k = 0; k < 4; k++)
                        {
                            if (k != i && enableInput[k])
                            {
                                if (cooldowns[k] - tempFighters[k].speed > maxInCast && inputStrings[k].Equals(""))  //TODO Change for cast speeds by Stat
                                {
                                    maxInCast = cooldowns[k] - tempFighters[k].speed;
                                    maxId = k;
                                }
                            }
                        }
                        if (maxId >= 0)
                        {
                            selectedCharacter = maxId;
                        }
                    }

                    battleGo = CheckEndCase();
                }
                else if (cooldowns[i] >= tempFighters[i].speed)
                {
                    //Debug.Log(i + " Enabled Input!    " + cooldowns[i]);
                    bool firstInCast = true;
                    for (int k = 0; k < 4; k++)
                    {
                        if (k != i && enableInput[k])
                        {
                            firstInCast = false;
                            break;
                        }
                    }
                    if (firstInCast)
                    {
                        selectedCharacter = i;
                    }

                    enableInput[i] = true;
                }
            }
        }


        //receive cursor input CONTROLLER
        if (Input.GetButtonDown(enemyUp)) // enemy up
        {
            do
            {
                enemyCursor = 4 + (enemyCursor - 4 + 1) % numEnemy;
            } while (tempFighters[enemyCursor].HP <= 0);

            //Debug.Log("Switched target to " + enemyCursor);
        }
        if (Input.GetButtonDown(enemyDown)) // enemy down
        {
            int i = 0;
            do
            {
                i++;
                if (enemyCursor <= 4)
                {
                    enemyCursor = 3 + numEnemy;
                }
                else
                {
                    enemyCursor = enemyCursor - 1;
                }
            } while (tempFighters[enemyCursor].HP <= 0 && i < 4);

            // Debug.Log("Switched target to " + enemyCursor);
        }
        if (Input.GetButtonDown(heroUp)) // hero up
        {
            do
            {
                selectedCharacter = (selectedCharacter + 1) % numHeroes;
                enemyCursor = lastTarget[selectedCharacter];
            } while (tempFighters[selectedCharacter].HP <= 0);

            //Debug.Log("Switched target to " + selectedCharacter);
        }
        if (Input.GetButtonDown(heroDown)) // hero down
        {
            int i = 0;
            do
            {
                i++;
                if (selectedCharacter <= 0)
                {
                    selectedCharacter = numHeroes - 1;
                }
                else
                {
                    selectedCharacter = selectedCharacter - 1;
                }
                enemyCursor = lastTarget[selectedCharacter];
            } while ((tempFighters[selectedCharacter].HP <= 0 || !enableInput[selectedCharacter]) && i < 4);

            //Debug.Log("Switched target to " + selectedCharacter);
        }

        if (enableInput[selectedCharacter])
        {
            lastTarget[selectedCharacter] = enemyCursor;
            for (int i = 0; i < 8; i++)
            {
                if (Input.GetButtonDown(inputs[i]))
                {
                    //Debug.Log("You pressed: " + inputs[i]);
                    if (inputStrings[selectedCharacter].Length < 2 + (int)(tempFighters[selectedCharacter].meter / 100))
                    {
                        inputStrings[selectedCharacter] = inputStrings[selectedCharacter] + inputs[i];
                    }
                    else
                    {
                        inputStrings[selectedCharacter] = inputs[i];
                    }
                }
            }
        }
        //Item Usage
        if (false)
        {
            if (newSave.potionCount > 0)
            {
                newSave.potionCount -= 1;
                ReceiveSpell(selectedCharacter, new string[] { "HP" }, "none", new float[] { 50 }, new float[] { 100 }, "");
            }
        }
        if (false)
        {
            if (newSave.poisonHealCount > 0)
            {
                newSave.poisonHealCount -= 1;
                ReceiveSpell(selectedCharacter, new string[] { "Status" }, "none", new float[] { 0 }, new float[] { 100 }, "");
            }
        }
        if (false)
        {
            if (newSave.burnHealCount > 0)
            {
                newSave.burnHealCount -= 1;
                ReceiveSpell(selectedCharacter, new string[] { "Status" }, "none", new float[] { 3 }, new float[] { 100 }, "");
            }
        }
        if (false)
        {
            if (newSave.freezeHealCount > 0)
            {
                newSave.freezeHealCount -= 1;
                ReceiveSpell(selectedCharacter, new string[] { "Status" }, "none", new float[] { 1 }, new float[] { 100 }, "");
            }
        }
    }


    //Check for End Cases
    public bool CheckEndCase()
    {
        bool heroesDead = true;
        bool enemiesDead = true;
        float XPGained = 0;

        for (int i = 0; i < 4; i++)
        {
            if (tempFighters[i].HP > 0)
            {
                heroesDead = false;
                break;
            }
        }
        for (int i = 0; i < numEnemy; i++)
        {
            XPGained += tempFighters[4 + i].XP;
            if (tempFighters[4 + i].HP > 0)
            {
                enemiesDead = false;
                break;
            }
        }

   //Change End of Battle Info
        if (heroesDead)
        {
            //TO ADD
            //Debug.Log("GAME OVER");
        }
        else if (enemiesDead)
        {
            for (int i = 0; i < 4; i++)
            {
                if (tempFighters[i].HP > 0)
                {
                    safeFighters[i].XP += XPGained;
                }
                safeFighters[i].HP = tempFighters[i].HP;
            }
            newSave.coin += XPGained * 100;
            while(enemyLoaders.Count > 0)
            {
                Destroy(enemyLoaders[enemyLoaders.Count - 1]);
                enemyLoaders.RemoveAt(enemyLoaders.Count - 1);
            }

            //TO ADD

            //Debug.Log("Enemies Dead");
        }
        if (heroesDead || enemiesDead)
        {
            using (StreamWriter sw = new StreamWriter(savePath))
            {
                sw.WriteLine(JsonUtility.ToJson(safeFighters[0]));
                sw.WriteLine(JsonUtility.ToJson(safeFighters[1]));
                sw.WriteLine(JsonUtility.ToJson(safeFighters[2]));
                sw.WriteLine(JsonUtility.ToJson(safeFighters[3]));
                sw.WriteLine(JsonUtility.ToJson(newSave));
            }
            //Debug.Log("FIGHT IS OVER");
            //TO ADD
            
            
            return false;
        }
        return true;
    }

    //************************************************************************************************************************************
    //Spells

    public Spell DecodeSpell(string input, int charNumber)
    {
        //track down spell
        Spell spell = new Spell();
        switch (charNumber)
        {

            case 0:
                if (Char0_activeSpells.Contains(input))
                {
                    Char0Spells.TryGetValue(input, out spell);
                }
                break;
            case 1:
                if (Char1_activeSpells.Contains(input))
                {
                    Char1Spells.TryGetValue(input, out spell);
                }
                break;
            case 2:
                if (Char2_activeSpells.Contains(input))
                {
                    Char2Spells.TryGetValue(input, out spell);
                }
                break;
            case 3:
                if (Char3_activeSpells.Contains(input))
                {
                    Char3Spells.TryGetValue(input, out spell);
                }
                break;

        }
        return spell;
    }

    void PerformSpell(Spell spell, int charNumber, int enemy)
    {
        //can't be defending if attacking
        defending[charNumber] = false;
        //track down spell

        if (spell != null)
        {
            if (spell.Combo != null)
            {
                //decode spell
                //TODO add modifiers to power possibly if stat boosted
                if (spell.TargetType.Equals("Self"))
                {
                    ReceiveSpell(charNumber, spell.StatEffected, spell.Type, spell.Arg1, spell.Arg2, spell.ParticleEffect);
                }
                else if (spell.TargetType.Equals("Enemy"))
                {
                    ReceiveSpell(enemy, spell.StatEffected, spell.Type, spell.Arg1, spell.Arg2, spell.ParticleEffect);
                    /*
                    if (spell.Combo.Length == 2)
                    {
                        if (tempFighters[charNumber].meter + (int)spell.Arg1[1] > 200)
                        {
                            tempFighters[charNumber].meter = 200;
                        }
                        else
                        {
                            tempFighters[charNumber].meter += (int)spell.Arg1[1];
                        }

                    }
                    else
                    {
                        if (tempFighters[charNumber].meter + (int)spell.Arg1[1] < 0)
                        {
                            tempFighters[charNumber].meter = 0;
                        }
                        else
                        {
                            tempFighters[charNumber].meter += (int)spell.Arg1[1];
                        }
                    }
                    */
                }
                else if (spell.TargetType.Equals("AllAllies"))
                {
                    for (int i = 0; i < numHeroes; i++)
                    {
                        ReceiveSpell(i, spell.StatEffected, spell.Type, spell.Arg1, spell.Arg2, spell.ParticleEffect);
                    }
                }
                else if (spell.TargetType.Equals("RandomEnemy"))
                {
                    int rand;
                    do
                    {
                        rand = 4 + (int)(Random.value * numEnemy);
                    } while (tempFighters[rand].HP <= 0);
                    ReceiveSpell(rand, spell.StatEffected, spell.Type, spell.Arg1, spell.Arg2, spell.ParticleEffect);
                }
                else if (spell.TargetType.Equals("AllEnemies"))
                {
                    for (int i = 4; i < numEnemy; i++)
                    {
                        ReceiveSpell(i, spell.StatEffected, spell.Type, spell.Arg1, spell.Arg2, spell.ParticleEffect);
                    }
                }
                else if (spell.TargetType.Equals("RandomAlly"))
                {
                    if (charNumber <= 3)
                    {
                        int rand;
                        do
                        {
                            rand = (int)(Random.value * numHeroes);
                        } while (tempFighters[rand].HP <= 0);
                        ReceiveSpell(rand, spell.StatEffected, spell.Type, spell.Arg1, spell.Arg2, spell.ParticleEffect);
                    }
                    else
                    {
                        ReceiveSpell(enemy, spell.StatEffected, spell.Type, spell.Arg1, spell.Arg2, spell.ParticleEffect);
                    }
                }
            }
            //trigger animation for spell
            //Debug.Log(spell.CastAnim);
            try
            {
                fighterAnimators[charNumber].SetTrigger(spell.CastAnim);
            }
            catch { }

        }
        //trigger animation for fizzle

    }

    void ReceiveSpell(int index, string[] stat, string type, float[] arg1, float[] arg2, string particleName)
    {
        //check for arena effects


        //check for type effectiveness
        //negative values are not very effective, positive values are super effective
        //int typeEffect = TypeEffect(type, tempFighters[index].type);
        for (int i = 0; i < stat.Length; i++)
        {
            switch (stat[i])
            {
                case "HP":
                    //hits and healing
                    //arg1 is power.  Negative Values are Hits, Positive Values are heals
                    //arg2 is accuracy


                    if (tempFighters[index].HP > 0)
                    {
                        if (arg2[i] - Random.value * 100 > 0)
                        {
                            arg1[i] = (((tempFighters[index].Intelligence / 50) * arg1[i]) / 2);

                            if (arg1[i] < 0)
                            {
                                if (defending[index])
                                {
                                    arg1[i] = arg1[i] / 2;
                                    defending[index] = false;
                                }

                                //if(typeEffect != 0)
                                //{
                                //    arg1[i] = arg1[i] + arg1[i] / 2 * typeEffect;
                                //}

                                tempFighters[index].UpdateHP(arg1[i]);
                                //Debug.Log(tempFighters[selectedCharacter].name + " has attacked " + tempFighters[index].name + " for "
                                //+ ((tempFighters[index].Intelligence / 50) * arg1[i]) + " damage!");
                            }

                            else
                            {
                                tempFighters[index].UpdateHP(arg1[i]);
                                Debug.Log(tempFighters[selectedCharacter].name + " has healed " + tempFighters[index].name + " for "
                                + (arg1[i]) + " hp!");
                            }



                        }
                    }
                    break;
                case "Fainted":
                    //revival
                    //arg1 is power.  Degree of healing
                    //arg2 is accuracy
                    if (tempFighters[index].HP <= 0 && arg2[i] - Random.value * 100 > 0)
                    {
                        tempFighters[index].UpdateHP(arg1[i]);
                    }
                    break;
                case "AttackSpeed":
                    //slow down or speed up on timeline
                    //arg1 is percent multiplier. 200 being doubled speed and 50 being halfed 
                    //arg2 is accuracy
                    if (tempFighters[index].HP > 0)
                    {
                        if (arg2[i] - Random.value * 100 > 0)
                        {
                            tempFighters[index].speed = tempFighters[index].speed * (arg1[i] / 100);
                        }
                    }
                    break;
                case "StatusOn":
                    //turn on status effects
                    //arg1 decodes the status effected
                    //arg2 is accuracy
                    if (tempFighters[index].HP > 0)
                    {
                        if (arg2[i] - Random.value * 100 > 0)
                        {
                            if (tempFighters[index].status.CompareTo("") == 0)
                            {
                                switch (arg1[i])
                                {
                                    case 0:
                                        tempFighters[index].status = "poison";
                                        statusTimeStamps[index] = Time.time;
                                        break;
                                    case 1:
                                        tempFighters[index].status = "freeze";
                                        statusTimeStamps[index] = Time.time;
                                        break;
                                    case 2:
                                        tempFighters[index].status = "sleep";
                                        statusTimeStamps[index] = Time.time;
                                        break;
                                    case 3:
                                        tempFighters[index].status = "burn";
                                        statusTimeStamps[index] = Time.time;
                                        break;
                                    case 4:
                                        tempFighters[index].status = "sloth";
                                        statusTimeStamps[index] = Time.time;
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case "StatusOff":
                    //turn off status effects
                    //arg1 decodes the status effected
                    //arg2 is accuracy
                    if (tempFighters[index].HP > 0)
                    {
                        if (arg2[i] - Random.value * 100 > 0)
                        {
                            if (tempFighters[index].status.CompareTo("") != 0)
                            {
                                switch (arg1[i])
                                {
                                    case 0:
                                        if (tempFighters[index].status == "poison")
                                        {
                                            tempFighters[index].status = "";
                                        }
                                        break;
                                    case 1:
                                        if (tempFighters[index].status == "freeze")
                                        {
                                            tempFighters[index].status = "";
                                        }
                                        break;
                                    case 2:
                                        if (tempFighters[index].status == "sleep")
                                        {
                                            tempFighters[index].status = "";
                                        }
                                        break;
                                    case 3:
                                        if (tempFighters[index].status == "burn")
                                        {
                                            tempFighters[index].status = "";
                                        }
                                        break;
                                    case 4:
                                        if (tempFighters[index].status == "sloth")
                                        {
                                            tempFighters[index].status = "";
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case "Defend":
                    defending[index] = true;
                    break;
            }
        }
        //perform receive animation
        try { Instantiate(Resources.Load("Spell Catalog/" + type + "/" + particleName, typeof(GameObject)), allSpawnLocations[index].position, Quaternion.LookRotation(Vector3.left, Vector3.up)); }
        catch { }
    }

    int TypeEffect(string moveType, string charType)
    {
        switch (charType)
        {
            case "Nature":
                switch (moveType)
                {
                    case "Fire": return 1;
                    case "Ice": return 1;
                    case "Nature": return -1;
                    default: return 0;
                }
            case "Fire":
                switch (moveType)
                {
                    case "Fire": return -1;
                    case "Ice": return -1;
                    case "Nature": return -1;
                    case "Arcane": return 1;
                    case "Dark": return 1;
                    case "Storm": return 1;
                    default: return 0;
                }
            case "Ice":
                switch (moveType)
                {
                    case "Fire": return 1;
                    case "Ice": return -1;
                    case "Nature": return 1;
                    case "Arcane": return -1;
                    case "Light": return 1;
                    case "Storm": return -1;
                    default: return 0;
                }
            case "Dark":
                switch (moveType)
                {
                    case "Arcane": return -1;
                    case "Dark": return 1;
                    case "Light": return 1;
                    default: return 0;
                }
            case "Light":
                switch (moveType)
                {
                    case "Arcane": return -1;
                    case "Dark": return 1;
                    case "Light": return 1;
                    default: return 0;
                }
            case "Storm":
                switch (moveType)
                {
                    case "Nature": return 1;
                    case "Light": return 1;
                    case "Dark": return -1;
                    case "Storm": return -1;
                    default: return 0;
                }
            case "Arcane":
                switch (moveType)
                {
                    case "Arcane": return -1;
                    case "Dark": return -1;
                    case "Light": return -1;
                    default: return 0;
                }
        }
        return 0;
    }

}
