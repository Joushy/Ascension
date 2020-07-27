using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Characters;

public class EnemyEncounter : MonoBehaviour
{

    PlayerCharacter player;
    OverworldGameManager ogm;
    LevelChanger LevelChanger;

    [SerializeField] string area;
    public bool isBoss;

    // Start is called before the first frame update
    void Start()
    {
        LevelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
        ogm = GameObject.Find("SceneManager").GetComponent<OverworldGameManager>();
        player = GameObject.FindGameObjectWithTag("TownPlayer").GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TownPlayer")
        {
            LevelChanger.changeBattleScenes(area, gameObject.name);
            player.characterLocomotion.runSpeed = 0;
            player.characterLocomotion.isControllable = false;

        }
    }
}
