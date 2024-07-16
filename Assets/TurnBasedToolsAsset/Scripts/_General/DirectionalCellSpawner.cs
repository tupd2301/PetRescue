﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DirectionalCellSpawner : MonoBehaviour
{
    public ILevelCell ReferenceCell;

    List<CompassDir> m_AllowedDirections = new List<CompassDir>();

    public UnityEvent OnReferenceCellDestroyed = new UnityEvent();

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetCurrentTile(ILevelCell InCell)
    {
        if ( ReferenceCell )
        {
            ReferenceCell.OnCellDestroyed.RemoveListener( HandleRefCellDestroyed );
        }

        ReferenceCell = InCell;

        if ( ReferenceCell )
        {
            ReferenceCell.OnCellDestroyed.AddListener( HandleRefCellDestroyed );
        }

        UpdateArrows();
    }

    void HandleRefCellDestroyed()
    {
        OnReferenceCellDestroyed.Invoke();
    }

    void UpdateAllowedDirections()
    {
        m_AllowedDirections.Clear();

        if (ReferenceCell)
        {
            if (ReferenceCell.GetGrid() as HexagonGrid)
            {
                CompassDir[] dirs = { CompassDir.E, CompassDir.NE, CompassDir.NW, CompassDir.SE, CompassDir.SW, CompassDir.W };
                m_AllowedDirections.AddRange(dirs);
            }
            else if (ReferenceCell.GetGrid() as SquareGrid)
            {
                CompassDir[] dirs = new CompassDir[]{ CompassDir.N, CompassDir.E, CompassDir.S, CompassDir.W };
                m_AllowedDirections.AddRange(dirs);
            }
        }
    }

    void UpdateArrows()
    {
        UpdateAllowedDirections();

        if (ReferenceCell)
        {
            ArrowSpawner[] ArrowSpawners = GetComponentsInChildren<ArrowSpawner>();
            foreach (ArrowSpawner arrow in ArrowSpawners)
            {
                if (arrow)
                {
                    if (ReferenceCell.HasAdjacentCell(arrow.direction) || !m_AllowedDirections.Contains(arrow.direction))
                    {
                        DestroyImmediate(arrow.gameObject);
                    }
                }
            }
        }
    }

    public ILevelCell SpawnTile(CompassDir Direction, bool bSelectNewTile)
    {
        if(ReferenceCell)
        {
            ILevelCell generatedCell = ReferenceCell.AddCellTo(Direction);
            UpdateArrows();

            return generatedCell;
        }

        return null;
    }
}
