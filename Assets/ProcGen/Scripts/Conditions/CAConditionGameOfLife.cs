using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Cellular Automata/Conditions/Game Of Life")]
public class CAConditionGameOfLife : CACondition {

    [SerializeField] private ECellState _wallState;
    [SerializeField] private ECellState _floorState;
    [SerializeField] private List<int> _lonely = new() { 0, 1 };
    [SerializeField] private List<int> _overcrowded = new() { 4, 5, 6, 7, 8 };
    [SerializeField] private List<int> _lives = new() { 2, 3 };
    [SerializeField] private List<int> _birth = new() { 3 };
    [SerializeField] private List<int> _barren = new() { 0, 1, 2, 4, 5, 6, 7, 8 };

    public override ECellState GetNewState(ECellState currentCellState, ECellState[] neighborStates) {
        ECellState[] walls = neighborStates.Where(s => s == _wallState).ToArray();
        int wallCount = walls.Length;

        if (currentCellState == _wallState)
        {
            if (_lonely.Contains(wallCount) ||
                _overcrowded.Contains(wallCount)) return _floorState;
            else if (_lives.Contains(wallCount)) return _wallState;
            else return currentCellState;
        }
        else
        {
            if (_birth.Contains(wallCount)) return _wallState;
            else if (_barren.Contains(wallCount)) return _floorState;
            else return currentCellState;
        }
    }
}
