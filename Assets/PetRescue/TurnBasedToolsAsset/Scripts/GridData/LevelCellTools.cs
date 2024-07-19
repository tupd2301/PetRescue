using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ILevelCell))]
public class LevelCellTools : MonoBehaviour
{
    public ILevelCell GetLevelCell()
    {
        return gameObject.GetComponent<ILevelCell>();
    }
}
