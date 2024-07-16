using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> petModels = new List<GameObject>();
    [SerializeField] private GameObject prefab;

    public void SpawnPet()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
