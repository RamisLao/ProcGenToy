using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// This class is responsible for creating the base grid and storing it in VariableGrid.
/// Grid parameters (e.g. grid size, cell size, etc) are found in GridSettings.
/// </summary>
public class GridMaker : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] private GridSettings _gridSettings;

    [Title("State Data")]
    [SerializeField] private CellStateData _floorStateData;
    [SerializeField] private CellStateData _defaultStateData;

    [Title("Prefabs")]
    [SerializeField] private Cell _cellPrefab;

    [Title("Variables")]
    [SerializeField] private VariableGrid _variableGrid;
    
    [Title("Listening on")]
    [SerializeField] private EventChannelVoid _eventCreateGrid;
    [SerializeField] private EventChannelVoid _eventResetGrid;

    private void Awake()
    {
        _eventCreateGrid.OnEventRaised += CreateGrid;
        _eventResetGrid.OnEventRaised += ResetGrid;
    }

    // Destroy all gameObjects in VariableGrid and set it to null
    private void ResetGrid() {
        if (_variableGrid.Value == null) return;

        for (int x = 0; x < _variableGrid.Value.Length; x++) {
            for (int y = 0; y < _variableGrid.Value[0].Length; y++) {
                Destroy(_variableGrid.Value[x][y].gameObject);
            }
        }

        _variableGrid.Value = null;
    }

    // Create a horizontal grid with size (_gridSettings.GridSizeX, _gridSettings.GridSizeY)
    // All cells will start with the default state
    private void CreateGrid() {
        if (_variableGrid.Value != null) return;

        _variableGrid.Value = new Cell[_gridSettings.GridSizeX][];
        Vector3 worldTopLeft = transform.position;

        for (int x = 0; x < _gridSettings.GridSizeX; x++) {

            _variableGrid.Value[x] = new Cell[_gridSettings.GridSizeY];
            for (int y = 0; y < _gridSettings.GridSizeY; y++) {

                // Build the Grid to the right and backwards
                Vector3 worldPoint = worldTopLeft + Vector3.right * (x * _gridSettings.CellSize + _gridSettings.CellHalfSize)
                                     + Vector3.back * (y * _gridSettings.CellSize + _gridSettings.CellHalfSize);
                // Instantiate a floor base that will always be there
                Cell floor = Instantiate(_cellPrefab, worldPoint, Quaternion.identity, transform);
                floor.StateData = _floorStateData;
                // Instantiate the actual cell for the grid
                Cell c = Instantiate(_cellPrefab, worldPoint, Quaternion.identity, transform);
                c.StateData = _defaultStateData;
                _variableGrid.Value[x][y] = c;
            }
        }
    }
}
