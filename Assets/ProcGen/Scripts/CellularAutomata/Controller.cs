using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for receiving input from keyboard and raising events
/// </summary>
public class Controller : MonoBehaviour
{
    [Title("Broadcasting on")]
    [PropertyTooltip("KeyCode: Q")]
    [SerializeField] private EventChannelVoid _eventCreateGrid;
    [PropertyTooltip("KeyCode: W")]
    [SerializeField] private EventChannelVoid _eventResetGrid;
    [PropertyTooltip("KeyCode: A")]
    [SerializeField] private EventChannelVoid _eventInitializeStates;
    [PropertyTooltip("KeyCode: S")]
    [SerializeField] private EventChannelVoid _eventRunCellularAutomata;
    [PropertyTooltip("KeyCode: D")]
    [SerializeField] private EventChannelVoid _eventStopCellularAutomata;
    [PropertyTooltip("KeyCode: Z")]
    [SerializeField] private EventChannelVoid _eventFillHoles;
    [PropertyTooltip("KeyCode: X")]
    [SerializeField] private EventChannelVoid _eventDeleteSmallIslands;
    [PropertyTooltip("KeyCode: C")]
    [SerializeField] private EventChannelVoid _eventRunFloodFill;
    [PropertyTooltip("KeyCode: V")]
    [SerializeField] private EventChannelVoid _eventAddEdgesToIslands;

    void Update() 
    {
        if (Input.GetKey(KeyCode.Q))
        {
            _eventCreateGrid.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.W)) 
        {
            _eventResetGrid.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.A))
        {
            _eventInitializeStates.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.S)) 
        {
            _eventRunCellularAutomata.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            _eventStopCellularAutomata.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            _eventFillHoles.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.X))
        {
            _eventDeleteSmallIslands.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.C)) 
        {
            _eventRunFloodFill.RaiseEvent();
        }
        if (Input.GetKey(KeyCode.V))
        {
            _eventAddEdgesToIslands.RaiseEvent();
        }
    }
}
