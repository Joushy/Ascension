using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusControlEnemies : MonoBehaviour
{
    GameManager gm;

    Image fighterHPImg;
    float fighterMaxHP;
    float fighterHP;

    Text txt;

    [SerializeField] int fighterId;

    // Start is called before the first frame update
    void Start()
    {
        
        try {
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            fighterHPImg = transform.Find("Enemy HP").GetComponent<Image>();
            txt = transform.Find("Name").GetComponent<Text>();

            
        }
        catch
        {

        }

        if (gm.newSave.numEnemies == 4)
        {
            fighterId = 4 + ((fighterId - 1) % 4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        try {
            txt.text = gm.tempFighters[fighterId].name;
            fighterHP = gm.tempFighters[fighterId].HP;
            fighterMaxHP = gm.tempFighters[fighterId].maxHP;

            fighterHPImg.fillAmount = fighterHP / fighterMaxHP;

            
        }
        catch { }

        
    }
}
