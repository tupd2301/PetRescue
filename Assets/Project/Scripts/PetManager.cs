using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [SerializeField] private List<Pet> petModels = new List<Pet>();
    [SerializeField] private GameObject prefab;

    public void SpawnPet()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
[System.Serializable]
public class Pet
{
    public int id;
    public GameObject model;
    public Vector3 position;
    public int rotationY;

    public Pet(int id, GameObject model, Vector3 position, int rotationY)
    {
        this.id = id;
        this.model = model;
        this.position = position;
        this.rotationY = rotationY;
    }
}
