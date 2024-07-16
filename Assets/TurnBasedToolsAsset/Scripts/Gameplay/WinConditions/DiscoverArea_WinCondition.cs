using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiscoverArea", menuName = "TurnBasedTools/WinCondition/Create DiscoverArea", order = 1)]
public class DiscoverArea_WinCondition : WinCondition
{
    protected override bool DidTeamWin(GameTeam InTeam)
    {
        List<ILevelCell> allCells = GameManager.GetGrid().GetAllCells();
        foreach (ILevelCell cell in allCells)
        {
            if (cell)
            {
                if( !cell.IsVisible() )
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override string GetConditionStateString()
    {
        int CurrentFogCells = 0;

        FogOfWar fogOfWar = GameManager.GetFogOfWar();
        if(fogOfWar)
        {
            CurrentFogCells = GameManager.GetFogOfWar().NumFogCells();
        }

        int TotalCells = GameManager.GetGrid().GetAllCells().Count;

        return "(" + (TotalCells - CurrentFogCells) + "/" + TotalCells + ")";
    }
}
