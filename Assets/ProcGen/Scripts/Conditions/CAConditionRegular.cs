using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Cellular Automata/Conditions/Regular")]
public class CAConditionRegular : CACondition 
{
    [SerializeField] private ECellState _wallState;
    [SerializeField] private ECellState _floorState;
    // If the cell is a wall and there are less wall neighbors than WallToFloorConversion, we make the cell a floor
    [SerializeField] private int WallToFloorConversion = 3;
    // If the cell is a floor and there are more neighbor walls than FloorToWallConversion, we make the cell a wall
    [SerializeField] private int FloorToWallConversion = 4;

    public override ECellState GetNewState(ECellState currentCellState, ECellState[] neighborStates) {

        ECellState[] walls = neighborStates.Where(s => s == _wallState).ToArray();
        int wallCount = walls.Length;

        if (currentCellState == _wallState && wallCount < WallToFloorConversion)
        {
            return _floorState;
        }
        else if (currentCellState == _floorState && wallCount > FloorToWallConversion)
        {
            return _wallState;
        }
        else return currentCellState;
    }
}
