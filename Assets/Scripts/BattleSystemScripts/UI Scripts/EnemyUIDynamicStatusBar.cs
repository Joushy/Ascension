using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIDynamicStatusBar : MonoBehaviour
{
    GameManager gm;

    GameObject enemy2StatusBar;
    GameObject enemy3StatusBar;
    GameObject enemy4StatusBar;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        enemy2StatusBar = GameObject.Find("Enemy2 Status");
        enemy3StatusBar = GameObject.Find("Enemy3 Status");
        enemy4StatusBar = GameObject.Find("Enemy4 Status");
    }

    // Update is called once per frame
    void Update()
    {
        switch (gm.numEnemy)
        {
            case 1:
                enemy2StatusBar.SetActive(false);
                enemy3StatusBar.SetActive(false);
                enemy4StatusBar.SetActive(false);
                break;
            case 2:
                enemy3StatusBar.SetActive(false);
                enemy4StatusBar.SetActive(false);
                break;
            case 3:
                enemy4StatusBar.SetActive(false);
                break;
            case 4:
                enemy2StatusBar.SetActive(true);
                enemy3StatusBar.SetActive(true);
                enemy4StatusBar.SetActive(true);
                break;
            default:
                break;

        }
    }
}
