using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetMovement : MonoBehaviour
{
    GameManager gm;
    [SerializeField] int myHero;

    GameObject char1;
    GameObject char2;
    GameObject char3;
    GameObject char4;

    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        mr = GetComponent<MeshRenderer>();
        try
        {
            char1 = GameObject.Find("EnemySpawnPoint1");
            char2 = GameObject.Find("EnemySpawnPoint2");
            char3 = GameObject.Find("EnemySpawnPoint3");
            char4 = GameObject.Find("EnemySpawnPoint4");
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.enableInput[myHero] && gm.selectedCharacter != myHero && !gm.inputStrings[myHero].Equals(""))
        {
            mr.enabled = true;
        }
        else
        {
            mr.enabled = false;
        }

        Debug.Log(gm.selectedCharacter);
        if (gm.lastTarget[myHero] == 4)
        {
            transform.position = char1.transform.position;
        }
        if (gm.lastTarget[myHero] == 5)
        {
            transform.position = char2.transform.position;
        }
        if (gm.lastTarget[myHero] == 6)
        {
            transform.position = char3.transform.position;
        }
        if (gm.lastTarget[myHero] == 7)
        {
            transform.position = char4.transform.position;
        }

    }
}
