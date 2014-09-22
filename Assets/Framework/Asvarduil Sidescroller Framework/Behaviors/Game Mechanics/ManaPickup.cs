using UnityEngine;
using System.Collections;

public class ManaPickup : Pickup
{
	#region Variables / Properties

	public int RestoreAmount;
	private ManaController _mana;

	#endregion Variables / Properties
	
	#region Engine Hooks
	
	#endregion Engine Hooks
	
	#region Methods

	public override bool NeedsPickup(GameObject who)
	{
		_mana = who.GetComponent<ManaController>();
		if(_mana == null)
			return false;
		
		return ! _mana.IsFull;
	}
	
	public override void OnPickup()
	{
		_mana.Gain(RestoreAmount);
	}
	
	#endregion Methods
}
