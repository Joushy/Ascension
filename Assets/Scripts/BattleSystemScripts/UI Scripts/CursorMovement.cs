using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{

    GameManager gm;

    GameObject char1;
    GameObject char2;
    GameObject char3;
    GameObject char4;

    Animator anim1;
    Animator anim2;
    Animator anim3;
    Animator anim4;

    Image klausImage;
    Image feroxImage;
    Image meiraImage;
    Image calidImage;


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

            anim1 = char1.GetComponent<Animator>();
            anim2 = char2.GetComponent<Animator>();
            anim3 = char3.GetComponent<Animator>();
            anim4 = char4.GetComponent<Animator>();

            klausImage = GameObject.Find("KlausHighlight").GetComponent<Image>();
            feroxImage = GameObject.Find("FeroxHighlight").GetComponent<Image>();
            meiraImage = GameObject.Find("MeiraHighlight").GetComponent<Image>();
            calidImage = GameObject.Find("CalidHighlight").GetComponent<Image>();
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {

        if (gm.selectedCharacter == 0)
        {
            transform.position = char1.transform.position;

            klausImage.enabled = true;
            feroxImage.enabled = false;
            meiraImage.enabled = false;
            calidImage.enabled = false;

            anim1.SetBool("Ready", true);
            anim2.SetBool("Ready", false);
            anim3.SetBool("Ready", false);
            anim4.SetBool("Ready", false);
        } 

        if (gm.selectedCharacter == 1)
        {
            transform.position = char2.transform.position;

            klausImage.enabled = false;
            feroxImage.enabled = true;
            meiraImage.enabled = false;
            calidImage.enabled = false;

            anim2.SetBool("Ready", true);
            anim1.SetBool("Ready", false);
            anim3.SetBool("Ready", false);
            anim4.SetBool("Ready", false);
        }

        if (gm.selectedCharacter == 2)
        {
            transform.position = char3.transform.position;

            klausImage.enabled = false;
            feroxImage.enabled = false;
            meiraImage.enabled = true;
            calidImage.enabled = false;

            anim3.SetBool("Ready", true);
            anim1.SetBool("Ready", false);
            anim2.SetBool("Ready", false);
            anim4.SetBool("Ready", false);
        }

        if (gm.selectedCharacter == 3)
        {
            transform.position = char4.transform.position;
            anim4.SetBool("Ready", true);
            anim1.SetBool("Ready", false);
            anim2.SetBool("Ready", false);
            anim3.SetBool("Ready", false);

            klausImage.enabled = false;
            feroxImage.enabled = false;
            meiraImage.enabled = false;
            calidImage.enabled = true;
        }

        bool anyoneInCast = false;
        for (int k = 0; k < 4; k++)
        {
            if (gm.enableInput[k])
            {
                anyoneInCast = true;
                break;
            }
        }
        if (!anyoneInCast)
        {
            transform.position = new Vector3(-999f,-999f,-999f);
            anim4.SetBool("Ready", false);
            anim1.SetBool("Ready", false);
            anim2.SetBool("Ready", false);
            anim3.SetBool("Ready", false);

            klausImage.enabled = false;
            feroxImage.enabled = false;
            meiraImage.enabled = false;
            calidImage.enabled = false;
        }

    }
}
