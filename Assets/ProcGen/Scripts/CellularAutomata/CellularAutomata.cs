using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to run and stop Cellular Automata iterations on VariableGrid using CACondition to get new cell states
/// </summary>
public class CellularAutomata : MonoBehaviour
{
    [SerializeField] private CASettings _caSettings;
    // Cellular automata rules that will be used to run the system
    [SerializeField] private CACondition _caCondition;
    [SerializeField] private VariableGrid _variableGrid;

    [Title("Listening on")]
    [SerializeField] private EventChannelVoid _eventInitializeStates;
    [SerializeField] private EventChannelVoid _eventRunIterations;
    [SerializeField] private EventChannelVoid _eventStopIterations;

    private bool _isRunning;

    private void Awake()
    {
        _eventInitializeStates.OnEventRaised += InitializeStates;
        _eventRunIterations.OnEventRaised += RunIterations;
        _eventStopIterations.OnEventRaised += StopIterations;
    }

    // Use the probability distributions in _caSettings to set the states of cells in _variableGrid in a pseudo random manner
    private void InitializeStates()
    {
        for (int i = 0; i < _variableGrid.Value.Length; i++)
        {
            for (int j = 0; j < _variableGrid.Value[0].Length; j++)
            {
                float proba = Random.Range(0.0f, 1.0f);
                CellStateData newState = _caSettings.GetStateFromRandomNum(proba);
                _variableGrid.Value[i][j].StateData = newState;
            }
        }
    }

    // Start the Cellular Automata
    private void RunIterations() {
        if (_variableGrid.Value == null) return;

        StartCoroutine(RunIterationsCoroutine());
    }

    // Stop the Cellular Automata
    private void StopIterations() {
        _isRunning = false;
    }

    private IEnumerator RunIterationsCoroutine() {
        _isRunning = true;
        // Create an alternate grid to leave the real one untouched until we get the new states
        ECellState[,] alternateGrid = new ECellState[_variableGrid.Value.Length, _variableGrid.Value[0].Length];
        int iterationCounter = 0;

        while (_isRunning) {

            for (int x = 0; x < _variableGrid.Value.Length; x++) {
                for (int y = 0; y < _variableGrid.Value[0].Length; y++) {
                    // Current state of cell
                    ECellState currentCellState = _variableGrid.Value[x][y].StateData.ECellState;
                    // Get the cell's neighbors
                    List<Cell> neighborsList;
                    if (_caCondition.UseMooreNeighbors)
                        neighborsList = _variableGrid.GetMooreNeighbors(x, y);
                    else
                        neighborsList = _variableGrid.GetVonNeumannNeighbors(x, y);

                    // Get all the neighbors' states
                    ECellState[] neighborsStates = _variableGrid.GetStatesFromCells(neighborsList);

                    // Get the new state of the current cell according to the rules in _caCondition
                    ECellState newState = _caCondition.GetNewState(currentCellState, neighborsStates);
                    alternateGrid[x, y] = newState;
                }
            }

            // Update _variableGrid with the new states
            for (int x = 0; x < _variableGrid.Value.Length; x++) {
                for (int y = 0; y < _variableGrid.Value[0].Length; y++) {

                    _variableGrid.Value[x][y].StateData = _caSettings.GetCellStateDataFromECellState(alternateGrid[x, y]);
                }
            }

            iterationCounter++;
            if (_caSettings.UseFixedIterations && iterationCounter >= _caSettings.NumOfIterations) {
                _isRunning = false;
            }

            yield return null;
        }
    }
}
