using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class Pickup : DebuggableBehavior
{
	#region Variables / Properties

	public List<string> AllowedTags;

	#endregion Variables / Properties
	
	#region Engine Hooks

	public void OnTriggerEnter(Collider who)
	{
		if(! AllowedTags.Contains(who.tag))
			return;

		if(! NeedsPickup(who.gameObject))
			return;

		OnPickup();
		gameObject.SetActive(false);
	}
	
	#endregion Engine Hooks
	
	#region Methods

	public abstract bool NeedsPickup(GameObject who);

	public abstract void OnPickup();
	
	#endregion Methods
}
