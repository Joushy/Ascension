using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCursorMovement : MonoBehaviour
{
    GameManager gm;

    GameObject char1;
    GameObject char2;
    GameObject char3;
    GameObject char4;

    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        mr = GetComponentInChildren<MeshRenderer>();
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
        if(gm.enableInput[gm.selectedCharacter])
        {
            //mr.enabled = true;
            mr.gameObject.SetActive(true);
        } else
        {
            //mr.enabled = false;
            mr.gameObject.SetActive(false);
        }

        //Debug.Log(gm.selectedCharacter);
        if (gm.enemyCursor == 4)
        {
            transform.position = char1.transform.position;
        }
        if (gm.enemyCursor == 5)
        {
            transform.position = char2.transform.position;
        }
        if (gm.enemyCursor == 6)
        {
            transform.position = char3.transform.position;
        }
        if (gm.enemyCursor == 7)
        {
            transform.position = char4.transform.position;
        }

    }
}
