using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Grid")]
public class GridSettings : ScriptableObject
{
    public Vector2 GridWorldSize = new(40, 40);
    public float CellSize = 1;
    public float CellHalfSize => CellSize / 2;
    public int GridSizeX => Mathf.RoundToInt(GridWorldSize.x / CellSize);
    public int GridSizeY => Mathf.RoundToInt(GridWorldSize.y / CellSize);
}
