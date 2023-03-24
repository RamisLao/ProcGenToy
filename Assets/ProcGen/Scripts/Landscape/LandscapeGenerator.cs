using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// This struct is used to store data about neighbor occupancy
public struct OccupiedNeighborCells
{
    public bool Forward;
    public bool Right;
    public bool Back;
    public bool Left;

    public int Count()
    {
        int count = 0;
        if (Forward) count++;
        if (Right) count++;
        if (Back) count++;
        if (Left) count++;

        return count;
    }
}

/// <summary>
/// This class uses the Floodfill algorithms to generate a procedurally generated landscape
/// </summary>
public class LandscapeGenerator : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] private SettingsLandscapeGeneration _settingsLandscapeGeneration;

    [Title("Base States")]
    [SerializeField] private CellStateData _wallState;
    [SerializeField] private CellStateData _defaultState;

    [Title("Edge States")]
    [SerializeField] private CellStateData _oneSidedEdge;
    [SerializeField] private CellStateData _twoSidedEdge;

    [Title("Variables")]
    [SerializeField] private VariableGrid _variableGrid;

    [Title("Listening on")]
    [SerializeField] private EventChannelVoid _eventFillHoles;
    [SerializeField] private EventChannelVoid _eventRunFloodFill;
    [SerializeField] private EventChannelVoid _eventDeleteSmallIslands;
    [SerializeField] private EventChannelVoid _eventAddEdgesToIslands;

    private int[,] _floorRegions;
    private Dictionary<int, int> _floorRegionsSizes;
    private int[,] _islandRegions;
    private Dictionary<int, int> _islandRegionsSizes;

    private void Awake()
    {
        _eventFillHoles.OnEventRaised += FillFloorStatesWith3AdjacentWalls;
        _eventDeleteSmallIslands.OnEventRaised += DeleteSmallIslands;
        _eventRunFloodFill.OnEventRaised += ApplyFloodFill;
        _eventAddEdgesToIslands.OnEventRaised += AddEdgesToIslands;
    }
    
    // Floor states with 3 adjacent wall states are very hard to manage, so we fill them instead
    private void FillFloorStatesWith3AdjacentWalls()
    {
        for (int x = 0; x < _variableGrid.Value.Length; x++)
        {
            for (int y = 0; y < _variableGrid.Value[0].Length; y++)
            {
                if (IsStateWith3AdjacentOtherStates(x, y, _defaultState, _wallState))
                {
                    _variableGrid.Value[x][y].StateData = _wallState;
                }
            }
        }
    }

    // Get all islands in the grid, measure them, and then delete the islands that are smaller than the MinimumIslandSize
    private void DeleteSmallIslands()
    {
        _islandRegions = FloodFill.FindDistinctRegions(_variableGrid.Value, _defaultState);
        _islandRegionsSizes = FloodFill.GetSizeOfRegions(_islandRegions);
        for (int x = 0; x < _islandRegions.GetLength(0); x++)
        {
            for (int y = 0; y < _islandRegions.GetLength(1); y++)
            {
                int region = _islandRegions[x, y];
                if (region == 0) continue;
                if (_islandRegionsSizes[region] < _settingsLandscapeGeneration.MinimumIslandSize)
                {
                    _variableGrid.Value[x][y].StateData = _defaultState;
                    _islandRegions[x, y] = 0;
                }
            }
        }

        _islandRegionsSizes = FloodFill.GetSizeOfRegions(_islandRegions);
    }

    // Apply the FloodFill algorithm to the grid
    private void ApplyFloodFill()
    {
        // Find all regions that are only floor
        _floorRegions = FloodFill.FindDistinctRegions(_variableGrid.Value, _wallState);
        // Get the sized of all those regions
        _floorRegionsSizes = FloodFill.GetSizeOfRegions(_floorRegions);
        // Get the key of the biggest floor region
        int maxKey = FloodFill.GetBiggestRegionIdx(_floorRegionsSizes);
        // Fill all the remaining regions with walls
        _variableGrid.Value = FloodFill.FillDisconnectedHoles(_variableGrid.Value, ref _floorRegions, maxKey, _wallState);
    }

    // For visual purposes, add edge meshes to all the edges of the islands
    private void AddEdgesToIslands()
    {
        for (int x = 0; x < _variableGrid.Value.Length; x++)
        {
            for (int y = 0; y < _variableGrid.Value[0].Length; y++)
            {
                OccupiedNeighborCells? occupied = IsStateWithAtLeastKAdjacentOtherStates(x, y, _defaultState, _wallState, 1);
                if (occupied == null) continue;
                if (occupied.Value.Count() == 1) AddOneSidedEdge(_variableGrid.Value[x][y], occupied.Value);
                if (occupied.Value.Count() == 2) AddTwoSidedEdge(_variableGrid.Value[x][y], occupied.Value);
            }
        }
    }

    // Return an OccupiedNeighborCells object to know exactly how many neighbor cells have state == otherState
    private OccupiedNeighborCells? IsStateWithAtLeastKAdjacentOtherStates(int x, int y, CellStateData state, CellStateData otherState, int k)
    {
        Cell thisCell = _variableGrid.Value[x][y];
        if (thisCell.StateData.ECellState != state.ECellState) return null;

        OccupiedNeighborCells occupied = new();
        List<Cell> adjacentCells = _variableGrid.GetVonNeumannNeighbors(x, y);
        foreach (Cell c in adjacentCells)
        {
            if (c.StateData.ECellState != otherState.ECellState) continue;
            if (_variableGrid.IsForward(thisCell, c)) occupied.Forward = true;
            else if (_variableGrid.IsRight(thisCell, c)) occupied.Right = true;
            else if (_variableGrid.IsBack(thisCell, c)) occupied.Back = true;
            else if (_variableGrid.IsLeft(thisCell, c)) occupied.Left = true;
        }

        if (occupied.Count() < k) return null;

        return occupied;
    }

    private void AddOneSidedEdge(Cell cell, OccupiedNeighborCells occupied)
    {
        cell.StateData = _oneSidedEdge;
        if (occupied.Left) cell.transform.Rotate(new Vector3(0f, 1f, 0f), 90f, Space.World);
        else if (occupied.Forward) cell.transform.Rotate(new Vector3(0f, 1f, 0f), 180f, Space.World);
        else if (occupied.Right) cell.transform.Rotate(new Vector3(0f, 1f, 0f), 270f, Space.World);
    }

    private void AddTwoSidedEdge(Cell cell, OccupiedNeighborCells occupied)
    {
        cell.StateData = _twoSidedEdge;
        if (occupied.Left && occupied.Forward) cell.transform.Rotate(new Vector3(0f, 1f, 0f), 90f, Space.World);
        else if (occupied.Right && occupied.Forward) cell.transform.Rotate(new Vector3(0f, 1f, 0f), 180f, Space.World);
        else if (occupied.Right && occupied.Back) cell.transform.Rotate(new Vector3(0f, 1f, 0f), 270f, Space.World);
    }

    // Return true if the cell == state and it has 3 adjacent otherState
    private bool IsStateWith3AdjacentOtherStates(int x, int y, CellStateData state, CellStateData otherState)
    {
        if (_variableGrid.Value[x][y].StateData.ECellState != state.ECellState) return false;

        List<Cell> adjacentStates = _variableGrid.GetVonNeumannNeighbors(x, y);
        ECellState[] states = _variableGrid.GetStatesFromCells(adjacentStates);
        ECellState[] filtered = states.Where(state => state == otherState.ECellState).ToArray();
        if (filtered.Length == 3) return true;

        return false;
    }
}
