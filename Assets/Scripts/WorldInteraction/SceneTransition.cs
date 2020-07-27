using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{

    GameObject player;
    LevelChanger LevelChanger;

    [SerializeField] string nextArea;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;

    OverworldGameManager ogm;

    // Start is called before the first frame update
    void Start()
    {
        
        LevelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
        ogm = GameObject.Find("SceneManager").GetComponent<OverworldGameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonUp("A"))
        {
            LevelChanger.changeScenes(nextArea, x, y, z);
        }
    }

    
}
