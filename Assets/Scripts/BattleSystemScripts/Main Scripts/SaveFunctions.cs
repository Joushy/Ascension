using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFunctions : MonoBehaviour
{
    public string saveFolder;
    int index;
    Save newSave;
    bool loading;

    void Start()
    {
        index = 1;
        loading = false;
    }
    // Start is called before the first frame update

    void Update()
    {
        if (!loading)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                index = 1;
                Debug.Log(index);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                index = 2;
                Debug.Log(index);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                index = 3;
                Debug.Log(index);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadGame(index);
                Debug.Log("Load: " + index);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame(index);
                Debug.Log("Reset: " + index);
            }
        }
    }



    void LoadGame(int i)
    {
        loading = true;
        using (StreamReader sr = new StreamReader(saveFolder + "/save" + i + ".json"))
        {
            using (StreamWriter sw = new StreamWriter(saveFolder + "/save0.json"))
            {
                sw.Write(sr.ReadToEnd());
            }
        }
        string[] Fighters = new string[4];
        using (StreamReader sr = new StreamReader(saveFolder + "/save0.json"))
        {
            Fighters[0] = sr.ReadLine();
            Fighters[1] = sr.ReadLine();
            Fighters[2] = sr.ReadLine();
            Fighters[3] = sr.ReadLine();
            newSave = JsonUtility.FromJson<Save>(sr.ReadLine());
        }
        newSave.nextArea = newSave.lastCheckpoint;
        newSave.nextAreaXYZ = newSave.lastCheckpointXYZ;
        using (StreamWriter sw = new StreamWriter(saveFolder + "/save0.json"))
        {
            sw.Write(Fighters[0]);
            sw.Write(Fighters[1]);
            sw.Write(Fighters[2]);
            sw.Write(Fighters[3]);
            sw.Write(JsonUtility.ToJson(newSave));
        }
        //Transition to next scene




    }
    void ResetGame(int i)
    {
        using (StreamReader sr = new StreamReader(saveFolder + "/saveEmpty.json"))
        {
            using (StreamWriter sw = new StreamWriter(saveFolder + "/save" + i + ".json"))
            {
                sw.Write(sr.ReadToEnd());
            }
        }
    }
}
