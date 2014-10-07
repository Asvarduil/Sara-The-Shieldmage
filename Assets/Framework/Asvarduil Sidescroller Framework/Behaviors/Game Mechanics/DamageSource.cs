using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DamageSource : DebuggableBehavior
{
	#region Variables / Properties

	public List<string> AllowedTags;

	public float DamageForce;
	public int DamageAmount;

	#endregion Variables / Properties

	#region Engine Hooks

	public void OnTriggerEnter(Collider who)
	{
		if(! AllowedTags.Contains(who.tag))
			return;

		who.gameObject.SendMessage("OnDamageTaken", SendMessageOptions.DontRequireReceiver);

		HealthController health = who.gameObject.GetComponent<HealthController>();
		if(health != null)
			health.TakeDamage(DamageAmount);

		SidescrollingMovement movement = who.gameObject.GetComponent<SidescrollingMovement>();
		if(movement != null)
			movement.RepelFromObject(gameObject, DamageForce);
	}

	#endregion Engine Hooks

	#region Methods

	#endregion Methods
}
