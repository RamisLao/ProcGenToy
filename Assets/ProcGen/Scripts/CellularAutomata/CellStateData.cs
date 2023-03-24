using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cellular Automata/Cell State Data")]
public class CellStateData : ScriptableObject
{
    public ECellState ECellState;
    public Mesh Mesh;
    public Material[] Materials;
}
