using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CACondition : ScriptableObject
{
    public bool UseMooreNeighbors = true;
    public abstract ECellState GetNewState(ECellState currentCellState, ECellState[] neighborStates);
}
