using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [SerializeField] private List<PetModelData> petModelDatas = new List<PetModelData>();
    [SerializeField] private List<PetData> pets = new List<PetData>();
    [SerializeField] private GameObject prefab;
    System.Random random = new System.Random();

    public void Reset()
    {
        for (int i = 0; i < pets.Count; i++)
        {
            Destroy(pets[i].petComponent.gameObject);
        }
        pets = new List<PetData>();
    }
    public int GetPetCount()
    {
        return pets.FindAll(x => x.petComponent.isHide == false).Count;
    }
    public void SpawnPets(List<Vector3Int> list)
    {
        Reset();
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].x == -1)
            {
                GameObject gameObj = GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == new Vector2(list[i].y, list[i].z)).obj;
                gameObj.SetActive(false);
                gameObj.GetComponent<BaseComponent>().isHide = true;
                continue;
            }
            if(list[i].x == 0) continue;
            Debug.Log(i + " " + list[i].x + " " + list[i].y + " " + list[i].z);
            SpawnPet(i, GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == new Vector2(list[i].y, list[i].z)), (HexDirection)list[i].x);
        }
    }
    public void SpawnPet(int id, BaseData baseData, HexDirection direction = HexDirection.Up)
    {
        GameObject obj = Instantiate(prefab, transform);
        Vector3 basePos = GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == baseData.coordinates).obj.transform.position;
        obj.transform.position = new Vector3(basePos.x, 1, basePos.z);
        obj.transform.localEulerAngles = new Vector3(0, 0, 0);
        PetModelData petModelData = SpawnModel(obj.transform, direction);
        PetData petData = new PetData(id, direction, petModelData, obj.GetComponent<PetComponent>(), baseData.coordinates);
        obj.GetComponent<PetComponent>().SetData(petData);
        pets.Add(petData);
    }
    public PetModelData SpawnModel(Transform parent, HexDirection direction = HexDirection.Up ,PetModelData petModelData = null)
    {
        PetModelData modelData = new PetModelData();
        if(petModelData == null)
        {
            petModelData = petModelDatas[random.Next(0, petModelDatas.Count)];
        }
        GameObject modelObj = Instantiate(petModelData.model, parent);
        modelObj.transform.localPosition = petModelData.position;
        modelObj.transform.localEulerAngles = new Vector3(0,(int)(direction) * 60 - 30, 0);
        modelObj.transform.localScale = petModelData.scale;
        modelData.id = petModelData.id;
        modelData.position = petModelData.position;
        modelData.scale = petModelData.scale;
        modelData.model = modelObj;
        return modelData;
    }
    public bool CheckPetExist(Vector2 coordinates)
    {
        if(pets.FirstOrDefault(x => x.baseCoordinates == coordinates && x.petComponent.isHide == false) != null) return true;
        return false;
    }
    public PetComponent GetPetByCoordinates(Vector2 coordinates)
    {
        return pets.FirstOrDefault(x => x.baseCoordinates == coordinates).petComponent;
    }
}

[System.Serializable]
public enum HexDirection
{
    Up = 1,
    UpRight = 2,
    DownRight = 3,
    Down = 4,
    DownLeft = 5,
    UpLeft = 6
}

[System.Serializable]
public class PetData
{
    public int id;
    public HexDirection direction;
    public PetModelData petModelData;
    public Vector2 baseCoordinates;
    public PetComponent petComponent;
    public PetData()
    {

    }

    public PetData(int id, HexDirection direction, PetModelData petModelData, PetComponent petComponent, Vector2 baseCoordinates = default(Vector2))
    {
        this.id = id;
        this.direction = direction;
        this.petModelData = petModelData;
        this.petComponent = petComponent;
        this.baseCoordinates = baseCoordinates;
    }

}

[System.Serializable]
public class PetModelData
{
    public int id;
    public GameObject model;
    public Vector3 position;
    public Vector3 scale = Vector3.one;  

    public PetModelData()
    {
        
    }

    public PetModelData(int id, GameObject model, Vector3 position, Vector3 scale = default(Vector3))
    {
        this.id = id;
        this.model = model;
        this.position = position;
        this.scale = scale;
    }
}
