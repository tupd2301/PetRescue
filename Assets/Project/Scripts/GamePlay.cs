using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public static GamePlay Instance;
    public PetManager petManager;
    public BaseManager baseManager;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        baseManager.Init();
        petManager.Init();
    }
}
