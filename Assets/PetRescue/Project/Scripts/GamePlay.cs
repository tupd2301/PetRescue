using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private TMP_Text _moveTxt;
    public static GamePlay Instance;
    public PetManager petManager;
    public BaseManager baseManager;
    public int level = 0;

    public List<LevelData> allLevelData = new List<LevelData>();

    private string config;

    public int move = 0;



    void Awake()
    {
        Instance = this;
        level = 10;
        config = "{3,0,4,4,1,3,0,3,0,2,17},{2,0,0,3,0,6,1,4,0,5,2,0,3,4,3,0,2,0,0,13},{2,5,0,3,0,4,2,4,0,3,0,0,3,4,0,1,2,6,2,13},{3,2,0,3,4,0,5,0,0,5,2,4,0,6,2,4,0,3,0,0,3,2,0,1,19},{2,0,4,3,2,-1,0,2,0,6,18},{3,3,0,0,4,0,0,4,0,5,0,2,-1,0,5,4,0,0,6,0,3,1,0,0,17},{3,5,4,0,4,2,-1,6,5,3,2,6,0,15},{3,3,5,4,4,0,-1,0,5,5,2,4,0,6,2,4,0,-1,0,3,3,5,2,1,19},{3,3,5,0,4,0,4,4,2,3,0,3,0,4,5,4,1,0,3,0,6,2,28},{2,0,3,3,3,8,6,2,6,0,14},{2,0,0,3,3,3,2,4,0,9,9,0,3,5,6,6,2,0,0,18},{3,3,0,4,4,0,0,3,0,5,2,8,5,0,2,4,0,0,2,0,3,5,0,1,20},{2,0,5,3,4,0,2,4,6,3,1,3,3,4,0,1,2,6,0,21},{4,0,5,4,0,5,2,1,0,1,6,6,1,4,5,4,2,0,5,2,4,0,2,4,4,0,5,6,0,23},{3,0,0,0,4,3,4,5,6,5,3,5,2,2,2,4,2,3,1,1,3,0,0,0,19},{3,0,3,0,4,0,6,2,0,5,0,4,5,4,3,6,2,6,2,0,3,0,5,6,1,2,6,0,4,0,5,2,0,3,0,6,0,26}";
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
            levelData.level = i - 1;
            levelData.config = levelConfigs[i];
            List<LineConfig> lineConfigs = new List<LineConfig>();

            string[] lineStrings = levelConfigs[i].Split(",");
            for (int j = 0; j < lineStrings.Length - 1; j++)
            {
                int num = Int32.Parse(lineStrings[j]);
                LineConfig lineConfig = new LineConfig();
                lineConfig.size = num;
                List<int> list = new List<int>();
                for (int k = 1; k <= num; k++)
                {
                    if (lineStrings.Count() <= j + k) break;
                    list.Add(Int32.Parse(lineStrings[j + k]));
                }
                lineConfig.petDirections = list;
                lineConfigs.Add(lineConfig);
                j += num;
            }
            levelData.lineConfigs = lineConfigs;
            levelData.moveMax = Int32.Parse(lineStrings[lineStrings.Count() - 1]);
            allLevelData.Add(levelData);
        }
    }
    void Start()
    {
        baseManager.Init(allLevelData[level].boardDesign);
        move = allLevelData[level].moveMax;
        UpdateMoveText();
    }

    public void UpdateMoveText()
    {
        _moveTxt.text = move.ToString();
    }

    public void Move()
    {
        move--;
        UpdateMoveText();
        if (move <= 0)
        {
            CheckWin();
        }
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
            move = allLevelData[level].moveMax;
            UpdateMoveText();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            level--;
            Reset();
            baseManager.Init(allLevelData[level].boardDesign);
            move = allLevelData[level].moveMax;
            UpdateMoveText();
        }
    }

    public void NextLevel()
    {
        CancelInvoke(nameof(NextLevel));
        if (level >= allLevelData.Count - 1) return;
        level++;
        Reset();
        baseManager.Init(allLevelData[level].boardDesign);
        move = allLevelData[level].moveMax;
    }

    public void Restart()
    {
        Reset();
        baseManager.Init(allLevelData[level].boardDesign);
        move = allLevelData[level].moveMax;
        UpdateMoveText();
    }

    public void CheckWin()
    {
        if (move > 0)
        {
            if (petManager.GetPetCount() == 0)
            {
                Debug.Log("Win");
                StartCoroutine(baseManager.SinkAll());
                Invoke(nameof(NextLevel), 3f);
            }
        }
        else
        {
            if (petManager.GetPetCount() > 0)
            {
                Debug.Log("Lose");
                StartCoroutine(baseManager.SinkAll());
                Invoke(nameof(Restart), 3f);
            }
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
    public int moveMax;
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