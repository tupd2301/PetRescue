using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayIsland : MonoBehaviour
{
    public static GamePlayIsland Instance { get; private set; }
    public IslandManager islandManager;

    public CameraControl cameraControl;

    private void Awake()
    {
        Instance = this;
    }
}
