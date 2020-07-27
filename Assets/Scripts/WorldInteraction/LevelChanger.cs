using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{

    GameObject player;
    Animator levelChanger;

    SceneTransition sc;

    OverworldGameManager ogm;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        levelChanger = GameObject.Find("LevelChanger").GetComponent<Animator>();
        try { ogm = GameObject.Find("SceneManager").GetComponent<OverworldGameManager>(); }
        catch { }
        
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitFade()
    {
        levelChanger.SetTrigger("FadeOut");
        yield return new WaitForSeconds(3f);
        ogm.TransitionToNextArea();
        levelChanger.SetTrigger("FadeIn");
    }

    IEnumerator WaitBattleFade()
    {
        levelChanger.SetTrigger("FadeOut"); // levelChanger.SetTrigger("BattleFadeOut");
        yield return new WaitForSeconds(2.75f);
        ogm.TransitionToNextArea();
        levelChanger.SetTrigger("FadeIn"); // levelChanger.SetTrigger("BattleFadeOut");
    }

    IEnumerator GMBattleEndFade()
    {
        levelChanger.SetTrigger("FadeOut"); // levelChanger.SetTrigger("BattleFadeOut");
        yield return new WaitForSeconds(2.75f);
        levelChanger.SetTrigger("FadeIn"); // levelChanger.SetTrigger("BattleFadeOut");
    }

    public void changeScenes(string s, float x, float y, float z)
    {
        ogm.ChooseNextArea(s, x, y, z);
        StartCoroutine(WaitFade());
    }

    public void changeBattleScenes(string area, string charName)
    {
        ogm.Encounter(area, charName);
        StartCoroutine(WaitBattleFade());
    }

    public void BattleEnd()
    {
        StartCoroutine(GMBattleEndFade());
    }

}
