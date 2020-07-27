using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBoxesManager : MonoBehaviour
{

    GameManager gm;

    GameObject box1;
    GameObject box2;
    GameObject box3;
    GameObject box4;
    GameObject box5;


    List<TextMesh> inputBoxes;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        box1 = GameObject.Find("Bubble1");
        box2 = GameObject.Find("Bubble2");
        box3 = GameObject.Find("Bubble3");
        box4 = GameObject.Find("Bubble4");
        //box5 = GameObject.Find("Bubble5");

        inputBoxes = new List<TextMesh>();

        inputBoxes.Add(GameObject.Find("InputText1").GetComponent<TextMesh>());
        inputBoxes.Add(GameObject.Find("InputText2").GetComponent<TextMesh>());
        inputBoxes.Add(GameObject.Find("InputText3").GetComponent<TextMesh>());
        inputBoxes.Add(GameObject.Find("InputText4").GetComponent<TextMesh>());
        //inputBoxes.Add(GameObject.Find("InputText5").GetComponent<TextMesh>());
    }

    // Update is called once per frame
    void Update()
    {
        checkActiveBoxes();

        //if (gm.enableInput[gm.selectedCharacter])
        //{

        //    if (inputBoxes[4].text != "")
        //    {
        //        inputBoxes[0].text = "";
        //        inputBoxes[1].text = "";
        //        try
        //        {
        //            inputBoxes[2].text = "";
        //            inputBoxes[3].text = "";
        //            inputBoxes[4].text = "";
        //        }
        //        catch { }
        //    }

        //    inputBoxes[0].text = gm.inputStrings[gm.selectedCharacter][0] + "";
        //    inputBoxes[1].text = gm.inputStrings[gm.selectedCharacter][1] + "";

        //    try
        //    {
        //        inputBoxes[2].text = gm.inputStrings[gm.selectedCharacter][2] + "";
        //        inputBoxes[3].text = gm.inputStrings[gm.selectedCharacter][3] + "";
        //        inputBoxes[4].text = gm.inputStrings[gm.selectedCharacter][4] + "";
        //    }
        //    catch
        //    { }
            
        //}

        

    }


    void checkActiveBoxes()
    {
        if (gm.tempFighters[gm.selectedCharacter].meter == 300)
        {
            box3.SetActive(true);
            box4.SetActive(true);
            //box5.SetActive(true);
        }
        else if (gm.tempFighters[gm.selectedCharacter].meter >= 200)
        {
            box3.SetActive(true);
            box4.SetActive(true);
            //box5.SetActive(false);
        }
        else if (gm.tempFighters[gm.selectedCharacter].meter >= 100)
        {
            box3.SetActive(true);
            box4.SetActive(false);
            //box5.SetActive(false);
        }
        else
        {
            box1.SetActive(true);
            box2.SetActive(true);
            box3.SetActive(false);
            box4.SetActive(false);
            //box5.SetActive(false);
        }
    }
}
