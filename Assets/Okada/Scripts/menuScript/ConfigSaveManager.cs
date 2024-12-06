using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class SaveData
{
    public string Controllertype;
    public string Name;
    public string Throw;
    public string Catch;
    public string Sleep;
    public string Jump;
}

public class ConfigSaveManager:MonoBehaviour
{
    private string folderName = "save";
    private void Awake()
    {
        string _savefolderPath = Application.dataPath + "/" + folderName;
        if (!Directory.Exists(_savefolderPath))
        {
            Directory.CreateDirectory(_savefolderPath);
        }

        for(int i = 1; i <= 6; i++)
        {
            if(!File.Exists(_savefolderPath + "/" + $"saveslot{i}.json"))
            {

            }
        }
    }
    private string GetSavePath(int path)
    {
        return Application.dataPath + "/" + folderName + $"saveslot{path}.json";
    }

    public void KeyConfigSave(SaveData data, int slot)
    {
        string json = JsonUtility.ToJson(data);
        string path = GetSavePath(slot);

        StreamWriter wr = new StreamWriter(path, false);
        wr.WriteLine(json);
        wr.Close();
    }

    public SaveData LoadSaveData(int slot)
    {
        string path = GetSavePath(slot);
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<SaveData>(json);
    }

    
    public void DeleteSave(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    //public string[] GetAllSaveFiles()
    //{
    //    string[] files = Directory.GetFiles(Application.persistentDataPath, )
    //}
}

