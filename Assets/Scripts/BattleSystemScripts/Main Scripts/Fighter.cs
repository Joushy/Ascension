using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter
{
    public string name;

    // HP
    public int maxHP;
    public int HP;
    public int vitality;

    // Damage
    public float baseAttack;
    public float intelligence;

    // Defense 
    public float baseDefense;
    public float resistance;

    // Accuracy 
    public float accuracyModifier;
    public float wisdom;

    // Pace
    public float speed;
    public float pace;

    // Mana / Meter
    public int meter;

    // XP until next level
    public float XP;
    public int Level;


    public string status;
    public string []types;
    public string[] unlockedSpells;


    public float Vitality
    {
        get { return (Level * (10 + (vitality / 2))); }
        set { maxHP = (Level * (10 + (vitality / 2))); }
    }
    public float Intelligence
    {
        get { return (Level * (10 + (intelligence / 2))); }
        set { baseAttack = (Level * (10 + (intelligence/2))); }
    }
    public float Resistance
    {
        get { return (Level * (10 + (resistance / 2))); }
        set { baseDefense = (Level * (10 + (resistance / 2))); }
    }
    public float Wisdom
    {
        get { return (Level * (10 + (wisdom / 2))); }
        set { accuracyModifier = (Level * (10 + (wisdom / 2))); }
    }
    public float Pace
    {
        get { return (Level * (10 + (pace / 2))); }
        set { speed = (Level * (10 + (pace / 2))); }
    }

    void GainXP(float enemyXP)
    {
        XP = enemyXP + XP;
        // WORK HERE
    }

    void MeterUp(float power)
    {
        if(meter + power > 200)
        {
            meter = 200;
        } else
        {
            meter = meter - (int)power;
        }
    }

    public void MeterDown(int spentBoxes)
    {
        meter -= spentBoxes * 100;
    }

    public void UpdateHP(float power)
    {
        if(HP + power < 0)
        {
            HP = 0;
        }
        else if(HP + power > maxHP)
        {
            HP = maxHP;
        } else
        {
            HP = HP + (int)power;
        }

        MeterUp(power/2);
    }

    override
    public string ToString()
    {
        return name + speed;
    }
}
