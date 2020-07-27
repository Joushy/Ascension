using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBoxText : MonoBehaviour
{
    GameManager gm;
    TextMesh txt;

    [SerializeField] int BoxNumber;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        txt = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            txt.text = gm.inputStrings[gm.selectedCharacter][BoxNumber] + "";
        } catch
        {
            txt.text = "";
        }
    }
}
