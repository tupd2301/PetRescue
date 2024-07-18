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
    
    public void Init()
    {
        SpawnPet(0, GamePlay.Instance.baseManager.bases.FirstOrDefault(x => x.coordinates == new Vector2(0, 0)), (HexDirection)random.Next(0, 5));
    }
    public void SpawnPet(int id, BaseData baseData, HexDirection direction = HexDirection.Up)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.transform.localPosition = new Vector3(baseData.coordinates.x * 1.75f, 1, baseData.coordinates.y * 2);
        obj.transform.localEulerAngles = new Vector3(0, 0, 0);
        PetModelData petModelData = SpawnModel(obj.transform, direction);
        PetData petData = new PetData(id, direction, petModelData, obj.GetComponent<PetComponent>(), baseData.coordinates);
        obj.GetComponent<PetComponent>().SetData(petData);
        pets.Add(petData);
    }
    public PetModelData SpawnModel(Transform parent, HexDirection direction = HexDirection.Up ,PetModelData petModelData = null)
    {
        if(petModelData == null)
        {
            petModelData = petModelDatas[random.Next(0, petModelDatas.Count)];
        }
        GameObject obj = Instantiate(petModelData.model, parent);
        obj.transform.localPosition = petModelData.position;
        obj.transform.localEulerAngles = new Vector3(0,(int)(direction) * 60 + 30, 0);
        obj.transform.localScale = petModelData.scale;
        petModelData.model = obj;
        return petModelData;
    }
    public bool CheckPetExist(Vector2 coordinates)
    {
        if(pets.FirstOrDefault(x => x.baseCoordinates == coordinates) != null) return true;
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
    Up = 0,
    UpRight = 1,
    DownRight = 2,
    Down = 3,
    DownLeft = 4,
    UpLeft = 5
}

[System.Serializable]
public class PetData
{
    public int id;
    public HexDirection direction;
    public PetModelData petModelData;
    public Vector2 baseCoordinates;
    public PetComponent petComponent;

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

    public PetModelData(int id, GameObject model, Vector3 position, Vector3 scale = default(Vector3))
    {
        this.id = id;
        this.model = model;
        this.position = position;
        this.scale = scale;
    }
}
