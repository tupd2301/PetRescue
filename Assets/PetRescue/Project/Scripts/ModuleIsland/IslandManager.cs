using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public List<Island> listIsland = new List<Island>();

    public IslandData currentIslandData;

    public GameObject currentIsland;

    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        QualitySettings.SetQualityLevel(0, true);
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            LoadIsland(currentIslandData.id + 1);
        }
    }

    public void Init()
    {
        LoadIsland(0);
    }

    public IslandData GetIslandDataById(int id)
    {
        return listIsland.FirstOrDefault(x => x.islandData.id == id).islandData;
    }

    public void LoadIsland(int id = 0)
    {
        id = id % (listIsland.Count);
        IslandData islandData = GetIslandDataById(id);
        if (islandData != null)
        {
            currentIslandData = islandData;
            if (currentIsland != null)
                Destroy(currentIsland);
            Debug.Log("LoadIsland: " + islandData.path);
            currentIsland = Instantiate(
                Resources.Load<GameObject>("Prefabs/Islands/" + islandData.path),
                transform
            );
            currentIsland.transform.localPosition = islandData.position;
            currentIsland.transform.localEulerAngles = Vector3.zero;
            currentIsland.transform.localScale = islandData.scale;
            currentIsland.name = islandData.name;
        }
    }
}

[System.Serializable]
public class IslandData
{
    public int id;
    public string name;
    public string path;
    public Vector3 position;
    public Vector3 scale = Vector3.one;
}
