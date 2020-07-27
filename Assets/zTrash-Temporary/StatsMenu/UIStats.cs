using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class UIStats : MonoBehaviour
{

    OverworldGameManager ogm;

    Text hp;
    Text meter;
    Text exp;
    Text spellPower;
    Text speed;
    Text crit;
    Text type1;
    Text type2;
    Text level;
    Text vitality;
    Text intellect;
    Text resist;
    Text wisdom;
    Text pace;
    Text luck;

    List<Fighter> fighters;

    //Gerard
    List<Dictionary<string, Spell>> spellDictionaries;
    public string char0_path;
    public string char1_path;
    public string char2_path;
    public string char3_path;
    //Gerard

    InGameMenu IGM;
    List<Spell> spells;
    List<Text> spellText;
    [SerializeField] string savePath;
    string spellBookPath;
    [SerializeField] int fighterId;
    Save newSave;

    // Start is called before the first frame update
    void Start()
    {

        spellDictionaries = new List<Dictionary<string, Spell>>();
        fighters = new List<Fighter>();
        using (StreamReader sr = new StreamReader(savePath))
        {
            for (int i = 0; i < 4; i++)
            {
                string fighterString = sr.ReadLine();
                Debug.Log(fighterString);
                fighters.Add(JsonUtility.FromJson<Fighter>(fighterString));
            }
            string remainderOfSave = sr.ReadToEnd();
            newSave = JsonUtility.FromJson<Save>(remainderOfSave);
        }

        spellDictionaries.Add(GetAllSpells(char0_path));
        spellDictionaries.Add(GetAllSpells(char1_path));
        spellDictionaries.Add(GetAllSpells(char2_path));
        spellDictionaries.Add(GetAllSpells(char3_path));


        IGM = GameObject.Find("CanvasMenu").GetComponent<InGameMenu>();
        hp = GameObject.Find("HPNum"+fighterId).GetComponent<Text>();
        meter = GameObject.Find("MeterNum" + fighterId).GetComponent<Text>();
        exp = GameObject.Find("ExpNum" + fighterId).GetComponent<Text>();
        spellPower = GameObject.Find("SpellNum" + fighterId).GetComponent<Text>();
        speed = GameObject.Find("Speed" + fighterId).GetComponent<Text>();
        crit = GameObject.Find("CritNum" + fighterId).GetComponent<Text>();
        type2 = GameObject.Find("Type2" + fighterId).GetComponent<Text>();
        vitality = GameObject.Find("VitalityNum" + fighterId).GetComponent<Text>();
        intellect = GameObject.Find("IntelNum" + fighterId).GetComponent<Text>(); 
        resist = GameObject.Find("ResistanceNum" + fighterId).GetComponent<Text>(); 
        wisdom = GameObject.Find("WisdomNum" + fighterId).GetComponent<Text>(); 
        pace = GameObject.Find("PaceNum" + fighterId).GetComponent<Text>(); 
        luck = GameObject.Find("LuckNum" + fighterId).GetComponent<Text>(); 
        level = GameObject.Find("LevelNum" + fighterId).GetComponent<Text>();

        //spellText = new List<Text>();
        //spellText.Add(GameObject.Find("SpellName1").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName2").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName3").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName4").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName5").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName6").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName7").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName8").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName9").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName10").GetComponent<Text>());
        //spellText.Add(GameObject.Find("SpellName11").GetComponent<Text>());

    }

    // Update is called once per frame
    void Update()
    {
        setStats();
        setSpells();

    }

    void setStats()
    {
        try
        {
            level.text = "" + fighters[fighterId].Level;
            hp.text = "" + fighters[fighterId].HP + " / " + fighters[fighterId].maxHP;
            meter.text = "" + fighters[fighterId].meter + " / 200";
            exp.text = "" + fighters[fighterId].XP + " / 1000";
            spellPower.text = "" + ((fighters[fighterId].Level * ((fighters[fighterId].intelligence / 2)))) + "%";
            // (Level * (10 + (intelligence / 2)))
            speed.text = fighters[fighterId].speed + "s";
            //crit.text = fighters[0].luck +"%";
            //type2.text = fighters[fighterId].type + "";
            vitality.text = "" + (int)fighters[fighterId].vitality;
            intellect.text = "" + (int)fighters[fighterId].intelligence;
            resist.text = "" + (int)fighters[fighterId].resistance;
            wisdom.text = "" + (int)fighters[fighterId].wisdom;
            pace.text = "" + (int)fighters[fighterId].pace;
            //luck.text = "" + fighters[fighterId].luck;
        }
        catch { }

    }

    void setSpells()
    {
        spells = getSpellsForSelectChar(fighterId);

        for (int i = 0; i < spells.Count; i++)
        { 
            //spellText[i].text = spells[i].Name;
        }
    }

    List<Spell> getSpellsForSelectChar(int j)
    {
        List<Spell> spells = new List<Spell>();
        Spell currentSpell = new Spell();

        for(int i = 0; i < fighters[j].unlockedSpells.Length; i++)
        {
            spellDictionaries[j].TryGetValue(fighters[j].unlockedSpells[i],out currentSpell);
            spells.Add(currentSpell);
        }
        return spells;
    }


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

}
