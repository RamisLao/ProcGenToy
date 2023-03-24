using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used to send found regions previous to the floodfill algorithm
/// </summary>

[CreateAssetMenu(menuName = "Event Channel/Regions")]
public class EventChannelRegions : ScriptableObject
{
	public UnityAction<int[,]> OnEventRaised;

	[Button("Raise Event")]
	public void RaiseEvent(int[,] value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}
}
