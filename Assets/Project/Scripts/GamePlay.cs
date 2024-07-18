using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public static GamePlay Instance;
    public PetManager petManager;
    public BaseManager baseManager;
    public int level = 0;

    public List<LevelData> allLevelData = new List<LevelData>();

    private string config;

    void Awake()
    {
        Instance = this;
        level = 0;
        config = "{2,3,0,3,0,2,1,2,6,0,17},{3,0,6,0,4,0,5,1,0,4,0,4,2,0,3,0,3,0,13},{3,0,1,0,4,4,3,0,6,4,0,2,0,1,3,0,3,5,20},{3,2,0,1,4,0,0,5,0,5,1,4,0,0,6,4,0,3,2,0,3,1,0,1,19},{2,3,0,3,0,-1,5,2,1,0,18},{3,0,0,4,4,0,3,0,0,5,2,0,-1,5,0,4,0,1,0,0,3,0,0,6,17},{1,4,2,3,5,3,4,-1,5,2,1,1,15},{3,3,4,1,4,4,0,5,2,5,2,-1,0,0,6,4,0,3,-1,1,3,1,0,4,19},{4,4,3,0,0,5,2,3,2,6,1,4,0,0,3,5,28},{2,2,5,3,0,7,0,2,2,5,14},{3,0,1,0,4,0,2,8,5,4,2,8,5,0,3,0,4,0,18},{3,3,0,1,4,0,2,0,0,5,2,0,4,1,6,4,0,7,0,0,3,1,0,4,20},{3,1,5,0,4,6,5,0,4,4,3,0,1,2,3,0,2,4,21}";
        ReadConfig(config);
    }
    void ReadConfig(string config)
    {
        List<string> levelConfigs = new List<string>();
        string[] levelStrings = config.Split("{"); 
        for (int i = 0; i < levelStrings.Length; i++)
        {
            string levelString = levelStrings[i].Split("}")[0];
            levelConfigs.Add(levelString);
        }

        for (int i = 1; i < levelConfigs.Count; i++)
        {
            LevelData levelData = new LevelData();
            levelData.level = i;
            levelData.config = levelConfigs[i];
            List<LineConfig> lineConfigs = new List<LineConfig>();

            string[] lineStrings = levelConfigs[i].Split(",");
            for (int j = 0; j < lineStrings.Length-1; j++)
            {
                int num = Int32.Parse(lineStrings[j]);
                LineConfig lineConfig = new LineConfig();
                lineConfig.size = num;
                List<int> list = new List<int>();
                for (int k = 1; k <= num; k++)
                {
                    if(lineStrings.Count() <= j + k) break;
                    list.Add(Int32.Parse(lineStrings[j + k]));
                }
                lineConfig.petDirections = list;
                lineConfigs.Add(lineConfig);
                j+=num;
            }
            levelData.lineConfigs = lineConfigs;
            
            allLevelData.Add(levelData);
        }
    }
    void Start()
    {
        baseManager.Init(allLevelData[level].boardDesign);
        
    }
    public void SpawnPets()
    {
        List<Vector3Int> petVector3 = baseManager.SpawnPets(allLevelData[level].boardDesign, allLevelData[level].lineConfigs);
        petManager.SpawnPets(petVector3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            level++;
            Reset();
            baseManager.Init(allLevelData[level].boardDesign);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            level--;
            Reset();
            baseManager.Init(allLevelData[level].boardDesign);
        }
    }

    public void CheckWin()
    {
        if(petManager.GetPetCount() == 0)
        {
            level++;
            Reset();
            baseManager.Init(allLevelData[level].boardDesign);
        }
    }

    void Reset()
    {
        baseManager.Reset();
        petManager.Reset();
    }
}

[System.Serializable]
public class LevelData
{
    public int level;
    public string config;
    public List<LineConfig> lineConfigs;
    public List<int> boardDesign
    {
        get
        {
            List<int> list = new List<int>();
            for (int i = 0; i < lineConfigs.Count; i++)
            {
                list.Add(lineConfigs[i].size);
            }
            return list;
        }
    }
}
[System.Serializable]
public class LineConfig
{
    public int size;
    public List<int> petDirections;
}