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
    public List<Vector2> activeBoardList = new List<Vector2>();

    public bool isShowCoordinate
    {
        get
        {
            return isShowCoordinate;
        }
        set
        {
            isShowCoordinate = value;
            foreach (var item in bases)
            {
                if (item.obj.GetComponent<BaseComponent>().isHide == false)
                    item.obj.GetComponent<BaseComponent>().ShowCoordinate(isShowCoordinate);
            }
        }
    }

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
        if (board[middle] % 2 == 0)
        {
            Vector3 pos = GamePlay.Instance.cameraParent.transform.position;
            GamePlay.Instance.cameraParent.transform.position = new Vector3(1, pos.y, pos.z);
        }
        else
        {
            Vector3 pos = GamePlay.Instance.cameraParent.transform.position;
            GamePlay.Instance.cameraParent.transform.position = new Vector3(0, pos.y, pos.z);
        }
        for (int y = middle; y > -middle - add; y--)
        {
            int middleBase = Mathf.FloorToInt(board[-y + middle] / 2f);
            int extraCount = board[-y + middle] % 2 == 0 ? 0 : 1;
            for (int x = -middleBase; x < middleBase + extraCount; x++)
            {
                float extra = board[middle] % 2 == 0 && board[-y + middle] % 2 == 0 ? 1f : 0;
                int newX = x;
                if (y == -1) newX = x - y;
                if (y < -1) newX = x - (y - 1) / 2;
                if (y > 1) newX = x - y / 2;
                if (y == 1) newX = x;
                activeBoard.Add(new Vector2(newX + extra, y));
            }
        }
        return activeBoard;
    }
    public IEnumerator SinkBases(List<BaseData> baseDatas, float timeScale = 1f)
    {
        List<BaseData> sortedList = baseDatas.FindAll(x => x.obj.GetComponent<BaseComponent>().isHide == false).ToList();
        int total = sortedList.Count;
        Debug.Log(total);
        for (int i = 0; i < total; i++)
        {
            System.Random random = new System.Random();
            int index = random.Next(0, sortedList.Count);
            if (sortedList[index].obj.GetComponent<BaseComponent>().isHide) continue;
            sortedList[index].obj.transform.DOLocalMoveY(-3.5f, 1f * timeScale).SetEase(Ease.Linear);
            sortedList[index].obj.GetComponent<BaseComponent>().isHide = true;
            sortedList.Remove(sortedList[index]);
            yield return new WaitForSeconds(random.Next(0, 5) * 0.05f * timeScale);
        }
    }
    public IEnumerator SinkAll()
    {
        List<BaseData> sortedList = bases.FindAll(x => x.obj.GetComponent<BaseComponent>().isHide == false).ToList();
        int total = sortedList.Count;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < total; i++)
        {
            System.Random random = new System.Random();
            int index = random.Next(0, sortedList.Count);
            if (sortedList[index].obj.GetComponent<BaseComponent>().isHide) continue;
            sortedList[index].obj.transform.DOLocalMoveY(-3.5f, 1f).SetEase(Ease.Linear);
            // sortedList[index].obj.GetComponent<BaseComponent>().CallSplashVFX();
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
        activeBoardList = GetActiveBoard(activeBoard);
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
                baseObj.GetComponent<BaseComponent>().SetCoordinateText(newX, y);
                baseObj.SetActive(false);
                BaseData newBase = new BaseData();
                newBase.id = newX * middle + y;
                newBase.coordinates = new Vector2(newX, y);
                newBase.obj = baseObj;
                bases.Add(newBase);
                baseObj.GetComponent<BaseComponent>().baseData = newBase;
                baseObj.GetComponent<BaseComponent>().isHide = !activeBoardList.Contains(new Vector2(newX, y));
                if (baseObj.GetComponent<BaseComponent>().isHide)
                    baseObj.GetComponent<BaseComponent>().type = BaseType.Hide;
                else
                {
                    baseObj.GetComponent<BaseComponent>().type = BaseType.Normal;

                }
                if (sandListVector2.Contains(new Vector2(newX, y)))
                {
                    baseObj.GetComponent<BaseComponent>().SetModelSand();
                    sandList.Add(newBase);
                }
            }
        }
        StartCoroutine(CallAnimationSpawn());
    }

    private void SpawnSpecialBases()
    {
        List<ValueData> petVectors = SpawnByValues(GamePlay.Instance.currentLevelData);
        foreach (var item in petVectors)
        {
            GameObject gameObj = GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == item.coordinates).obj;
            List<int> list = GamePlay.Instance.GetValues(item.trueCoordinates);
            if (list.Count > 1)
            {
                switch (list[1])
                {
                    case 12:
                        SpawnBase(gameObj, 12, item);
                        break;
                    case 11:
                        SpawnBase(gameObj, list[1], item);
                        break;
                    default: break;
                }
            }
            else if (list.Count == 1) SpawnBase(gameObj, list[0], item);
        }
    }
    private void SpawnBase(GameObject obj, int id, ValueData valueData)
    {
        GameObject gameObj = obj;
        switch (id)
        {
            case -1:
                gameObj.SetActive(false);
                gameObj.GetComponent<BaseComponent>().isHide = true;
                break;
            case 0:
                break;
            case 7:
                gameObj.GetComponent<BaseComponent>().type = BaseType.SwapUpDown;
                gameObj.GetComponent<BaseComponent>().SetModel("swap7");
                break;
            case 8:
                gameObj.GetComponent<BaseComponent>().type = BaseType.SwapLeftDown;
                gameObj.GetComponent<BaseComponent>().SetModel("swap8");
                break;
            case 9:
                gameObj.GetComponent<BaseComponent>().type = BaseType.SwapLeftUp;
                gameObj.GetComponent<BaseComponent>().SetModel("swap9");
                break;
            case 10:
                gameObj.GetComponent<BaseComponent>().type = BaseType.Stop;
                gameObj.GetComponent<BaseComponent>().SetModel("stop");
                break;
            case 11:
                gameObj.GetComponent<BaseComponent>().type = BaseType.Lock;
                gameObj.GetComponent<BaseComponent>().SetModel("lock");
                SpecialTileLock lockTile = gameObj.AddComponent<SpecialTileLock>();
                lockTile.count = GamePlay.Instance.GetValues(valueData.trueCoordinates)[2];
                break;
            case 12:
                gameObj.GetComponent<BaseComponent>().type = BaseType.Boom;
                gameObj.GetComponent<BaseComponent>().SetModel("boom");
                SpecialTileBoom boomTile = gameObj.AddComponent<SpecialTileBoom>();
                break;
            default: break;
        }
    }

    public List<ValueData> SpawnByValues(LevelData levelData)
    {
        List<ValueData> petValues = new List<ValueData>();
        int middle = levelData.boardDesign.Count / 2;
        int add = levelData.boardDesign.Count % 2 == 1 ? 1 : 0;
        for (int y = middle; y > -middle - add; y--)
        {
            int middleBase = Mathf.FloorToInt(levelData.boardDesign[-y + middle] / 2f);
            int extraCount = levelData.boardDesign[-y + middle] % 2 == 0 ? 0 : 1;
            for (int x = -middleBase; x < middleBase + extraCount; x++)
            {
                int extra = levelData.boardDesign[middle] % 2 == 0 && levelData.boardDesign[-y + middle] % 2 == 0 ? 1 : 0;
                int newX = x;
                if (y == -1) newX = x - y;
                if (y < -1) newX = x - (y - 1) / 2;
                if (y > 1) newX = x - y / 2;
                if (y == 1) newX = x;
                ValueData valueData = new ValueData();
                valueData.coordinates = new Vector2Int(newX + extra, y);
                valueData.id = levelData.lineConfigs[Mathf.Abs(-y + middle)].values[x + middleBase][0];
                valueData.trueCoordinates = new Vector2Int(x + middleBase, Mathf.Abs(-y + middle));
                petValues.Add(valueData);
            }
        }

        return petValues;
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
                item.obj.GetComponent<BaseComponent>().RandomGrassModel();
                item.obj.GetComponent<Animator>().Play("Spawn");
                currentX = (int)item.coordinates.x;
            }
        }
        SpawnSpecialBases();
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
            if (GamePlay.Instance.petManager.CheckPetExist(baseData.coordinates)
                || baseData.obj.GetComponent<BaseComponent>().type == BaseType.Stop
                || baseData.obj.GetComponent<BaseComponent>().type == BaseType.Boom
                || baseData.obj.GetComponent<BaseComponent>().type == BaseType.SwapUpDown
                || baseData.obj.GetComponent<BaseComponent>().type == BaseType.SwapLeftUp
                || baseData.obj.GetComponent<BaseComponent>().type == BaseType.SwapLeftDown)
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

    public bool Swap(BaseData baseData)
    {
        Vector2 portal1Coordinates = new Vector2();
        Vector2 portal2Coordinates = new Vector2();
        switch (baseData.obj.GetComponent<BaseComponent>().type)
        {
            case BaseType.SwapUpDown:
                portal1Coordinates = new Vector2(baseData.coordinates.x, baseData.coordinates.y + 1);
                portal2Coordinates = new Vector2(baseData.coordinates.x, baseData.coordinates.y - 1);
                break;
            case BaseType.SwapLeftUp:
                portal1Coordinates = new Vector2(baseData.coordinates.x - 1, baseData.coordinates.y + 1);
                portal2Coordinates = new Vector2(baseData.coordinates.x + 1, baseData.coordinates.y - 1);
                break;
            case BaseType.SwapLeftDown:
                portal1Coordinates = new Vector2(baseData.coordinates.x - 1, baseData.coordinates.y);
                portal2Coordinates = new Vector2(baseData.coordinates.x + 1, baseData.coordinates.y);
                break;
            default:
                return false;
        }
        PetData pet1 = GamePlay.Instance.petManager.GetPetByCoordinates(portal1Coordinates);
        PetData pet2 = GamePlay.Instance.petManager.GetPetByCoordinates(portal2Coordinates);
        if ((pet1 != null && pet1.petComponent.isBusy) || (pet2 != null && pet2.petComponent.isBusy)) return false;
        if (pet1 != null)
        {
            pet1.baseCoordinates = portal2Coordinates;
            Vector3 pos = bases.FirstOrDefault(x => x.coordinates == pet1.baseCoordinates).obj.transform.position;
            pet1.petComponent.transform.position = new Vector3(pos.x, 1.2f, pos.z);
        }
        if (pet2 != null)
        {
            pet2.baseCoordinates = portal1Coordinates;
            Vector3 pos = bases.FirstOrDefault(x => x.coordinates == pet2.baseCoordinates).obj.transform.position;
            pet2.petComponent.transform.position = new Vector3(pos.x, 1.2f, pos.z);
        }
        return pet1 != null && pet2 != null;
    }


    public Dictionary<int, BaseData> GetBaseDestination(PetData petData)
    {
        return CalculateDestinationForPet(petData.baseCoordinates, petData.direction);
    }

    public List<BaseData> GetBaseNearByCoordinates(Vector2 coordinates)
    {
        List<BaseData> list = new List<BaseData>();
        list.Add(bases.FirstOrDefault(x => x.coordinates == coordinates + new Vector2(0, 1)));
        list.Add(bases.FirstOrDefault(x => x.coordinates == coordinates + new Vector2(0, -1)));
        list.Add(bases.FirstOrDefault(x => x.coordinates == coordinates + new Vector2(1, -1)));
        list.Add(bases.FirstOrDefault(x => x.coordinates == coordinates + new Vector2(-1, 1)));
        list.Add(bases.FirstOrDefault(x => x.coordinates == coordinates + new Vector2(1, 0)));
        list.Add(bases.FirstOrDefault(x => x.coordinates == coordinates + new Vector2(-1, 0)));

        return list;
    }
}
[System.Serializable]
public class BaseData
{
    public int id;
    public Vector2 coordinates;

    public GameObject obj;
}
[System.Serializable]
public class ValueData
{
    public int id;
    public Vector2Int trueCoordinates;
    public Vector2Int coordinates;
}
