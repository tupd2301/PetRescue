using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArrowSpawner : MonoBehaviour
{
    public CompassDir direction;

    DirectionalCellSpawner TileSpawner;

    void Start()
    {
        TileSpawner = GetComponentInParent<DirectionalCellSpawner>();
    }

    void Update()
    {

    }

    public ILevelCell OnRightClick()
    {
        return TileSpawner.SpawnTile(direction, false);
    }

    public ILevelCell OnLeftClick()
    {
        return TileSpawner.SpawnTile(direction, true);
    }
}
