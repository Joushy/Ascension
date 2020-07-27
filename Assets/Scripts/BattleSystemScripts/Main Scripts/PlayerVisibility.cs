using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    GameManager gm;

    GameObject char1;
    GameObject char2;
    GameObject char3;
    GameObject char4;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        try
        {
            char1 = GameObject.Find("Klaus(Clone)");
            char2 = GameObject.Find("Ferox(Clone)");
            char3 = GameObject.Find("Meira(Clone)");
            char4 = GameObject.Find("Calid(Clone)");
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.numHeroes == 1)
        {
            char2.SetActive(false);
            char3.SetActive(false);
            char4.SetActive(false);
        }
        else if (gm.numHeroes == 2)
        {
            char3.SetActive(false);
            char4.SetActive(false);
        }
        else if (gm.numHeroes == 3)
        {
            char4.SetActive(false);
        }
        else if (gm.numHeroes == 4)
        {
            char1.SetActive(true);
            char2.SetActive(true);
            char3.SetActive(true);
            char4.SetActive(true);
        }
    }
}
