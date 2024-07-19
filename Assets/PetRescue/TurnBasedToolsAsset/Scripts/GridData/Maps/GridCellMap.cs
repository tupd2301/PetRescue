﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PositionToLevelCellPair
{
    [SerializeField]
    public Vector2 _Key;

    [SerializeField]
    public ILevelCell _Value;

    public PositionToLevelCellPair(Vector2 InKey, ILevelCell InValue) : this()
    {
        this._Key = InKey;
        this._Value = InValue;
    }
}

[System.Serializable]
public class GridCellMap : System.Object
{
    [SerializeField]
    public List<PositionToLevelCellPair> Pairs;

    public GridCellMap()
    {
        Pairs = new List<PositionToLevelCellPair>();
    }

    public PositionToLevelCellPair Add(Vector2 InKey, ILevelCell InValue)
    {
        foreach (PositionToLevelCellPair item in Pairs)
        {
            if (item._Key == InKey)
            {
                return item;
            }
        }

        Pairs.Add(new PositionToLevelCellPair(InKey, InValue));
        return Pairs[Pairs.Count - 1];
    }

    public void Remove(Vector2 InIndex)
    {
        foreach (var item in Pairs)
        {
            if(item._Key == InIndex)
            {
                Pairs.Remove(item);
                break;
            }
        }
    }

    public bool ContainsKey(Vector2 InKey)
    {
        foreach (var item in Pairs)
        {
            if (item._Key == InKey)
            {
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        Pairs.Clear();
    }

    public ILevelCell this[Vector2 InKey]
    {
        get
        {
            foreach (var item in Pairs)
            {
                if (item._Key == InKey)
                {
                    return item._Value;
                }
            }

            return null;
        }
    }


}
