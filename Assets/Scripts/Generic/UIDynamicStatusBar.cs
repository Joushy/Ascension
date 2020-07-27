using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDynamicStatusBar : MonoBehaviour
{
    GameManager gm;

    GameObject feroxStatusBar;
    GameObject meiraStatusBar;
    GameObject calidStatusBar;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        feroxStatusBar = GameObject.Find("Ferox Status");
        meiraStatusBar = GameObject.Find("Meira Status");
        calidStatusBar = GameObject.Find("Calid Status");
    }

    // Update is called once per frame
    void Update()
    {
        switch (gm.numHeroes)
        {
            case 1:
                feroxStatusBar.SetActive(false);
                meiraStatusBar.SetActive(false);
                calidStatusBar.SetActive(false);
                break;
            case 2:
                meiraStatusBar.SetActive(false);
                calidStatusBar.SetActive(false);
                break;
            case 3:
                calidStatusBar.SetActive(false);
                break;
            case 4:
                feroxStatusBar.SetActive(true);
                meiraStatusBar.SetActive(true);
                calidStatusBar.SetActive(true);
                break;
            default:
                break;

        }
    }
}
