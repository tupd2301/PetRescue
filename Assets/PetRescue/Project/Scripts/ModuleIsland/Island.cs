using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Island", menuName = "Island", order = 1)]
public class Island : ScriptableObject
{
    [Tooltip("Island data")]
    public IslandData islandData;
}
