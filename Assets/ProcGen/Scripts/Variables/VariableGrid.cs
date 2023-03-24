using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ScriptableObject where the main grid is stored, along with the states of all its cells
/// </summary>
[CreateAssetMenu(menuName = "Variables/Grid")]
public class VariableGrid : Variable<Cell[][]>
{
    public List<Cell> GetMooreNeighbors(int x, int y)
    {
        List<Cell> neighbors = new();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int newX = x + i;
                int newY = y + j;

                if (newX < 0 || newX > Value.Length - 1 || newY < 0 || newY > Value[0].Length - 1) continue;

                neighbors.Add(Value[newX][newY]);
            }
        }

        return neighbors;
    }

    public List<Cell> GetVonNeumannNeighbors(int x, int y)
    {
        List<Cell> neighbors = new();

        if (x > 0)
            neighbors.Add(Value[x - 1][y]);
        if (x < Value.Length - 1)
            neighbors.Add(Value[x + 1][y]);
        if (y > 0)
            neighbors.Add(Value[x][y - 1]);
        if (y < Value[0].Length - 1)
            neighbors.Add(Value[x][y + 1]);

        return neighbors;
    }

    public ECellState[] GetStatesFromCells(List<Cell> cells)
    {
        return cells.Select(cell => cell.StateData.ECellState).ToArray();
    }

    public bool IsForward(Cell referenceCell, Cell otherCell)
    {
        if (otherCell.transform.position.z > referenceCell.transform.position.z) return true;
        return false;
    }

    public bool IsRight(Cell referenceCell, Cell otherCell)
    {
        if (otherCell.transform.position.x > referenceCell.transform.position.x) return true;
        return false;
    }

    public bool IsBack(Cell referenceCell, Cell otherCell)
    {
        if (otherCell.transform.position.z < referenceCell.transform.position.z) return true;
        return false;
    }

    public bool IsLeft(Cell referenceCell, Cell otherCell)
    {
        if (otherCell.transform.position.x < referenceCell.transform.position.x) return true;
        return false;
    }
}
