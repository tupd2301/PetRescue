using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private GameObject basePrefab;

    public List<BaseData> bases = new List<BaseData>();

    private List<int> boardDesign = new List<int>();

    private List<BaseData> sandList = new List<BaseData>();

    public void Reset()
    {
        foreach (var item in bases)
        {
            Destroy(item.obj);
        }
        bases = new List<BaseData>();
        sandList = new List<BaseData>();
    }

    public void Init(List<int> ints)
    {
        boardDesign = ints;
        CreateBoardEmpty(boardDesign);
    }

    public BaseData GetRandomBaseEmpty()
    {
        List<BaseData> list = bases.FindAll(x => x.obj.GetComponent<BaseComponent>().isHide == false && GamePlay.Instance.petManager.CheckPetExist(x.coordinates) == false).ToList();
        return list[Random.Range(0, list.Count)];
    }

    private void CreateBoardEmpty(List<int> board)
    {
        List<int> boardEmpty = new List<int>();
        //Create
        int max = 11;
        boardEmpty = new List<int>(Enumerable.Repeat(max, max));

        //Edit
        for (int i = 0; i < boardEmpty.Count; i++)
        {
            if (i % 2 == 0)
            {
                boardEmpty[i] += 1;
            }
            else
            {
                boardEmpty[i] += 0;
            }
        }
        CreateBoard(boardEmpty, board);
    }

    public List<Vector2> GetActiveBoard(List<int> board)
    {
        List<Vector2> activeBoard = new List<Vector2>();
        int middle = board.Count / 2;
        int add = board.Count % 2 == 1 ? 1 : 0;
        for (int y = middle; y > -middle - add; y--)
        {
            int middleBase = Mathf.FloorToInt(board[-y + middle] / 2f);
            int extraCount = board[-y + middle] % 2 == 0 ? 0 : 1;
            for (int x = -middleBase; x < middleBase + extraCount; x++)
            {
                float extra = board[-y + middle] % 2 == 0 ? 1f : 0;
                int newX = x;
                if (y == -1) newX = x - y;
                if (y < -1) newX = x - (y - 1) / 2;
                if (y > 1) newX = x - y / 2;
                if (y == 1) newX = x;
                activeBoard.Add(new Vector2(newX, y));
            }
        }
        return activeBoard;
    }
    public IEnumerator SinkAll()
    {
        List<BaseData> sortedList = bases.FindAll(x => x.obj.GetComponent<BaseComponent>().isHide == false).ToList();
        int total = sortedList.Count;
        Debug.Log(total);
        for (int i = 0; i < total; i++)
        {
            System.Random random = new System.Random();
            int index = random.Next(0, sortedList.Count);
            if (sortedList[index].obj.GetComponent<BaseComponent>().isHide) continue;
            sortedList[index].obj.transform.DOLocalMoveY(-3.5f, 1f).SetEase(Ease.Linear);
            sortedList[index].obj.GetComponent<BaseComponent>().CallSplashVFX();
            sortedList.Remove(sortedList[index]);
            yield return new WaitForSeconds(random.Next(0, 5) * 0.05f);
        }
    }

    public List<Vector2> GetSandList(List<int> board)
    {
        List<Vector2> sandListVector2 = new List<Vector2>();
        int middle = board.Count / 2;
        int add = board.Count % 2 == 1 ? 1 : 0;
        for (int y = middle; y > -middle - add; y--)
        {
            // Debug.Log(y +" "+ middle);
            int middleBase = Mathf.FloorToInt(board[-y + middle] / 2f);
            int extraCount = board[-y + middle] % 2 == 0 ? 0 : 1;
            for (int x = -middleBase; x < middleBase + extraCount; x++)
            {
                float extra = board[-y + middle] % 2 == 0 ? 1f : 0;
                int newX = x;
                if (y == -1) newX = x - y;
                if (y < -1) newX = x - (y - 1) / 2;
                if (y > 1) newX = x - y / 2;
                if (y == 1) newX = x;
                if (y == -middle || y == middle || x == -middleBase || x == middleBase + extraCount - 1)
                {
                    sandListVector2.Add(new Vector2(newX, y));
                }
            }
        }
        return sandListVector2;
    }

    public void CreateBoard(List<int> board, List<int> activeBoard)
    {
        List<Vector2> activeBoardList = GetActiveBoard(activeBoard);
        List<Vector2> sandListVector2 = GetSandList(activeBoard);
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
                baseObj.transform.localPosition = new Vector3(y * -1.75f, 0, x * 2f + extra);
                baseObj.transform.localEulerAngles = new Vector3(0, 90, 0);
                int newX = x;
                if (y == -1) newX = x - y;
                if (y < -1) newX = x - (y - 1) / 2;
                if (y > 1) newX = x - y / 2;
                if (y == 1) newX = x;
                baseObj.name = "Base (" + newX + "," + y + ")";
                if (baseObj.GetComponentInChildren<TMP_Text>()) baseObj.GetComponentInChildren<TMP_Text>().text = newX + "," + y;
                baseObj.SetActive(false);
                BaseData newBase = new BaseData();
                newBase.id = newX * middle + y;
                newBase.coordinates = new Vector2(newX, y);
                newBase.obj = baseObj;
                bases.Add(newBase);
                baseObj.GetComponent<BaseComponent>().baseData = newBase;
                baseObj.GetComponent<BaseComponent>().isHide = !activeBoardList.Contains(new Vector2(newX, y));
                if (sandListVector2.Contains(new Vector2(newX, y)))
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

    public List<Vector3Int> SpawnPets(List<int> board, List<LineConfig> lineConfigs)
    {
        List<Vector3Int> petVector = new List<Vector3Int>();
        int middle = board.Count / 2;
        int add = board.Count % 2 == 1 ? 1 : 0;
        for (int y = middle; y > -middle - add; y--)
        {
            int middleBase = Mathf.FloorToInt(board[-y + middle] / 2f);
            int extraCount = board[-y + middle] % 2 == 0 ? 0 : 1;
            for (int x = -middleBase; x < middleBase + extraCount; x++)
            {
                float extra = board[-y + middle] % 2 == 0 ? 1f : 0;
                int newX = x;
                if (y == -1) newX = x - y;
                if (y < -1) newX = x - (y - 1) / 2;
                if (y > 1) newX = x - y / 2;
                if (y == 1) newX = x;
                petVector.Add(new Vector3Int(lineConfigs[Mathf.Abs(-y + middle)].petDirections[x + middleBase], newX, y));
            }
        }

        return petVector;
    }

    IEnumerator CallAnimationSpawn()
    {
        int currentX = 0;
        List<BaseData> sortedList = bases.OrderBy(x => x.coordinates.x).ToList();
        currentX = (int)sortedList[0].coordinates.x;
        foreach (BaseData item in sortedList)
        {
            if (item.obj.GetComponent<BaseComponent>().isHide)
            {
                item.obj.GetComponent<BaseComponent>().main.SetActive(false);
                item.obj.SetActive(true);
            }
            else
            {
                if (currentX != (int)item.coordinates.x)
                {
                    yield return new WaitForSeconds(0.3f);
                }
                item.obj.SetActive(true);
                item.obj.GetComponent<Animator>().Play("Spawn");
                currentX = (int)item.coordinates.x;
            }
        }
        yield return new WaitForSeconds(0.5f);
        GamePlay.Instance.SpawnPets();
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
        BaseData baseData = bases.FirstOrDefault(x => x.coordinates == des && x.obj.GetComponent<BaseComponent>().isHide == false);
        if (baseData != null)
        {
            if (GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates))
            {
                data = baseData;
                return;
            }
            CalculateDestination(des, direction);
        }
        else
        {
            data = bases.FirstOrDefault(x => x.coordinates == des);
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
