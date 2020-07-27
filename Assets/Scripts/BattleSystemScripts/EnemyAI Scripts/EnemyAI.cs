using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyAI
{
    //Struct for associating spells and percentage chances
    struct SpellPercent
    {
        public int percent;
        public string name;
    }
    int numEnemy;
    int numHeroes;
    bool heroAboutToAttack;
    bool enemyAboutToAttack;
    List<SpellPercent> spellChances;

    int[] heroHPTargetPercentages;
    int[] heroSpeedTargetPercentages;
    int[] heroAttackImpendingTargetPercentages;

    int[] enemyHPTargetPercentages;
    int[] enemySpeedTargetPercentages;
    int[] enemyAttackImpendingTargetPercentages;
    int[] enemyFaintedTargetPercentages;

    //StatusConditions - Update Later
    int[] enemyFrozenTargetPercentages;
    int[] enemyFireTargetPercentages;
    int[] enemyPoisonTargetPercentages;
    int[] enemySleepTargetPercentages;
    int[] enemySlothTargetPercentages;


    Dictionary<string, Spell> allSpells;
    Dictionary<string, EnemyAIState> states;
    int percentageUpdateCount;
    int ID;

    




    public EnemyAI(string enemyName, int enemyID, int nE, int nH)
    {
        numEnemy = nE;
        numHeroes = nH;
        ID = enemyID;
        //populate target Arrays
        ResetTargets();

        //get all spells
        allSpells = new Dictionary<string, Spell>();
        states = new Dictionary<string, EnemyAIState>();
        spellChances = new List<SpellPercent>();
        Spell sp;
        StreamReader reader;
        string spellString;
        SpellPercent spellPercent;
        string[] a = Directory.GetFiles("Assets/Scripts/BattleSystemScripts/EnemyFighters/" + enemyName + "/Spellbook");  // TODO Replace with correct path
        foreach (string spell in a)
        {
            if (!spell.Contains("meta"))
            {
                reader = new StreamReader(spell);
                spellString = reader.ReadToEnd();
                sp = JsonUtility.FromJson<Spell>(spellString);
                allSpells.Add(sp.Name, sp);
                reader.Close();


                //add to structure for spells and percentage values
                spellPercent = new SpellPercent();
                spellPercent.percent = 0;
                spellPercent.name = sp.Name;
                spellChances.Add(spellPercent);
            }
        }
        percentageUpdateCount = 0;
        
        //get states
        EnemyAIState st;
        string stateString;
        using(StreamReader sr = new StreamReader("Assets/Scripts/BattleSystemScripts/EnemyFighters/" + enemyName + "/AIStates.JSON")) // TODO Replace with correct path
        {
            while (sr.Peek() > -1)
            {
                stateString = sr.ReadLine();
                st = JsonUtility.FromJson<EnemyAIState>(stateString);
                states.Add(st.condition, st);
            }
        }
    }


    //---------------------------------------------------------------------------------------------------------------------
    //Check Conditions to Update Spell Percentages and Target Percentages
    //TO ADD - CHANGES MADE HERE
    public void CheckCondition(List<Fighter> fighters, List<float> cooldowns)
    {
        //check heroes
        for (int i = 0; i < numHeroes; i++)
        {
            //This Fighter's HP is low
            if (fighters[i].HP * 1.0 / fighters[i].maxHP < 0.5f && fighters[i].HP > 0)
            {
                UpdatePercentage("Hero_HP_Low");
                UpdateTarget(true, i, "HP");
            }
            if (fighters[i].HP * 1.0 / fighters[i].maxHP < 0.25f && fighters[i].HP > 0)
            {
                UpdatePercentage("Hero_HP_Low");
                UpdateTarget(true, i, "HP");
            }
            if (fighters[i].HP * 1.0 / fighters[i].maxHP < 0.1f && fighters[i].HP > 0)
            {
                UpdatePercentage("Hero_HP_Low");
                UpdateTarget(true, i, "HP");
            }

            //This Fighters Speed is too fast
            if (fighters[ID].speed * 1.0 / fighters[i].speed > 1.25f)
            {
                UpdatePercentage("Hero_Speed_Fast");
                UpdateTarget(true, i, "Speed");
            }
            if (fighters[ID].speed * 1.0 / fighters[i].speed > 1.50f)
            {
                UpdatePercentage("Hero_Speed_Fast");
                UpdateTarget(true, i, "Speed");
            }
            if (fighters[ID].speed * 1.0 / fighters[i].speed > 1.75f)
            {
                UpdatePercentage("Hero_Speed_Fast");
                UpdateTarget(true, i, "Speed");
            }

            //Position On Timeline
            if(cooldowns[i] > fighters[i].speed)
            {
                UpdatePercentage("Hero_About_To_Attack");
                UpdateTarget(true, i, "Hero_About_To_Attack");
            }
        }

        //check enemies
        for (int i = 4; i < numEnemy; i++)
        {
            if(fighters[i].HP <= 0)
            {
                UpdatePercentage("Enemy_Fainted");
                UpdateTarget(false, i, "Fainted");
            }
            //This Fighter's HP is low
            if (fighters[i].HP * 1.0 / fighters[i].maxHP < 0.5f && fighters[i].HP > 0)
            {
                UpdatePercentage("Enemy_HP_Low");
                UpdateTarget(false, i, "HP");
            }
            if (fighters[i].HP * 1.0 / fighters[i].maxHP < 0.25f && fighters[i].HP > 0)
            {
                UpdatePercentage("Enemy_HP_Low");
                UpdateTarget(false, i, "HP");
            }
            if (fighters[i].HP * 1.0 / fighters[i].maxHP < 0.1f && fighters[i].HP > 0)
            {
                UpdatePercentage("Enemy_HP_Low");
                UpdateTarget(false, i, "HP");
            }

            //This Fighters Speed is too fast
            if (fighters[ID].speed * 1.0 / fighters[i].speed > 0.75f)
            {
                UpdatePercentage("Enemy_Speed_Slow");
                UpdateTarget(false, i, "Speed");
            }
            if (fighters[ID].speed * 1.0 / fighters[i].speed > 0.50f)
            {
                UpdatePercentage("Enemy_Speed_Slow");
                UpdateTarget(false, i, "Speed");
            }
            if (fighters[ID].speed * 1.0 / fighters[i].speed > 0.25f)
            {
                UpdatePercentage("Enemy_Speed_Slow");
                UpdateTarget(false, i, "Speed");
            }

            //Check Status Effect
            if (!fighters[i].status.Equals(""))
            {
                UpdatePercentage("Enemy_Has_Effect_" + fighters[i].status);
                UpdateTarget(false, i, fighters[i].status);
            }

            //Position On Timeline
            if (cooldowns[i] > fighters[i].speed)
            {
                UpdatePercentage("Enemy_About_To_Attack");
                UpdateTarget(false, i, "Enemy_About_To_Attack");
            }
        }

    }
    //TO ADD -Changes made here
    void UpdateTarget(bool isHero, int targetID, string statEffected)
    {
        //add 2
        if (isHero)
        {
            switch (statEffected)
            {
                case "HP":
                    heroHPTargetPercentages[targetID] += 2;
                    break;
                case "Speed":
                    heroSpeedTargetPercentages[targetID] += 2;
                    break;
                case "Hero_About_To_Attack":
                    heroAttackImpendingTargetPercentages[targetID] += 2;
                    break;
            }
        }
        else
        {
            targetID -= 4;
            switch (statEffected)
            {
                case "Fainted":
                    enemyFaintedTargetPercentages[targetID] += 2;
                    break;
                case "HP":
                    heroHPTargetPercentages[targetID] += 2;
                    break;
                case "Speed":
                    heroSpeedTargetPercentages[targetID] += 2;
                    break;
                case "Enemy_About_To_Attack":
                    enemyAttackImpendingTargetPercentages[targetID] += 2;
                    break;
                case "burn":
                    enemyFireTargetPercentages[targetID] += 2;
                    break;
                case "freeze":
                    enemyFrozenTargetPercentages[targetID] += 2;
                    break;
                case "poison":
                    enemyPoisonTargetPercentages[targetID] += 2;
                    break;
                case "sloth":
                    enemySlothTargetPercentages[targetID] += 2;
                    break;
                case "sleep":
                    enemySleepTargetPercentages[targetID] += 2;
                    break;
            }
        }
    }

    void UpdatePercentage(string condition)
    {
        try
        {
            //get state associated with condition
            EnemyAIState state = new EnemyAIState();
            states.TryGetValue(condition, out state);

            //find matching spell
            for (int i = 0; i < state.spellNames.Length; i++)
            {
                for (int k = 0; k < spellChances.Count; k++)
                {
                    if (spellChances[k].name.Equals(state.spellNames[i]))
                    {

                        //Update spell percentage
                        SpellPercent sp = new SpellPercent();
                        sp.name = state.spellNames[i];
                        sp.percent = spellChances[k].percent + state.spellPercents[i];
                        spellChances[k] = sp;
                        break;
                    }
                }
            }
            if (condition.Equals("Hero_About_To_Attack"))
            {
                heroAboutToAttack = true;
            }
            if (condition.Equals("Enemy_About_To_Attack"))
            {
                enemyAboutToAttack = true;
            }
            percentageUpdateCount++;
        }
        catch { }
    }


    //-------------------------------------------------------------------------------------------------------------------------------------
    //Get Spells and Targets


    public Spell GetSpell()
    {
        Spell spell = new Spell();

        //Random Spell if Condition Does not Update
        if (percentageUpdateCount <= 0)
        {
            int randomKey = (int)((Random.value * spellChances.Count) % spellChances.Count);
            allSpells.TryGetValue(spellChances[randomKey].name, out spell);
            return spell;
        }

        //Get Spell by percentage chance
        int roll = (int)Random.value * 100 * percentageUpdateCount;
        int count = 0;
        string chosen = "";
        for (int i = 0; i < spellChances.Count; i++)
        {
            if (count + spellChances[i].percent >= roll)
            {
                chosen = spellChances[i].name;
                break;
            }
            else
            {
                count += spellChances[i].percent;
            }
        }

        //reset spell percentages
        SpellPercent sp = new SpellPercent();
        for (int i = 0; i < spellChances.Count; i++)
        {
            sp.name = spellChances[i].name;
            sp.percent = 0;
            spellChances[i] = sp;
        }
        percentageUpdateCount = 0;

        //return spell
        allSpells.TryGetValue(chosen, out spell);
        return spell;
    }
    public int GetTarget(Spell spell)
    {
        if (spell.TargetType.Equals("Self"))
        {
            return ID;
        }
        else if (spell.TargetType.Equals("Enemy") || spell.TargetType.Equals("RandomEnemy"))
        {
            //other enemies
            if (enemyAboutToAttack)
            {
                return GetRandomEnemy(enemyAttackImpendingTargetPercentages);
            }
            if (spell.StatEffected[0].Equals("Fainted"))
            {
                return GetRandomEnemy(enemyFaintedTargetPercentages);
            }
            if (spell.StatEffected[0].Equals("HP"))
            {
                return GetRandomEnemy(enemyHPTargetPercentages);
            }
            if (spell.StatEffected[0].Equals("AttackSpeed"))
            {
                return GetRandomEnemy(enemySpeedTargetPercentages);
            }
            if (spell.StatEffected[0].Equals("StatusOn")) {
                switch (spell.Arg1[0])
                {
                    case 0:
                        return GetRandomEnemy(enemyPoisonTargetPercentages);
                    case 1:
                        return GetRandomEnemy(enemyFrozenTargetPercentages);
                    case 2:
                        return GetRandomEnemy(enemySleepTargetPercentages);
                    case 3:
                        return GetRandomEnemy(enemyFireTargetPercentages);
                    case 4:
                        return GetRandomEnemy(enemySlothTargetPercentages);
                }
            }
            return GetRandomEnemy();

        }
        else if (spell.TargetType.Equals("AllAllies"))
        {
            return -1;
        }
        else if (spell.TargetType.Equals("AllEnemies"))
        {
            return -2;
        }
        else if (spell.TargetType.Equals("RandomAlly"))
        {
            //any hero
            if (heroAboutToAttack && spell.StatEffected.Equals("StatusOn"))
            {
                return GetRandomHero(heroAttackImpendingTargetPercentages);
            }
            if (spell.StatEffected[0].Equals("HP"))
            {
                return GetRandomHero(heroHPTargetPercentages);
            }
            if (spell.StatEffected[0].Equals("AttackSpeed"))
            {
                return GetRandomHero(heroSpeedTargetPercentages);
            }
            return GetRandomHero();
        }
        return -999;
    }
    int GetRandomEnemy()
    {
        int roll = (int)((Random.value * numEnemy) % numEnemy);
        return roll;
    }
    int GetRandomEnemy(int[] enemyTargetPercentages)
    {
        //get sum of enemyTargetPercentages
        int sum = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += enemyTargetPercentages[i];
        }


        //roll for random
        int roll = (int)((Random.value * sum) % sum);
        int count = 0;
        int chosen = -999;

        //choose enemy

        for (int i = 0; i < numEnemy; i++)
        {
            if (count + enemyTargetPercentages[i] > roll)
            {
                chosen = i;
                break;
            }
            else
            {
                count += enemyTargetPercentages[i];
            }
        }
        ResetTargets();
        return chosen;
    }

    int GetRandomHero()
    {
        int roll = (int)((Random.value * numHeroes) % numHeroes);
        return roll;
    }
    int GetRandomHero(int[] heroTargetPercentages)
    {
        //get sum of heroTargetPercentages
        int sum = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += heroTargetPercentages[i];
        }
        //roll for random
        int roll = (int)((Random.value * sum) % sum);
        int count = 0;
        int chosen = -999;

        //choose hero
        for (int i = 0; i < numHeroes; i++)
        {
            if (count + heroTargetPercentages[i] > roll)
            {
                chosen = i;
                break;
            }
            else
            {
                count += heroTargetPercentages[i];
            }
        }
        ResetTargets();
        return chosen;
    }

    void ResetTargets()
    {
        heroAboutToAttack = false;
        enemyAboutToAttack = false;
        heroHPTargetPercentages = new int[] { 0, 0, 0, 0 };
        heroSpeedTargetPercentages = new int[] { 0, 0, 0, 0 };
        heroAttackImpendingTargetPercentages = new int[] { 0, 0, 0, 0 };

        enemyHPTargetPercentages = new int[] { 0, 0, 0, 0 };
        enemySpeedTargetPercentages = new int[] { 0, 0, 0, 0 };
        enemyAttackImpendingTargetPercentages = new int[] { 0, 0, 0, 0 };
        enemyFaintedTargetPercentages = new int[] { 0, 0, 0, 0 };

        //StatusConditions - Update Later
        enemyFrozenTargetPercentages = new int[] { 0, 0, 0, 0 };
        enemyFireTargetPercentages = new int[] { 0, 0, 0, 0 };
        enemyPoisonTargetPercentages = new int[] { 0, 0, 0, 0 };
        enemySleepTargetPercentages = new int[] { 0, 0, 0, 0 };
        enemySlothTargetPercentages = new int[] { 0, 0, 0, 0 };

        for (int i = 0; i < numHeroes; i++)
        {
            heroHPTargetPercentages[i] = 1;
            heroSpeedTargetPercentages[i] = 1;
            heroAttackImpendingTargetPercentages[1] = 1;
        }
        for (int i = 0; i < numEnemy; i++)
        {
            enemyHPTargetPercentages[i] = 1;
            enemySpeedTargetPercentages[i] = 1;
            enemyAttackImpendingTargetPercentages[i] = 1;
        }
    }
}
