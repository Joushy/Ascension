using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusControl : MonoBehaviour
{

    GameManager gm;

    Image fighterHPImg;
    float fighterMaxHP;
    float fighterHP;

    Image fighterMeterImg;
    float fighterMaxMeter;
    float fighterMeter;

    Text fighterHPNum;
    Text fighterMeterNum;

    [SerializeField] int fighterId;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fighterHPImg = transform.Find("HP").GetComponent<Image>();
        fighterHPNum = transform.Find("Health Number Displayed").GetComponent<Text>();

        fighterMeterImg = transform.Find("MP").GetComponent<Image>();
        fighterMeterNum = transform.Find("MP Number Displayed").GetComponent<Text>();

        fighterMaxMeter = 200f;


    }

    // Update is called once per frame
    void Update()
    {

        fighterMaxHP = gm.tempFighters[fighterId].maxHP;
        fighterHP = gm.tempFighters[fighterId].HP;
        fighterMeter = gm.tempFighters[fighterId].meter;


        fighterHPImg.fillAmount = fighterHP / fighterMaxHP;
        fighterHPNum.text = fighterHP.ToString();

        fighterMeterImg.fillAmount = fighterMeter / fighterMaxMeter;
        fighterMeterNum.text = fighterMeter.ToString();

        
    }
}
