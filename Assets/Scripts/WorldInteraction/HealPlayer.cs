using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class HealPlayer : MonoBehaviour
{

    List<Fighter> fighters;
    [SerializeField] string savePath;
    Save newSave;

    // Start is called before the first frame update
    void Start()
    {


        fighters = new List<Fighter>();
        using (StreamReader sr = new StreamReader(savePath))
        {
            for (int i = 0; i < 4; i++)
            {
                string fighterString = sr.ReadLine();
                fighters.Add(JsonUtility.FromJson<Fighter>(fighterString));
            }
            string remainderOfSave = sr.ReadToEnd();
            newSave = JsonUtility.FromJson<Save>(remainderOfSave);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonUp("A"))
        {
            for (int i = 0; i < fighters.Count; i++)
            {
                fighters[i].HP = fighters[i].maxHP;
            }

            using (StreamWriter sw = new StreamWriter(savePath))
            {
                sw.WriteLine(JsonUtility.ToJson(fighters[0]));
                sw.WriteLine(JsonUtility.ToJson(fighters[1]));
                sw.WriteLine(JsonUtility.ToJson(fighters[2]));
                sw.WriteLine(JsonUtility.ToJson(fighters[3]));
                sw.WriteLine(JsonUtility.ToJson(newSave));
            }
        }
    }
}
