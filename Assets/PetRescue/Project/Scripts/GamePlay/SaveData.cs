using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;
    private string filePath;

    public GameProcessData gameProcessData = new GameProcessData();

    void Awake()
    {
        Instance = this;
        filePath = Application.persistentDataPath + "/playerData.json";
        Debug.Log(filePath);
    }

    public GameProcessData GetGameProcessData()
    {
        return gameProcessData;
    }


    [ContextMenu("Save")]
    public void Save()
    {
        gameProcessData = new GameProcessData();
        if (!File.Exists(filePath)) PlayerPrefs.SetInt("level", GamePlay.Instance.currentLevelData.level);
        // return;
        GameProcessData data = new GameProcessData();
        data.bases = GamePlay.Instance.baseManager.bases;
        data.pets = GamePlay.Instance.petManager.pets;
        data.level = GamePlay.Instance.currentLevelData.level;
        data.moves = GamePlay.Instance.move;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        gameProcessData = new GameProcessData();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gameProcessData = JsonUtility.FromJson<GameProcessData>(json);
        }
        else
        {
            Debug.LogWarning("Save file not found");
            int level = PlayerPrefs.GetInt("level", 0);
            gameProcessData.level = level;
        }
    }
}

public class GameProcessData
{
    public int level;
    public int moves;
    public List<BaseData> bases = new List<BaseData>();
    public List<PetData> pets = new List<PetData>();
}
