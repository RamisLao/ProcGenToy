using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>
[CreateAssetMenu(menuName = "Event Channel/Void")]
public class EventChannelVoid : ScriptableObject
{
	public UnityAction OnEventRaised;

	[Button("Raise Event")]
	public void RaiseEvent()
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke();
	}
}


