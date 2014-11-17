using UnityEngine;
using System.Collections;

public class BloodPinionCultist : DebuggableBehavior 
{
	#region Variables / Properties

	private HealthController _health;
	private SidescrollingMovement _movement;

	#endregion Variables / Properties

	#region Hooks

	public void Start()
	{
		_health = GetComponent<HealthController>();
		_movement = GetComponent<SidescrollingMovement>();
	}

	public void Update()
	{
		_movement.MoveCharacter();
	}

	#endregion Hooks

	#region Methods

	#endregion Methods
}
