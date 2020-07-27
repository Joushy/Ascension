using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UICharInfo : Fighter
{
    //public List<string> comboStrings { get; set; } //Done
    public List<Spell> allSpells { get; set; } //Done
    public List<string> types { get; set; } //Done
    public List<List<Tuple<string, Spell>>> activeSpells { get; set; } //Done
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public UICharInfo setAll(Fighter input)
    {
        this.name = input.name;
        this.maxHP = input.maxHP;
        this.HP = input.HP;
        this.status = input.status;
        this.unlockedSpells = input.unlockedSpells;
        return this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
