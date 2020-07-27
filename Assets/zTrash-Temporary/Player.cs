using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator anim;

    public string floor;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
        {
            anim.SetBool("Running", true);
        }
        else
        { anim.SetBool("Running", false); }
    }

    // for sound effect footsteps
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            floor = "Water"; }

        if (other.tag == "Wood")
        { floor = "Wood"; }

        if (other.tag == "Dirt")
        {
            floor = "Dirt"; }

        if (other.tag == "Snow")
        { floor = "Snow"; }

    }
}



