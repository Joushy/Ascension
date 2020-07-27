using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Demo : MonoBehaviour {
    public enum T { Spell, Projectiles, Aura, Shield, Variations }
  
    public T demo;
    public GameObject[] SpellList;
    public GameObject[] CastingList;
    public GameObject[] AuraList;
    public GameObject[] ShieldList;
    public GameObject[] VariationsList;

    public Text Title;
    public int Selection = 0;

    public GameObject BackText;
    public GameObject NextText;
    public GameObject BackButton;
    public GameObject NextButton;

    public GameObject SpellsGroup;
    public GameObject CastingGroup;
    public GameObject AuraGroup;
    public GameObject ShieldGroup;
    public GameObject VariationsGroup;

    public GameObject SelectionSpells;
    public GameObject SelectionProjectiles;
    public GameObject SelectionAura;
    public GameObject SelectionShields;
    public GameObject SelectionVariations;


    void Start () {
        SpellList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();
    }
    void Update()
    {
      
        if (Selection == 0)
        {
            BackText.SetActive(false);
            BackButton.SetActive(false);
        }
        else
        {
            BackText.SetActive(true);
            BackButton.SetActive(true);
        }

        if(demo == T.Spell)
        {
            if (Selection == SpellList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }

            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionSpells.SetActive(true);
        }

        if (demo == T.Projectiles)
        {
            if (Selection == CastingList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionProjectiles.SetActive(true);
        }
        if (demo == T.Aura)
        {
            if (Selection == AuraList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionAura.SetActive(true);
        }
        if (demo == T.Shield)
        {
            if (Selection == ShieldList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionVariations.SetActive(false);
            SelectionShields.SetActive(true);
        }
        if (demo == T.Variations)
        {
            if (Selection == VariationsList.Length - 1)
            {
                NextText.SetActive(false);
                NextButton.SetActive(false);
            }
            else
            {
                NextText.SetActive(true);
                NextButton.SetActive(true);
            }
            SelectionSpells.SetActive(false);
            SelectionProjectiles.SetActive(false);
            SelectionAura.SetActive(false);
            SelectionShields.SetActive(false);
            SelectionVariations.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Back();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Next();
        }


    }
    public void Back()
    {
       
        if(demo == T.Spell)
        {
            if (Selection < SpellList.Length && Selection != 0)
            {
                SpellList[Selection].SetActive(false);
                Selection -= 1;
                SpellList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Projectiles)
        {
            if (Selection < CastingList.Length && Selection != 0)
            {
                CastingList[Selection].SetActive(false);
                Selection -= 1;
                CastingList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + CastingList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Aura)
        {
            if (Selection < AuraList.Length && Selection != 0)
            {
                AuraList[Selection].SetActive(false);
                Selection -= 1;
                AuraList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + AuraList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Shield)
        {
            if (Selection < ShieldList.Length && Selection != 0)
            {
                ShieldList[Selection].SetActive(false);
                Selection -= 1;
                ShieldList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + ShieldList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Variations)
        {
            if (Selection < VariationsList.Length && Selection != 0)
            {
                VariationsList[Selection].SetActive(false);
                Selection -= 1;
                VariationsList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + VariationsList[Selection].gameObject.transform.name.ToString();
            }
        }
    }

    public void Next()
    {
        if (demo == T.Spell)
        {
            if (Selection < SpellList.Length && Selection != SpellList.Length - 1)
            {
                SpellList[Selection].SetActive(false);
                Selection += 1;
                SpellList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();
            }
        }

        if (demo == T.Projectiles)
        {
            if (Selection < CastingList.Length && Selection != CastingList.Length - 1)
            {
                CastingList[Selection].SetActive(false);
                Selection += 1;
                CastingList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + CastingList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Aura)
        {
            if (Selection < AuraList.Length && Selection != AuraList.Length - 1)
            {
                AuraList[Selection].SetActive(false);
                Selection += 1;
                AuraList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + AuraList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Shield)
        {
            if (Selection < ShieldList.Length && Selection != ShieldList.Length - 1)
            {
                ShieldList[Selection].SetActive(false);
                Selection += 1;
                ShieldList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + ShieldList[Selection].gameObject.transform.name.ToString();
            }
        }
        if (demo == T.Variations)
        {
            if (Selection < VariationsList.Length && Selection != VariationsList.Length - 1)
            {
                VariationsList[Selection].SetActive(false);
                Selection += 1;
                VariationsList[Selection].SetActive(true);
                Title.text = "Prefab Name: " + VariationsList[Selection].gameObject.transform.name.ToString();
            }
        }
    }

    public void Spells()
    {
        Last();
        demo = T.Spell;
        Selection = 0;
        SpellList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + SpellList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        SpellsGroup.SetActive(true);
    }
    public void Projectiles()
    {
        Last();
        demo = T.Projectiles;
        Selection = 0;
        CastingList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + CastingList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        CastingGroup.SetActive(true);
    }
    public void Auras()
    {
        Last();
        demo = T.Aura;
        Selection = 0;
        AuraList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + AuraList[Selection].gameObject.transform.name.ToString();

       
        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        AuraGroup.SetActive(true);
    }

    public void Shields()
    {
        Last();
        demo = T.Shield;
        Selection = 0;
        ShieldList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + ShieldList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        VariationsGroup.SetActive(false);
        ShieldGroup.SetActive(true);
    }

    public void Variations()
    {
        Last();
        demo = T.Variations;
        Selection = 0;
        VariationsList[Selection].SetActive(true);
        Title.text = "Prefab Name: " + VariationsList[Selection].gameObject.transform.name.ToString();

        AuraGroup.SetActive(false);
        CastingGroup.SetActive(false);
        SpellsGroup.SetActive(false);
        ShieldGroup.SetActive(false);
        VariationsGroup.SetActive(true);
    }

    public void Last()
    {
        if (Selection < SpellList.Length)
        {
            SpellList[Selection].SetActive(false);
        }
        if (Selection < CastingList.Length)
        {
            CastingList[Selection].SetActive(false);
        }
        
        if (Selection < AuraList.Length)
        {
            AuraList[Selection].SetActive(false);
        }
        if (Selection < ShieldList.Length)
        {
            ShieldList[Selection].SetActive(false);
        }
        
        if (Selection < VariationsList.Length)
        {
            VariationsList[Selection].SetActive(false);
        }
        
    }
}
