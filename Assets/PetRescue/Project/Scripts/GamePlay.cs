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
    public GameObject cameraParent;
    public CameraControl cameraControl;
    public static GamePlay Instance;
    public PetManager petManager;
    public BaseManager baseManager;
    public EnvirontmentManager envManager;
    public int level = 0;

    public List<LevelData> allLevelData = new List<LevelData>();

    List<string> levelConfigs = new List<string>();

    public string config;

    public int move = 0;

    public LevelData currentLevelData;

    public System.Action OnPetJump;



    //-------
    [Header("Test (T)")]
    [SerializeField] private string testInput;

    public void Test()
    {
        if (testInput != "")
        {
            List<LevelData> levelData = ReadConfig(testInput, false);
            Init(levelData[0]);
            Invoke(nameof(UpdateMoveText), 0.1f);
        }
    }



    void Awake()
    {
        Instance = this;
        //level = 3;
        // config = "{2,0,4,3,1,3,0,2,0,2,17},{2,0,0,3,0,6,1,4,0,5,2,0,3,4,3,0,2,0,0,13},{2,5,0,3,0,4,2,4,0,3,0,0,3,4,0,1,2,6,2,13},{3,2,0,3,4,0,5,0,0,5,2,4,0,6,2,4,0,3,0,0,3,2,0,1,19},{2,0,4,3,2,-1,0,2,0,6,18},{3,3,0,0,4,0,0,4,0,5,0,2,-1,0,5,4,0,0,6,0,3,1,0,0,17},{3,5,4,0,4,2,-1,6,5,3,2,6,0,15},{3,3,5,4,4,0,-1,0,5,5,2,4,0,6,2,4,0,-1,0,3,3,5,2,1,19},{3,3,5,0,4,0,4,4,2,3,0,3,0,4,5,4,1,0,3,0,6,2,28},{2,0,3,3,3,8,6,2,6,0,14},{2,0,0,3,3,3,2,4,0,9,9,0,3,5,6,6,2,0,0,18},{3,3,0,4,4,0,0,3,0,5,2,8,5,0,2,4,0,0,2,0,3,5,0,1,20},{2,0,5,3,4,0,2,4,6,3,1,3,3,4,0,1,2,6,0,21},{4,0,5,4,0,5,2,1,0,1,6,6,1,4,5,4,2,0,5,2,4,0,2,4,4,0,5,6,0,23},{3,0,0,0,4,3,4,5,6,5,3,5,2,2,2,4,2,3,1,1,3,0,0,0,19},{3,0,3,0,4,0,6,2,0,5,0,4,5,4,3,6,2,6,2,0,3,0,5,6,1,2,6,0,4,0,5,2,0,3,0,6,0,26},{3,4,6,2,4,0,4,5,0,5,0,5,3,5,0,4,0,6,4,0,3,2,3,1,25},{2,0,0,3,0,4,5,4,0,7,-1,0,3,3,0,0,2,6,0,14},{3,2,3,0,4,0,6,9,1,3,2,0,5,16},{3,0,3,5,4,0,2,-1,0,5,2,8,5,4,0,4,6,3,-1,4,3,0,0,6,24},{3,0,0,4,4,0,0,9,0,5,0,1,0,3,0,6,0,2,8,5,0,0,5,0,3,0,4,5,4,0,3,5,0,3,0,0,0,20},{3,0,0,4,4,0,0,5,0,5,0,4,4,5,6,6,1,7,0,7,0,0,5,6,0,1,2,0,4,0,3,0,0,3,0,1,0,19},{3,4,0,2,4,0,0,1,6,5,0,3,8,4,4,6,3,6,8,5,2,0,5,0,1,8,6,6,4,0,0,2,4,3,2,6,3,32},{3,3,0,5,4,0,4,8,6,5,2,1,0,1,0,4,4,9,4,0,3,5,2,1,19},{4,0,3,6,0,5,1,-1,2,-1,2,6,0,5,0,0,1,4,5,2,-1,5,-1,5,4,0,1,4,0,15},{6,5,0,0,0,0,2,7,0,6,4,4,2,8,5,8,0,6,5,0,9,6,0,4,9,0,2,-1,5,5,6,5,-1,2,8,0,1,5,0,9,4,0,6,7,0,2,8,6,5,2,3,6,2,0,0,0,0,1,34},{4,0,0,0,0,5,3,0,0,0,5,6,1,2,0,10,0,6,5,0,0,0,0,0,4,0,0,0,0,15},{3,0,0,0,4,0,0,5,4,5,0,3,3,0,0,4,0,2,10,0,3,6,0,5,13},{4,2,0,3,0,5,1,3,1,4,6,6,0,0,0,10,5,0,5,0,6,5,0,0,4,0,2,1,0,16},{3,4,5,3,4,0,6,7,6,5,0,2,5,0,0,6,2,0,-1,0,4,6,5,2,5,2,0,1,4,0,9,6,6,3,2,1,0,31},{4,0,0,2,3,5,0,3,0,4,0,6,4,7,0,0,9,0,7,0,2,0,6,2,5,0,8,5,6,0,1,8,3,1,2,7,0,2,0,4,2,5,0,6,4,9,0,0,7,0,5,0,1,0,6,0,4,6,0,5,1,38}";
        ReadConfig(config);
    }

    List<LevelData> ReadConfig(string config, bool isOverride = true)
    {
        levelConfigs = new List<string>();
        List<LevelData> levelDatas = new List<LevelData>();
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

            //read size and directions
            string[] lineStrings = levelConfigs[i].Split(",");
            for (int j = 0; j < lineStrings.Length - 1; j++)
            {
                int num = Int32.Parse(lineStrings[j]);
                LineConfig lineConfig = new LineConfig();
                lineConfig.size = num;
                List<List<int>> listDirections = new List<List<int>>();
                for (int k = 1; k <= num; k++)
                {
                    List<int> ints = new List<int>();
                    if (lineStrings.Count() <= j + k) break;
                    if (lineStrings[j + k].Contains('['))
                    {
                        string[] valueString = lineStrings[j + k].Split("[")[1].Split("]");
                        if (valueString[0].Contains("|") == false)
                        {
                            ints.Add(Int32.Parse(valueString[0]));
                        }
                        else
                        {
                            ints.Add(Int32.Parse(valueString[1]));
                            string[] strings = valueString[0].Split("|");
                            for (int l = 0; l < strings.Length; l++)
                            {
                                ints.Add(Int32.Parse(strings[l]));
                            }
                        }
                    }
                    else
                    {
                        // Debug.Log(lineStrings[j + k]);
                        ints.Add(Int32.Parse(lineStrings[j + k]));
                    }
                    listDirections.Add(ints);
                }
                lineConfig.values = listDirections;
                lineConfigs.Add(lineConfig);
                j += num;
            }
            levelData.lineConfigs = lineConfigs;
            levelData.moveMax = Int32.Parse(lineStrings[lineStrings.Count() - 1]);
            levelDatas.Add(levelData);
        }
        if (isOverride)
        {
            allLevelData = levelDatas;
        }
        return levelDatas;
    }
    void Start()
    {
        // currentLevelData = allLevelData[level];
        // baseManager.Init(allLevelData[level].boardDesign);
        // move = allLevelData[level].moveMax;
        // UpdateMoveText();
        // testInput = $"{{{levelConfigs[level + 1]}}}";
        Init(allLevelData[level]);
        envManager.Init();
        OnPetJump = UnlockAll;
    }

    void Init(LevelData levelData)
    {
        currentLevelData = levelData;
        baseManager.Init(levelData.boardDesign);
        move = levelData.moveMax;
        UpdateMoveText();
        cameraControl.UpdateCamera(new Vector2(levelData.lineConfigs.Count, levelData.lineConfigs.Max(x => x.size)));
        testInput = $"{{{levelData.config}}}";
    }


    public BaseData GetBaseEnvironment(int posID = 0)
    {
        List<BaseData> list = new List<BaseData>();
        list = baseManager.bases.FindAll(x => x.obj.GetComponent<BaseComponent>().isHide == true).ToList();
        int maxY = (int)list.OrderByDescending(x => x.coordinates.y).ToList()[0].coordinates.y;
        int maxX = (int)list.OrderByDescending(x => x.coordinates.x).ToList()[0].coordinates.x;
        switch (posID)
        {
            case 1://top-right
                maxY = (int)list.OrderByDescending(x => x.coordinates.y).ToList()[0].coordinates.y;
                list = list.FindAll(x => x.coordinates.y == maxY).ToList();
                maxX = (int)list.OrderByDescending(x => x.coordinates.x).ToList()[0].coordinates.x;
                list = list.FindAll(x => x.coordinates.x == maxX).ToList();
                break;
            case -1://top-left
                maxY = (int)list.OrderByDescending(x => x.coordinates.y).ToList()[0].coordinates.y;
                list = list.FindAll(x => x.coordinates.y == maxY).ToList();
                maxX = (int)list.OrderBy(x => x.coordinates.x).ToList()[0].coordinates.x;
                list = list.FindAll(x => x.coordinates.x == maxX).ToList();
                break;
            case 2://bottom-right
                maxY = (int)list.OrderBy(x => x.coordinates.y).ToList()[0].coordinates.y;
                list = list.FindAll(x => x.coordinates.y == maxY).ToList();
                maxX = (int)list.OrderByDescending(x => x.coordinates.x).ToList()[0].coordinates.x;
                list = list.FindAll(x => x.coordinates.x == maxX).ToList();
                break;
            case -2://bottom-left
                maxY = (int)list.OrderBy(x => x.coordinates.y).ToList()[0].coordinates.y;
                list = list.FindAll(x => x.coordinates.y == maxY).ToList();
                maxX = (int)list.OrderBy(x => x.coordinates.x).ToList()[0].coordinates.x;
                list = list.FindAll(x => x.coordinates.x == maxX).ToList();
                break;
        }
        return list.FirstOrDefault();
    }

    public void UpdateMoveText()
    {
        int number = move;
        if (move < 0) number = 0;
        _moveTxt.text = "Moves: " + number.ToString();
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
        List<ValueData> petVector3 = baseManager.SpawnByValues(currentLevelData);
        petManager.SpawnPets(petVector3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StopAllCoroutines();
            Reset();
            Invoke(nameof(Test), 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StopAllCoroutines();
            level++;
            Reset();
            Init(allLevelData[level]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            StopAllCoroutines();
            level--;
            Reset();
            Init(allLevelData[level]);
        }
    }

    public void NextLevel()
    {
        CancelInvoke(nameof(NextLevel));
        StopAllCoroutines();
        if (level >= allLevelData.Count - 1) return;
        level++;
        Reset();
        Init(allLevelData[level]);
    }

    public void Restart()
    {
        Reset();
        Init(allLevelData[level]);
    }

    public void CheckWin()
    {
        if (move >= 0)
        {
            if (petManager.GetPetCount() == 0)
            {
                Debug.Log("Win");
                SoundManager.Instance.PlaySound("win");
                StartCoroutine(baseManager.SinkAll());
                Invoke(nameof(NextLevel), 3f);
            }
        }
        else
        {
            if (petManager.GetPetCount() > 0)
            {
                Debug.Log("Lose");
                SoundManager.Instance.PlaySound("lose");
                StartCoroutine(baseManager.SinkAll());
                Invoke(nameof(Restart), 3f);
            }
        }
    }

    public void UnlockAll()
    {
        List<BaseData> collection = baseManager.bases.FindAll(x => x.obj.GetComponent<BaseComponent>().isHide == false && x.obj.GetComponent<SpecialTileLock>()).ToList();
        foreach (var item in collection)
        {
            if (item.obj.GetComponent<SpecialTileLock>().isUnlocked == false)
            {
                item.obj.GetComponent<SpecialTileLock>().Unlock();
            }
        }
    }

    public List<int> GetValues(Vector2Int coordinates)
    {
        List<List<List<int>>> ints = currentLevelData.boardDesignValues;
        // Debug.Log(coordinates.x + " " + coordinates.y);
        return ints[coordinates.y][coordinates.x];
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
    public List<List<List<int>>> boardDesignValues
    {
        get
        {
            List<List<List<int>>> list = new List<List<List<int>>>();
            for (int i = 0; i < lineConfigs.Count; i++)
            {
                list.Add(lineConfigs[i].values);
            }
            return list;
        }
    }
}
[System.Serializable]
public class LineConfig
{
    public int size;
    public List<List<int>> values;
}