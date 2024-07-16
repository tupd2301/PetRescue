﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum CellState
{
    eNormal,
    eHover,
    ePositive,
    eNegative,
    eMovement
}

[System.Serializable]
public class GameTeamEvent : UnityEvent<GameTeam>
{ }

[System.Serializable]
public class GridUnitEvent : UnityEvent<GridUnit>
{ }

[System.Serializable]
public struct TeamInfo
{
    public int TeamId;

    public TeamInfo(int InTeamId)
    {
        TeamId = InTeamId;
    }

    public static TeamInfo InvalidTeam()
    {
        return new TeamInfo(-1);
    }

    public bool IsValid()
    {
        return TeamId != -1;
    }

    public override bool Equals(object obj)
    {
        TeamInfo otherTeamInfo = (TeamInfo)obj;
        return otherTeamInfo.TeamId == TeamId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class GameManager : MonoBehaviour
{
    static GameManager sInstance = null;

    [Space(10)]

    [SerializeField]
    ILevelGrid m_LevelGrid;

    [SerializeField]
    GameRules m_GameRules;

    [SerializeField]
    FogOfWar m_FogOfWar;

    [SerializeField]
    [HideInInspector]
    InputHandler m_InputHandler;

    [SerializeField]
    CameraController m_CameraController;

    [Space(10)]

    [SerializeField]
    GameObject m_SelectedHoverObject;

    [Space(10)]
    [Header("Team Data")]

    [SerializeField]
    HumanTeamData m_FriendlyTeamData;

    [SerializeField]
    TeamData m_HostileTeamData;

    [Space(10)]

    [SerializeField]
    WinCondition[] m_WinConditions;

    [SerializeField]
    GameObject[] m_SpawnOnStart;

    [SerializeField]
    GameObject[] m_AddToSpawnedUnits;

    [SerializeField]
    AbilityParticle[] m_DeathParticles;

    [Space(10)]

    [SerializeField]
    public GameTeamEvent OnTeamWon;

    [HideInInspector]
    public GridUnitEvent OnUnitSelected;

    [HideInInspector]
    public GridUnitEvent OnUnitHover = new GridUnitEvent();

    List<GridObject> SpawnedCellObjects = new List<GridObject>();

    Dictionary<GameTeam, List<GridUnit>> m_Teams = new Dictionary<GameTeam, List<GridUnit>>();

    Dictionary<GameTeam, int> m_NumberOfKilledTargets = new Dictionary<GameTeam, int>();
    Dictionary<GameTeam, int> m_NumberOfKilledEntities = new Dictionary<GameTeam, int>();

    List<ILevelCell> CurrentHoverCells = new List<ILevelCell>();

    UnityEvent OnFinishedPerformedActions = new UnityEvent();

    ILevelCell m_CurrentHoverCell;

    int m_NumActionsBeingPerformed;

    bool m_bIsPlaying = false;

    void Awake()
    {
        if (sInstance == null)
        {
            sInstance = this;
        }
    }

    void Start()
    {
        DirectionalCellSpawner[] CellSpawners = (DirectionalCellSpawner[])GameObject.FindObjectsOfType(typeof(DirectionalCellSpawner));
        foreach (DirectionalCellSpawner cellSpawner in CellSpawners)
        {
            Destroy(cellSpawner.gameObject);
        }

        if (!m_LevelGrid)
        {
            Debug.Log("([TurnBasedTools]::GameManager::Start) Missing Grid");
        }

        if (!m_GameRules)
        {
            Debug.Log("([TurnBasedTools]::GameManager::Start) Missing GameRules");
        }

        if (m_WinConditions.Length == 0)
        {
            Debug.Log("([TurnBasedTools]::GameManager::Start) Missing WinConditions");
        }

        if (!m_FriendlyTeamData)
        {
            Debug.Log("([TurnBasedTools]::GameManager::Start) Missing Friendly Team Data");
        }

        if (!m_HostileTeamData)
        {
            Debug.Log("([TurnBasedTools]::GameManager::Start) Missing Hostile Team Data");
        }

        if (m_FriendlyTeamData)
        {
            m_FriendlyTeamData.SetTeam(GameTeam.Friendly);
        }

        if (m_HostileTeamData)
        {
            m_HostileTeamData.SetTeam(GameTeam.Hostile);
        }

        if (m_GameRules)
        {
            m_InputHandler.OnNumPressed.AddListener(m_GameRules.HandleNumPressed);
            OnTeamWon.AddListener(m_GameRules.HandleTeamWon);
        }

        Initalize();

        foreach (GameObject SpawnObj in m_SpawnOnStart)
        {
            if (SpawnObj)
            {
                Instantiate(SpawnObj);
            }
        }
    }

    void Update()
    {
        m_InputHandler.Update();

        if (m_GameRules)
        {
            m_GameRules.Update();
        }
    }

    void Initalize()
    {
        SetupGrid();
        if (m_GameRules)
        {
            m_GameRules.InitalizeRules();
        }

        if (m_FogOfWar)
        {
            m_FogOfWar.SpawnFogObjects();
        }
    }

    void SetupGrid()
    {
        if (m_LevelGrid)
        {
            m_LevelGrid.SetupAllCellAdjacencies();
            m_LevelGrid.OnCellInteraction.AddListener(HandleInteraction);
        }
        SetupMaterials();
    }

    void SetupMaterials()
    {
        if (m_LevelGrid)
        {
            ILevelCell TestCell = null;

            List<ILevelCell> LevelCells = m_LevelGrid.GetAllCells();
            if (LevelCells.Count > 0)
            {
                TestCell = LevelCells[0];
            }
            if (TestCell != null)
            {
                foreach (ILevelCell currCell in LevelCells)
                {
                    CellState cellState = currCell.GetNormalState();
                    currCell.SetCellState(cellState);
                    currCell.SetMaterial(cellState);
                }
            }
        }
    }

    #region Public Statics

    public static GameManager Get()
    {
        return sInstance;
    }

    public static ILevelGrid GetGrid()
    {
        return sInstance.m_LevelGrid;
    }

    public static GameRules GetRules()
    {
        return sInstance.m_GameRules;
    }
    
    public static FogOfWar GetFogOfWar()
    {
        return sInstance.m_FogOfWar;
    }

    public static CameraController GetCameraController()
    {
        return sInstance.m_CameraController;
    }

    public static GameObject GetSelectedHoverPrefab()
    {
        return sInstance.m_SelectedHoverObject;
    }

    public static List<WinCondition> GetWinConditions()
    {
        return new List<WinCondition>(sInstance.m_WinConditions);
    }

    public static GameTeam GetTeamAffinity(GameTeam InTeam1, GameTeam InTeam2)
    {
        if (InTeam1 == InTeam2)
        {
            return GameTeam.Friendly;
        }

        return GameTeam.Hostile;
    }

    public static Dictionary<GameTeam, List<GridUnit>> GetTeamsMap()
    {
        return sInstance.m_Teams;
    }

    public static List<GameTeam> GetTeamList()
    {
        List<GameTeam> outTeams = new List<GameTeam>();

        foreach (var item in GetTeamsMap().Keys)
        {
            outTeams.Add(item);
        }

        return outTeams;
    }

    //Checks if a player is moving, attacking or basically anything that should prevent input.
    public static bool IsActionBeingPerformed()
    {
        return (sInstance.m_NumActionsBeingPerformed > 0);
    }

    public static void AddActionBeingPerformed()
    {
        ++sInstance.m_NumActionsBeingPerformed;
    }

    public static void RemoveActionBeingPerformed()
    {
        --sInstance.m_NumActionsBeingPerformed;

        if (sInstance.m_NumActionsBeingPerformed == 0)
        {
            sInstance.OnFinishedPerformedActions.Invoke();
        }
    }

    public static void BindToOnFinishedPerformedActions(UnityAction InAction)
    {
        sInstance.OnFinishedPerformedActions.AddListener(InAction);
    }

    public static void UnBindFromOnFinishedPerformedActions(UnityAction InAction)
    {
        sInstance.OnFinishedPerformedActions.RemoveListener(InAction);
    }

    public static int GetNumTargetsKilled(GameTeam InTeam)
    {
        if (sInstance.m_NumberOfKilledTargets.ContainsKey(InTeam))
        {
            return sInstance.m_NumberOfKilledTargets[InTeam];
        }

        return 0;
    }

    public static int NumUnitsKilled(GameTeam InTeam)
    {
        if (sInstance.m_NumberOfKilledEntities.ContainsKey(InTeam))
        {
            return sInstance.m_NumberOfKilledEntities[InTeam];
        }

        return 0;
    }

    public static bool AreAllUnitsOnTeamDead(GameTeam InTeam)
    {
        return NumUnitsKilled(InTeam) == sInstance.m_Teams[InTeam].Count;
    }

    public static bool CanFinishTurn()
    {
        bool bActionBeingPerformed = IsActionBeingPerformed();
        bool bIsTeamHuman = true;

        GameRules gameRules = GetRules();
        if (gameRules)
        {
            bIsTeamHuman = IsTeamHuman(gameRules.GetCurrentTeam());
        }

        return (!bActionBeingPerformed && bIsTeamHuman);
    }

    public static int GetNumOfTargets(GameTeam InTeam)
    {
        return GetTeamTargets(InTeam).Count;
    }

    public static List<GridUnit> GetTeamTargets(GameTeam InTeam)
    {
        List<GridUnit> units = new List<GridUnit>();

        if(sInstance.m_Teams.ContainsKey(InTeam))
        {
            foreach (GridUnit unit in sInstance.m_Teams[InTeam])
            {
                if (unit.IsTarget())
                {
                    units.Add(unit);
                }
            }
        }

        return units;
    }

    public static bool KilledAllTargets(GameTeam InTeam)
    {
        return GetNumTargetsKilled(InTeam) == GetNumOfTargets(InTeam);
    }

    public static HumanTeamData GetFriendlyTeamData()
    {
        return sInstance.m_FriendlyTeamData;
    }

    public static TeamData GetHostileTeamData()
    {
        return sInstance.m_HostileTeamData;
    }

    public static TeamData GetDataForTeam(GameTeam InTeam)
    {
        switch (InTeam)
        {
            case GameTeam.Friendly:
                return GetFriendlyTeamData();
            case GameTeam.Hostile:
                return GetHostileTeamData();
        }

        Debug.Log("([TurnBasedTools]::GameManager::GetDataForTeam) Trying to get TeamData for invalid team: " + InTeam.ToString());
        return new TeamData();
    }

    public static T GetDataForTeam<T>(GameTeam InTeam) where T : TeamData
    {
        switch (InTeam)
        {
            case GameTeam.Friendly:
                return GetFriendlyTeamData() as T;
            case GameTeam.Hostile:
                return GetHostileTeamData() as T;
        }

        return null;
    }

    public static GridObject SpawnObjectOnCell(GameObject InObject, ILevelCell InCell, Vector3 InOffset = default(Vector3))
    {
        if ( InCell && InObject )
        {
            GridObject SpawnedGridObject = Instantiate(InObject).AddComponent<GridObject>();

            float ObjHeight = SpawnedGridObject.GetBounds().y;
            float CellHeight = InCell.GetRenderer().bounds.size.y;

            Vector3 HeightOffset = new Vector3(0.0f, (CellHeight * 0.5f) + (ObjHeight * 0.5f), 0.0f);

            SpawnedGridObject.gameObject.transform.position = InCell.gameObject.transform.position + HeightOffset + InOffset;

            SpawnedGridObject.Initalize();
            SpawnedGridObject.SetGrid( GetGrid() );
            SpawnedGridObject.SetCurrentCell( InCell );
            SpawnedGridObject.PostInitalize();

            return SpawnedGridObject;
        }

        return null;
    }

    public static GridUnit SpawnUnit(UnitData InUnitData, GameTeam InTeam, Vector2 InIndex, CompassDir InStartDirection = CompassDir.S)
    {
        ILevelCell cell = sInstance.m_LevelGrid[InIndex];

        if (InTeam == GameTeam.Friendly)
        {
            cell.SetVisible(true);
        }

        GridUnit SpawnedGridUnit;

        if (InUnitData.m_UnitClass == "")
        {
            SpawnedGridUnit = Instantiate(InUnitData.m_Model).AddComponent<GridUnit>();
        }
        else
        {
            System.Type classType = GameUtils.FindType(InUnitData.m_UnitClass);
            SpawnedGridUnit = Instantiate(InUnitData.m_Model).AddComponent(classType) as GridUnit;
        }

        SpawnedGridUnit.Initalize();
        SpawnedGridUnit.SetUnitData(InUnitData);
        SpawnedGridUnit.SetTeam(InTeam);
        SpawnedGridUnit.SetGrid(sInstance.m_LevelGrid);
        SpawnedGridUnit.SetCurrentCell(cell);
        SpawnedGridUnit.AlignToGrid();
        SpawnedGridUnit.PostInitalize();

        ILevelCell DirCell = SpawnedGridUnit.GetCell().GetAdjacentCell(InStartDirection);
        if (DirCell)
        {
            SpawnedGridUnit.LookAtCell(DirCell);
        }

        foreach (GameObject obj in sInstance.m_AddToSpawnedUnits)
        {
            if (obj)
            {
                Instantiate(obj, SpawnedGridUnit.gameObject.transform);
            }
        }

        sInstance.SpawnedCellObjects.Add(SpawnedGridUnit);

        if (!sInstance.m_Teams.ContainsKey(InTeam))
        {
            sInstance.m_Teams.Add(InTeam, new List<GridUnit>());
        }

        sInstance.m_Teams[InTeam].Add(SpawnedGridUnit);

        if (InTeam == GameTeam.Friendly)
        {
            if (sInstance.m_FogOfWar)
            {
                sInstance.m_FogOfWar.CheckPoint(SpawnedGridUnit.GetCell());
            }
        }

        return SpawnedGridUnit;
    }

    void SpawnDeathParticlesForUnit(GridUnit InUnit)
    {
        foreach (AbilityParticle particle in m_DeathParticles)
        {
            if (particle)
            {
                AbilityParticle spawnedParticle = Instantiate(particle.gameObject, InUnit.gameObject.transform.position, particle.gameObject.transform.rotation).GetComponent<AbilityParticle>();
                if (spawnedParticle)
                {
                    spawnedParticle.Setup(null, null, null);
                }
            }
        }
    }

    public static List<Renderer> GetAllRenderersOfObject(GameObject InObject)
    {
        List<Renderer> Renderers = new List<Renderer>();

        Renderers.Add(InObject.GetComponent<Renderer>());
        Renderers.AddRange(InObject.GetComponentsInChildren<Renderer>());

        return Renderers;
    }

    public static Vector3 GetBoundsOfObject(GameObject InObject)
    {
        if (InObject)
        {
            Vector3 bounds = new Vector3();

            List<Renderer> Renderers = GetAllRenderersOfObject(InObject);
            foreach (Renderer currRenderer in Renderers)
            {
                if (currRenderer)
                {
                    Vector3 rendererBound = currRenderer.bounds.size;
                    if (rendererBound.x > bounds.x)
                    {
                        bounds.x = rendererBound.x;
                    }
                    if (rendererBound.y > bounds.y)
                    {
                        bounds.y = rendererBound.y;
                    }
                    if (rendererBound.z > bounds.z)
                    {
                        bounds.z = rendererBound.z;
                    }
                }
            }

            return bounds;
        }

        return new Vector3();
    }

    public static List<GridUnit> GetUnitsOnTeam(GameTeam InTeam)
    {
        if (InTeam == GameTeam.None)
        {
            Debug.Log("([TurnBasedTools]::GameManager::GetUnitsOnTeam) Trying to get units for invalid team: " + InTeam.ToString());
        }

        if (sInstance.m_Teams.ContainsKey(InTeam))
        {
            return sInstance.m_Teams[InTeam];
        }

        return new List<GridUnit>();
    }

    public static bool IsTeamHuman(GameTeam InTeam)
    {
        return GetDataForTeam<HumanTeamData>(InTeam) != null;
    }

    public static bool IsTeamAI(GameTeam InTeam)
    {
        return GetDataForTeam<AITeamData>(InTeam) != null;
    }

    public static bool IsUnitOnTeam(GridUnit InUnit, GameTeam InTeam)
    {
        if(sInstance.m_Teams.ContainsKey(InTeam))
        {
            return sInstance.m_Teams[InTeam].Contains(InUnit);
        }
        return false;
    }

    public static bool IsPlaying()
    {
        return sInstance.m_bIsPlaying;
    }

    public static void HandleGameStarted()
    {
        sInstance.m_bIsPlaying = true;
    }

    public static GameTeam GetUnitTeam(GridUnit InUnit)
    {
        if (IsUnitOnTeam(InUnit, GameTeam.Friendly))
        {
            return GameTeam.Friendly;
        }
        if (IsUnitOnTeam(InUnit, GameTeam.Hostile))
        {
            return GameTeam.Hostile;
        }

        return GameTeam.None;
    }

    public static void ResetCellState(ILevelCell InCell)
    {
        SetCellState(InCell, InCell.GetNormalState());
    }

    public static void SetCellState(ILevelCell InCell, CellState InCellState)
    {
        if (sInstance.m_LevelGrid)
        {
            InCell.SetMaterial(InCellState);
            InCell.SetCellState(InCellState);
            if (InCell.IsMouseOver())
            {
                sInstance.BeginHover(InCell);
            }
        }
    }

    public static bool CanCasterEffectTarget(ILevelCell InCaster, ILevelCell InTarget, GameTeam InEffectedTeam, bool bAllowBlocked)
    {
        if (!InCaster || !InTarget)
        {
            return false;
        }

        if ((InCaster.IsBlocked() || InTarget.IsBlocked()) && !bAllowBlocked)
        {
            return false;
        }

        if (InEffectedTeam == GameTeam.None)
        {
            return false;
        }

        if (InCaster.GetCellTeam() != GameTeam.None)
        {
            GameTeam ObjAffinity = GameManager.GetTeamAffinity(InCaster.GetCellTeam(), InTarget.GetCellTeam());
            if (ObjAffinity == GameTeam.Friendly && InEffectedTeam == GameTeam.Hostile)
            {
                return false;
            }
        }

        return true;
    }

    public static void FinishTurn()
    {
        if (CanFinishTurn())
        {
            if (sInstance.m_GameRules)
            {
                sInstance.m_GameRules.EndTurn();
            }

            CheckWinConditions();
        }
    }

    public static void CheckWinConditions()
    {
        if (!sInstance.m_bIsPlaying)
        {
            return;
        }

        Dictionary<GameTeam, int> TeamToWinCount = new Dictionary<GameTeam, int>()
        {
            { GameTeam.Friendly, 0},
            { GameTeam.Hostile, 0}
        };

        int NumWinConditions = sInstance.m_WinConditions.Length;

        foreach (WinCondition currWinCondition in sInstance.m_WinConditions)
        {
            if (currWinCondition)
            {
                if (currWinCondition.m_bCheckWinFirst)
                {
                    if (DidTeamPassCondition(currWinCondition, GameTeam.Friendly))
                    {
                        if (++TeamToWinCount[GameTeam.Friendly] >= NumWinConditions)
                        {
                            TeamWon(GameTeam.Friendly);
                        }
                    }

                    if (DidTeamPassCondition(currWinCondition, GameTeam.Hostile))
                    {
                        if (++TeamToWinCount[GameTeam.Hostile] >= NumWinConditions)
                        {
                            TeamWon(GameTeam.Hostile);
                        }
                    }

                    CheckLost(currWinCondition, GameTeam.Friendly);
                    CheckLost(currWinCondition, GameTeam.Hostile);
                }
                else
                {
                    CheckLost(currWinCondition, GameTeam.Friendly);
                    CheckLost(currWinCondition, GameTeam.Hostile);

                    if (DidTeamPassCondition(currWinCondition, GameTeam.Friendly))
                    {
                        if (++TeamToWinCount[GameTeam.Friendly] >= NumWinConditions)
                        {
                            TeamWon(GameTeam.Friendly);
                        }
                    }

                    if (DidTeamPassCondition(currWinCondition, GameTeam.Hostile))
                    {
                        if (++TeamToWinCount[GameTeam.Hostile] >= NumWinConditions)
                        {
                            TeamWon(GameTeam.Hostile);
                        }
                    }
                }
            }
        }
    }

    public static void HandleUnitDeath(GridUnit InUnit)
    {
        if (!sInstance.m_NumberOfKilledEntities.ContainsKey(InUnit.GetTeam()))
        {
            sInstance.m_NumberOfKilledEntities.Add(InUnit.GetTeam(), 0);
        }

        sInstance.m_NumberOfKilledEntities[InUnit.GetTeam()]++;

        if (InUnit.IsTarget())
        {
            if (!sInstance.m_NumberOfKilledTargets.ContainsKey(InUnit.GetTeam()))
            {
                sInstance.m_NumberOfKilledTargets.Add(InUnit.GetTeam(), 0);
            }

            sInstance.m_NumberOfKilledTargets[InUnit.GetTeam()]++;
        }

        sInstance.SpawnDeathParticlesForUnit(InUnit);

        CheckWinConditions();
    }

    public static void HandleUnitActivated(GridUnit InUnit)
    {

    }

    #endregion

    #region EventStuff

    void BeginHover(ILevelCell InCell)
    {
        m_CurrentHoverCell = InCell;
        UpdateHoverCells();
    }

    public void UpdateHoverCells()
    {
        CleanupHoverCells();

        if (m_CurrentHoverCell)
        {
            GridUnit hoverGrid = m_CurrentHoverCell.GetUnitOnCell();
            if (hoverGrid)
            {
                OnUnitHover.Invoke(hoverGrid);
            }

            CurrentHoverCells.Add(m_CurrentHoverCell);

            GameRules gameRules = GetRules();
            if (gameRules)
            {
                GridUnit selectedUnit = gameRules.GetSelectedUnit();
                if (selectedUnit)
                {
                    UnitState unitState = selectedUnit.GetCurrentState();
                    if (unitState == UnitState.UsingAbility)
                    {
                        CurrentHoverCells.AddRange(selectedUnit.GetAbilityHoverCells(m_CurrentHoverCell));
                    }
                    else if (unitState == UnitState.Moving)
                    {
                        List<ILevelCell> AllowedMovementCells = selectedUnit.GetAllowedMovementCells();

                        if (AllowedMovementCells.Contains(m_CurrentHoverCell))
                        {
                            List<ILevelCell> PathToCursor = selectedUnit.GetPathTo(m_CurrentHoverCell, AllowedMovementCells);

                            foreach (ILevelCell pathCell in PathToCursor)
                            {
                                if (pathCell)
                                {
                                    if (AllowedMovementCells.Contains(pathCell))
                                    {
                                        CurrentHoverCells.Add(pathCell);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (ILevelCell currCell in CurrentHoverCells)
            {
                currCell.SetMaterial(CellState.eHover);
            }

            m_CurrentHoverCell.HandleMouseOver();
        }
    }

    void EndHover(ILevelCell InCell)
    {
        CleanupHoverCells();

        if (InCell)
        {
            InCell.HandleMouseExit();
        }

        m_CurrentHoverCell = null;

        OnUnitHover.Invoke(null);
    }

    void CleanupHoverCells()
    {
        foreach (ILevelCell currCell in CurrentHoverCells)
        {
            if (currCell)
            {
                currCell.SetMaterial(currCell.GetCellState());
            }
        }

        CurrentHoverCells.Clear();
    }

    void HandleCellClicked(ILevelCell InCell)
    {
        if (!InCell)
        {
            return;
        }

        if (!m_GameRules)
        {
            return;
        }

        GridUnit gridUnit = InCell.GetUnitOnCell();
        if (gridUnit)
        {
            GameTeam CurrentTurnTeam = m_GameRules.GetCurrentTeam();
            GameTeam UnitsTeam = gridUnit.GetTeam();

            if (UnitsTeam == CurrentTurnTeam)
            {
                m_GameRules.HandlePlayerSelected(gridUnit);
            }
            else
            {
                if (UnitsTeam == GameTeam.Hostile)
                {
                    m_GameRules.HandleEnemySelected(gridUnit);
                }
            }
        }
        m_GameRules.HandleCellSelected(InCell);
    }

    static bool DidTeamPassCondition(WinCondition InCondition, GameTeam InTeam)
    {
        if (InCondition)
        {
            if (InCondition.CheckTeamWin(InTeam))
            {
                return true;
            }
        }

        return false;
    }

    static void CheckLost(WinCondition InCondition, GameTeam InTeam)
    {
        if (InCondition)
        {
            if (InCondition.CheckTeamLost(InTeam))
            {
                TeamLost(InTeam);
            }
        }
    }

    static void TeamWon(GameTeam InTeam)
    {
        sInstance.OnTeamWon.Invoke(InTeam);
        sInstance.HandleGameComplete();
    }

    static void TeamLost(GameTeam InTeam)
    {
        GameTeam WinningTeam = InTeam == GameTeam.Friendly ? GameTeam.Hostile : GameTeam.Friendly;
        TeamWon(WinningTeam);
    }

    void HandleGameComplete()
    {
        m_CurrentHoverCell = null;
        CleanupHoverCells();
        m_bIsPlaying = false;
    }

    void HandleInteraction(ILevelCell InCell, CellInteractionState InInteractionState)
    {
        GridObject ObjOnCell = InCell.GetObjectOnCell();
        if (ObjOnCell)
        {
            ObjOnCell.HandleInteraction(InInteractionState);
        }

        switch (InInteractionState)
        {
            case CellInteractionState.eBeginHover:
                BeginHover(InCell);
                break;
            case CellInteractionState.eEndHover:
                EndHover(InCell);
                break;
            case CellInteractionState.eLeftClick:
                HandleCellClicked(InCell);
                break;
        }
    }

    #endregion
}