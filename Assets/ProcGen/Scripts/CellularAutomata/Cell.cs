using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellStateData _stateData;
    public CellStateData StateData 
    {
        get { return _stateData; }
        set 
        {
            _stateData = value;
            Setup();
        }
    }

    private MeshFilter _filter;
    private MeshRenderer _renderer;

    private void Setup() 
    {
        SetMeshAndMaterialsForState();
    }

    public void SetMeshAndMaterialsForState()
    {
        if (_filter == null) _filter = GetComponent<MeshFilter>();
        if (_renderer == null) _renderer = GetComponent<MeshRenderer>();

        _filter.mesh = _stateData.Mesh;
        _renderer.materials = _stateData.Materials;
    }

    public void SetInstanceColor(Color newColor)
    {
        foreach (Material m in _renderer.materials)
        {
            m.SetInt("_UseAlternativeColor", 1);
            m.SetColor("_AlternativeColor", newColor);
        }
    }
}
