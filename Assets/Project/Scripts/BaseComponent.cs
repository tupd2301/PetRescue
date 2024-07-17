using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    public GameObject spawnPoint;
    
    [SerializeField] private List<GameObject> baseModels = new List<GameObject>();

    void Start()
    {
        
    }
    public void RandomModel()
    {
        baseModels.ForEach(x => x.SetActive(false));
        int index = Random.Range(1, baseModels.Count);
        baseModels[index].SetActive(true);
    }
    public void SetModelSand()
    {
        baseModels.ForEach(x => x.SetActive(false));
        baseModels[0].SetActive(true);
    }

    public void OnBaseClicked()
    {
        GetComponent<Animator>().Play("Bounce");
    }
}
