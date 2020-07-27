using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimelineControl : MonoBehaviour
{
    //[SerializeField] GameObject obj;
    GameManager gm;

    Image startOfWaitPosition;
    Image startCastPosition;
    Image endPosition;

    float waitFraction;
    float castFraction;

    float fraction;
    float speedOfLerp;

    RectTransform rectTrans;

    [SerializeField] int fighterId;
    [SerializeField] string start;
    [SerializeField] string middle;
    [SerializeField] string end;

    GameObject feroxBubble;
    GameObject meiraBubble;
    GameObject calidBubble;

    GameObject enemy2Bubble;
    GameObject enemy3Bubble;
    GameObject enemy4Bubble;

    void Awake()
    {
        rectTrans = GetComponent<RectTransform>();

        feroxBubble = GameObject.Find("FeroxBubble");
        meiraBubble = GameObject.Find("MeiraBubble");
        calidBubble = GameObject.Find("CalidBubble");

        enemy2Bubble = GameObject.Find("Enemy2Bubble");
        enemy3Bubble = GameObject.Find("Enemy3Bubble");
        enemy4Bubble = GameObject.Find("Enemy4Bubble");

        speedOfLerp = 0f;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        startOfWaitPosition = GameObject.Find(start).GetComponent<Image>();
        startCastPosition = GameObject.Find(middle).GetComponent<Image>();
        endPosition = GameObject.Find(end).GetComponent<Image>();

        rectTrans.position = startOfWaitPosition.rectTransform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //position on the timeline
        HandleTimelinePosition();


        // visual cue on ui
        AllyUI();
        EnemyUI();

    }

    void HandleTimelinePosition()
    {
        try
        {
            fraction = speedOfLerp * Time.deltaTime;

            waitFraction = gm.cooldowns[fighterId] / gm.tempFighters[fighterId].speed;
            castFraction = (gm.cooldowns[fighterId] - gm.tempFighters[fighterId].speed) / gm.castSpeed;
        }
        catch { }

        if (castFraction > 0)
        {
            rectTrans.position = Vector3.Lerp(startCastPosition.rectTransform.position, endPosition.rectTransform.position, castFraction);
        }
        else
        {
            rectTrans.position = Vector3.Lerp(startOfWaitPosition.rectTransform.position, startCastPosition.rectTransform.position, waitFraction);
        }
    }

    void AllyUI()
    {
        switch (gm.numHeroes)
        {
            case 1:
                feroxBubble.SetActive(false);
                meiraBubble.SetActive(false);
                calidBubble.SetActive(false);
                break;
            case 2:
                meiraBubble.SetActive(false);
                calidBubble.SetActive(false);
                break;
            case 3:
                calidBubble.SetActive(false);
                break;
            case 4:
                feroxBubble.SetActive(true);
                meiraBubble.SetActive(true);
                calidBubble.SetActive(true);
                break;
            default:
                break;
        }
    }

    void EnemyUI()
    {
        switch (gm.numEnemy)
        {
            case 1:
                enemy2Bubble.SetActive(false);
                enemy3Bubble.SetActive(false);
                enemy4Bubble.SetActive(false);
                break;
            case 2:
                enemy3Bubble.SetActive(false);
                enemy4Bubble.SetActive(false);
                break;
            case 3:
                enemy4Bubble.SetActive(false);
                break;
            case 4:
                enemy2Bubble.SetActive(true);
                enemy3Bubble.SetActive(true);
                enemy4Bubble.SetActive(true);
                break;
            default:
                break;
        }
    }
}
