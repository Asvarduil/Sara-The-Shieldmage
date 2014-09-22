using UnityEngine;
using System.Collections;

public class HealthPickup : Pickup
{
	#region Variables / Properties

	public int HealAmount;

	private HealthController _health;

	#endregion Variables / Properties
	
	#region Engine Hooks
	
	#endregion Engine Hooks
	
	#region Methods

	public override bool NeedsPickup(GameObject who)
	{
		_health = who.GetComponent<HealthController>();
		if(_health == null)
			return false;

		return ! _health.IsFull;
	}

	public override void OnPickup()
	{
		_health.Heal(HealAmount);
	}
	
	#endregion Methods
}
