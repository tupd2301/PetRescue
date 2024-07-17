using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private GameObject basePrefab;

    public List<BaseData> bases = new List<BaseData>();

    private List<int> boardDesign = new List<int>();

    private List<BaseData> sandList = new List<BaseData>();

    public void Init()
    {
        boardDesign = new List<int> { 8,9,4,5,6,7,6,5,4,9,8};
        CreateBoard(boardDesign);
    }

    public void CreateBoard(List<int> board)
    {
        bases = new List<BaseData>();
        int middle = board.Count / 2;
        for (int y = middle; y >= -middle; y--)
        {
            int middleBase = Mathf.FloorToInt(board[y + middle] / 2f);
            int extraCount = board[y + middle] % 2 == 0 ? 0 : 1;
            for (int x = -middleBase; x < middleBase + extraCount; x++)
            {
                float extra = board[y + middle] % 2 == 0 ? 1f : 0;
                GameObject baseObj = Instantiate(basePrefab, transform);
                baseObj.transform.localPosition =  new Vector3(y*-1.75f, 0,x*2f + extra);
                baseObj.transform.localEulerAngles = new Vector3(0, 90, 0);
                int newX = x;
                if(y == -1) newX = x - y;
                if(y < -1) newX = x - (y-1)/2;
                if(y > 1) newX = x - y/2;
                if(y == 1) newX = x;
                baseObj.name = "Base (" + newX + "," + y+")";
                baseObj.GetComponentInChildren<TMP_Text>().text = newX + "," + y;
                baseObj.SetActive(false);
                BaseData newBase = new BaseData();
                newBase.id =newX*middle + y;
                newBase.coordinates = new Vector2(newX, y);
                newBase.obj = baseObj;
                bases.Add(newBase);
                baseObj.GetComponent<BaseComponent>().baseData = newBase;
                if(y == -middle || y == middle || x == -middleBase || x == middleBase + extraCount -1)
                {
                    baseObj.GetComponent<BaseComponent>().SetModelSand();
                    sandList.Add(newBase);
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

    int step = 0;
    BaseData data;

    public Dictionary<int, BaseData> CalculateDestinationForPet(Vector2 baseCoordinates, HexDirection direction = HexDirection.Up)
    {
        step = 0;
        data = new BaseData();
        CalculateDestination(baseCoordinates, direction);
        return new Dictionary<int, BaseData> { { step++, data } };
    }

    private void CalculateDestination(Vector2 baseCoordinates, HexDirection direction = HexDirection.Up)
    {
        Vector2 des = baseCoordinates;
        step++;
        switch (direction)
        {
            case HexDirection.Up:
                des += new Vector2(0, 1);
                break;
            case HexDirection.UpRight:
                des += new Vector2(1, 0);
                break;
            case HexDirection.UpLeft:
                des += new Vector2(-1, 1);
                break;
            case HexDirection.Down:
                des += new Vector2(0, -1);
                break;
            case HexDirection.DownRight:
                des += new Vector2(1, -1);
                break;
            case HexDirection.DownLeft:
                des += new Vector2(-1, 0);
                break;
            default: break;
        }
        BaseData baseData = bases.FirstOrDefault(x => x.coordinates == des);
        Debug.Log("GetBaseDestination: " + des);
        if(baseData != null)
        {
            CalculateDestination(des, direction);
        }
        else
        {
            data = bases.FirstOrDefault(x => x.coordinates == baseCoordinates);
        }
    }


    public Dictionary<int, BaseData> GetBaseDestination(PetData petData)
    {
        return CalculateDestinationForPet(petData.baseCoordinates, petData.direction);
    }
}
[System.Serializable]
public class BaseData
{
    public int id;
    public Vector2 coordinates;

    public GameObject obj;
}
