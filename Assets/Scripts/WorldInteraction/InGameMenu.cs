using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using GameCreator.Characters;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] bool toggleStartMenu, toggleSelectMenu;
    [SerializeField] Vector2 WidthAndHeightOfInfoBox;
    public List<UICharInfo> characters;
    public List<GameObject> CharSpellBookUI;
    List<Transform> CharBookTabs;
    string savePath,spellPath;
    int slotsInView, charIndex, tabIndex;

    GameObject selectMenu, startMenu;
    enum UIWindow { None, startMenu, selectMenu};
    Save newSave;
    UIWindow currentWindow;

    PlayerCharacter player;
    bool l2;
    bool r2;


    /* READ ME if lost.
     *  List<UICharInfo> character[index] -> a list containing all the characters ie: Klaus, Ferox, Meira, Calid
     *  Example: character[2] would be Meira.
     *  UICharInfo is a class that inherits Fighter. Use the method SetAll(Fighter x) to save a Fighter class to the character.
     *  UICharInfo contains the following..
     *  ... List<Spell> All Spells -> (Every spell a character can have). Yes all of them
     *  ... List<List Tuple<string, Spell>> Active Spells..
     *  ......The outside list is the type of spell, the inside list is all the spells that belong to that type.
     *  Example: character[2].activeSpells[i][j]. the 'i' is the two types, for this case its Meira's Shadow or Ice.
     * ...The 'j' is the List of tuples, Item1, is the type. Item2 is the Spell itself. 
     */

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("TownPlayer").GetComponent<PlayerCharacter>();
        l2 = true;
        r2 = true;
        savePath = "/Scripts/BattleSystemScripts/Saves/Save0.JSON";
        spellPath = "/Scripts/BattleSystemScripts/Spellbooks";
        DirectoryInfo saveDirectory = new DirectoryInfo(Application.dataPath + savePath);
        DirectoryInfo spellDirectory = new DirectoryInfo(Application.dataPath + spellPath);

        characters = new List<UICharInfo>();
        int charNum = 0;

        using (StreamReader sr = new StreamReader(saveDirectory.FullName))
        {
            for (int i = 0; i < 4; i++)
            {
                string fighterString = sr.ReadLine();
                Fighter tempf = JsonUtility.FromJson<Fighter>(fighterString);

                UICharInfo newchar = new UICharInfo();
                characters.Add(newchar);
                characters[i].setAll(tempf);
                //characters[i].comboStrings = new List<string>();
                characters[i].types = new List<string>();
                characters[i].activeSpells = new List<List<Tuple<string, Spell>>>();
            }
            string remainderOfSave = sr.ReadToEnd();
            newSave = JsonUtility.FromJson<Save>(remainderOfSave);
        }


        foreach (var folder in spellDirectory.GetDirectories())
        {
            characters[charNum].allSpells = new List<Spell>();
            int typeIndex = 0;
            foreach (var file in folder.GetFiles("*.JSON"))
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    Spell newSpell = JsonUtility.FromJson<Spell>(sr.ReadToEnd());
                    Tuple<string, Spell> typeAndSpell = new Tuple<string, Spell>(newSpell.Type, newSpell);
       
                    if (!characters[charNum].types.Contains(newSpell.Type)){ //Unfinished, Klaus may break this once he has more than 2 
                        List<Tuple<string, Spell>> tupleList = new List<Tuple<string, Spell>>();
                        characters[charNum].types.Add(newSpell.Type); // Come back here to edit for Klaus eventually           
                        characters[charNum].activeSpells.Add(tupleList);
                        typeIndex = characters[charNum].activeSpells.Count - 1;
                        characters[charNum].activeSpells[typeIndex].Add(typeAndSpell);
                    }
                    else
                    {
                        for(int i = 0; i < characters[charNum].activeSpells.Count; i++)
                        {

                            if (characters[charNum].activeSpells[i][0].Item1 == newSpell.Type)
                            {
                                characters[charNum].activeSpells[i].Add(typeAndSpell);
                            }
                        }
                    }

                    characters[charNum].allSpells.Add(newSpell);
                }
            }
            charNum++;
        }

        currentWindow = UIWindow.None;
        float top = GameObject.Find("SpellBook(Klaus)").GetComponent<Image>().rectTransform.sizeDelta.y;
        slotsInView = (int)(top / WidthAndHeightOfInfoBox.y);
        //Debug.Log(top + " : " + slotsInView);
        getUIComponents();

    }

    // Update is called once per frame
    void Update()
    {

        switch (currentWindow)
        {
            case UIWindow.None:
                
                resetStartMenu();
                resetSelectMenu();
                if (player.characterLocomotion.isControllable)
                {
                    player.characterLocomotion.runSpeed = 17;
                } else
                {
                    player.characterLocomotion.runSpeed = 0;
                }
                
                break;
            case UIWindow.startMenu:
                resetSelectMenu();
                startMenu.SetActive(true);
                player.characterLocomotion.runSpeed = 0;
                break;
            case UIWindow.selectMenu:
                resetStartMenu();
                SelectMenuHandlerV1();
                player.characterLocomotion.runSpeed = 0;
                break;

        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentWindow)
            {
                case UIWindow.None:
                    break;
                case UIWindow.selectMenu:
                    currentWindow = UIWindow.None;
                    break;
                case UIWindow.startMenu:
                    currentWindow = UIWindow.None;
                    break;
            }
        }

        //Select Menu Button
        if (Input.GetButtonUp("Select"))
        {
            //Debug.Log("hello ");
            if (toggleSelectMenu) { toggleSelectMenu = false; currentWindow = UIWindow.None; }
            else { toggleSelectMenu = true; currentWindow = UIWindow.selectMenu; }           
        }else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (toggleStartMenu) { toggleStartMenu = false; currentWindow = UIWindow.None; }
            else { toggleStartMenu = true; currentWindow = UIWindow.startMenu; }
        }
    }

    void resetStartMenu()
    {
        toggleStartMenu = false;
        startMenu.SetActive(false);
    }

    void resetSelectMenu()
    {
        charIndex = 0;
        tabIndex = 0;
        toggleSelectMenu = false;
        selectMenu.SetActive(false);
        SelectCharSwapper(0);
    }

    void getUIComponents()
    {
        charIndex = 0;
        tabIndex = 0;
        //Gets the spellbook UI stuff.
        CharSpellBookUI = new List<GameObject>();
        CharSpellBookUI.Add(GameObject.Find("SpellBook(Klaus)"));
        CharSpellBookUI.Add(GameObject.Find("SpellBook(Ferox)"));
        CharSpellBookUI.Add(GameObject.Find("SpellBook(Meira)"));
        CharSpellBookUI.Add(GameObject.Find("SpellBook(Calid)"));


        //Gets the type UI stuff.
        CharBookTabs = new List<Transform>();
        for (int i = 0; i < CharSpellBookUI.Count; i++)
        {
            CharBookTabs.Add(CharSpellBookUI[i].transform.Find("Stats"));
            CharBookTabs.Add(CharSpellBookUI[i].transform.Find("Type1"));
            CharBookTabs.Add(CharSpellBookUI[i].transform.Find("Type2"));
        }
        startMenu = GameObject.Find("StartMenu");
        selectMenu = GameObject.Find("SelectMenu");
        SelectCharSwapper(0);

    }

    IEnumerator L2() { l2 = false;  yield return new WaitForSeconds(0.2f); l2 = true; }
    IEnumerator R2() { r2 = false;  yield return new WaitForSeconds(0.2f); r2 = true; }

    void SelectMenuHandlerV1()
    {
        selectMenu.SetActive(true);
        int i, k;
        if (Input.GetAxisRaw("L2") >= 1) //Input for moving left for character tabs;
        {
            if (l2)
            {
                
                StartCoroutine(L2());
                tabIndex = 0;
                if ((--charIndex) == -1) { charIndex = newSave.numHeroesUnlocked - 1; }
                i = charIndex % newSave.numHeroesUnlocked;
                SelectCharSwapper(i);
            }
            
            
        }
        else if (Input.GetAxisRaw("R2") >= 1) //Input for moving right for character tabs;
        {
            if (r2)
            {
                StartCoroutine(R2());
                charIndex++;
                i = charIndex % newSave.numHeroesUnlocked;
                SelectCharSwapper(i);
            }
            
        }
        else if (Input.GetButtonUp("R1")) //Input for moving left for Type tabs;
        {
            tabIndex++;
            k = tabIndex % 3;
            int c = charIndex % newSave.numHeroesUnlocked;
            //Debug.Log(c);
            changeTab(k, c);
            //Debug.Log("Name: " + characters[c].name + " / Type: " + characters[c].activeSpells[k][0].Item1 + " / Spell: " + characters[c].activeSpells[k][0].Item2.Name);
        }
        else if (Input.GetButtonUp("L1")) //Input for moving right for Type tabs;
        {
            if ((--tabIndex) == -1) { tabIndex = 2; }
            k = tabIndex % 3;
            int c = charIndex % newSave.numHeroesUnlocked;
            changeTab(k, c);

            //Debug.Log(characters[charIndex].name + " : " + characters[charIndex].activeSpells[k][0].Item1);
        }
    }

    void changeTab(int k, int c)
    {
        switch (k)
        {
            case 0:
                CharBookTabs[3 * c].gameObject.SetActive(true);
                CharBookTabs[3 * c + 1].gameObject.SetActive(false);
                CharBookTabs[3 * c + 2].gameObject.SetActive(false);
                break;
            case 1:
                CharBookTabs[3 * c].gameObject.SetActive(false);
                CharBookTabs[3 * c + 1].gameObject.SetActive(true);
                CharBookTabs[3 * c + 2].gameObject.SetActive(false);
                break;
            case 2:
                CharBookTabs[3 * c].gameObject.SetActive(false);
                CharBookTabs[3 * c + 1].gameObject.SetActive(false);
                CharBookTabs[3 * c + 2].gameObject.SetActive(true);
                break;
        }
    }

    void SelectCharSwapper(int i)
    {
        for (int j = 0; j < CharSpellBookUI.Count; j++)
        {
            if (i == j)
            {
                CharSpellBookUI[j].SetActive(true);
                //Debug.Log("Should be: Stats" );
                CharBookTabs[3 * j].gameObject.SetActive(true);
                CharBookTabs[3 * j + 1].gameObject.SetActive(false);
                CharBookTabs[3 * j + 2].gameObject.SetActive(false);
            }
            else
            {
                CharSpellBookUI[j].SetActive(false);
                CharBookTabs[3 * j].gameObject.SetActive(false);
                CharBookTabs[3 * j + 1].gameObject.SetActive(false);
                CharBookTabs[3 * j + 2].gameObject.SetActive(false);
                //changeTab(i, j);
            }
        }
    }
}
