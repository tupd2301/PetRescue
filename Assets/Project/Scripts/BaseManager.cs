using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private GameObject basePrefab;

    public List<BaseData> bases = new List<BaseData>();

    private List<int> boardDesign = new List<int>();

    public void Init()
    {
        boardDesign = new List<int> { 3,4,5,4,3};
        CreateBoard(boardDesign);
    }

    public void CreateBoard(List<int> board)
    {
        bases = new List<BaseData>();
        int middle = board.Count / 2;
        for (int i = -middle; i < middle + 1; i++)
        {
            int middleBase = Mathf.FloorToInt(board[i + middle] / 2f);
            int extraCount = board[i + middle] % 2 == 0 ? 0 : 1;
            for (int j = -middleBase; j < middleBase + extraCount; j++)
            {
                float extra = board[i + middle] % 2 == 0 ? 1f : 0;
                GameObject baseObj = Instantiate(basePrefab, transform);
                baseObj.transform.localPosition =  new Vector3(i*1.75f, 0, j*2 + extra);
                baseObj.transform.localEulerAngles = new Vector3(0, 90, 0);
                baseObj.name = "Base (" + i + "," + j+")";
                baseObj.SetActive(false);
                BaseData newBase = new BaseData();
                newBase.id = i*middle + j;
                newBase.coordinates = new Vector2(i, j);
                newBase.obj = baseObj;
                bases.Add(newBase);
                if(i == -middle || i == middle || j == -middleBase || j == middleBase + extraCount -1)
                {
                    baseObj.GetComponent<BaseComponent>().SetModelSand();
                }
                else
                {
                    baseObj.GetComponent<BaseComponent>().RandomModel();
                }
            }
        }
        StartCoroutine(CallAnimationSpawn());
    }
    IEnumerator CallAnimationSpawn()
    {
        int currentX = 0;
        List<BaseData> sortedList = bases.OrderBy(x => x.coordinates.x).ToList();
        currentX = (int)sortedList[0].coordinates.x;
        foreach (BaseData item in sortedList)
        {
            if(currentX != (int)item.coordinates.x)
            {
                yield return new WaitForSeconds(0.3f);
            }
            item.obj.SetActive(true);
            item.obj.GetComponent<Animator>().Play("Spawn");
            currentX = (int)item.coordinates.x;
        }
    }
}
[System.Serializable]
public class BaseData
{
    public int id;
    public Vector2 coordinates;

    public GameObject obj;
}
