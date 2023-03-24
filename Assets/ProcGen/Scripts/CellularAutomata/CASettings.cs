using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Settings for the Cellular Automata
/// </summary>
[CreateAssetMenu(menuName = "Settings/Cellular Automata")]
public class CASettings : SerializedScriptableObject
{
    [InfoBox("All Cell State Data available")]
    public List<CellStateData> CellStatesData;
    [InfoBox("At initialization, the probability of getting one the following Cell States")]
    public Dictionary<CellStateData, float> InitStateProba;
    // If true, use a fixed number of iterations. If false, run indefinitely
    public bool UseFixedIterations = true;
    [ShowIf("@this.UseFixedIterations == true")]
    public int NumOfIterations = 50;

    // Get a CellStateData object that has the same ECellState as eCellState
    public CellStateData GetCellStateDataFromECellState(ECellState eCellState)
    {
        CellStateData d = CellStatesData.Where(data => data.ECellState.Name == eCellState.Name).ToArray()[0];
        return d;
    }

    // Using InitStateProba, get a state from the (0, 1) random number num
    public CellStateData GetStateFromRandomNum(float num)
    {
        float currentMin = 0;
        foreach (KeyValuePair<CellStateData, float> kv in InitStateProba)
        {
            if (num > currentMin && num <= currentMin + kv.Value) return kv.Key;
            currentMin += kv.Value;
        }

        return CellStatesData[Random.Range(0, CellStatesData.Count)];
    }

}
