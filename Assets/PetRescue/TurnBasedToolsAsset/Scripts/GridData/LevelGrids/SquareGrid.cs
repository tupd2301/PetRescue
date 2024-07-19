﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : ILevelGrid
{
    public bool m_bCanMoveDiagonal = true;

    protected override void SetupAdjacencies(ILevelCell InCell)
    {
        if (InCell)
        {
            var adjacentVects = GetRelativeIndicesMap(InCell);
            foreach (var currAdjVect in adjacentVects)
            {
                Vector2 val = currAdjVect.Value;
                ILevelCell adjContender = this[val];
                if (adjContender)
                {
                    InCell.AddAdjacentCell(currAdjVect.Key, adjContender);
                }
            }
        }
    }

    protected override Vector2 GetIndex(Vector2 InOriginalIndex, CompassDir InDirection)
    {
        Vector2 index = InOriginalIndex + GetOffsetFromDirection(InDirection);
        return index;
    }

    protected override Vector2 GetPosition(Vector2 OriginalIndex, CompassDir dir)
    {
        ILevelCell OriginalCell = this[OriginalIndex];

        Vector3 bounds = m_CellObjCursor.GetComponent<Renderer>().bounds.size;

        float finalY = 0.0f;
        float finalX = 0.0f;

        if (OriginalCell)
        {
            switch (dir)
            {
                case CompassDir.N:
                    finalX = OriginalCell.transform.position.x;
                    finalY = OriginalCell.transform.position.z + bounds.z;
                    break;
                case CompassDir.NE:
                    finalX = OriginalCell.transform.position.x + bounds.x;
                    finalY = OriginalCell.transform.position.z + bounds.z;
                    break;
                case CompassDir.E:
                    finalX = OriginalCell.transform.position.x + bounds.x;
                    finalY = OriginalCell.transform.position.z;
                    break;
                case CompassDir.SE:
                    finalX = OriginalCell.transform.position.x + bounds.x;
                    finalY = OriginalCell.transform.position.z - bounds.z;
                    break;
                case CompassDir.S:
                    finalX = OriginalCell.transform.position.x;
                    finalY = OriginalCell.transform.position.z - bounds.z;
                    break;
                case CompassDir.SW:
                    finalX = OriginalCell.transform.position.x - bounds.x;
                    finalY = OriginalCell.transform.position.z - bounds.z;
                    break;
                case CompassDir.W:
                    finalX = OriginalCell.transform.position.x - bounds.x;
                    finalY = OriginalCell.transform.position.z;
                    break;
                case CompassDir.NW:
                    finalX = OriginalCell.transform.position.x - bounds.x;
                    finalY = OriginalCell.transform.position.z + bounds.z;
                    break;
                default:
                    break;
            }
        }

        return new Vector2(finalX, finalY);
    }

    protected override Vector2 GetOffsetFromDirection(CompassDir dir)
    {
        Vector2 Offset = new Vector2();

        switch (dir)
        {
            case CompassDir.N:
                Offset = new Vector2(0, -1);
                break;
            case CompassDir.NE:
                Offset = new Vector2(1, -1);
                break;
            case CompassDir.E:
                Offset = new Vector2(1, 0);
                break;
            case CompassDir.SE:
                Offset = new Vector2(1, 1);
                break;
            case CompassDir.S:
                Offset = new Vector2(0, 1);
                break;
            case CompassDir.SW:
                Offset = new Vector2(-1, 1);
                break;
            case CompassDir.W:
                Offset = new Vector2(-1, 0);
                break;
            case CompassDir.NW:
                Offset = new Vector2(-1, -1);
                break;
        }

        return Offset;
    }

    protected override Dictionary<CompassDir, Vector2> GetRelativeIndicesMap(ILevelCell InCell)
    {
        Dictionary<CompassDir, Vector2> AdjacentVects = new Dictionary<CompassDir, Vector2>();

        if (InCell)
        {
            Vector2 cellIndex = InCell.GetIndex();

            AdjacentVects.Add(CompassDir.N, cellIndex + new Vector2(0, -1));
            AdjacentVects.Add(CompassDir.E, cellIndex + new Vector2(1, 0));
            AdjacentVects.Add(CompassDir.S, cellIndex + new Vector2(0, 1));
            AdjacentVects.Add(CompassDir.W, cellIndex + new Vector2(-1, 0));

            if(m_bCanMoveDiagonal)
            {
                AdjacentVects.Add(CompassDir.NW, cellIndex + new Vector2(-1, -1));
                AdjacentVects.Add(CompassDir.NE, cellIndex + new Vector2(1, -1));
                AdjacentVects.Add(CompassDir.SE, cellIndex + new Vector2(1, 1));
                AdjacentVects.Add(CompassDir.SW, cellIndex + new Vector2(-1, 1));
            }
        }

        return AdjacentVects;
    }
}
