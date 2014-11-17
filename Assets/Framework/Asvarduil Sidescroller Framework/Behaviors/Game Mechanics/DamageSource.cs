using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DamageSource : DebuggableBehavior
{
	#region Variables / Properties

	public bool DestroyOnImpact = false;
	public GameObject DestroyEffect;
	public List<string> AllowedTags;

	public float DamageForce;
	public int DamageAmount;

	#endregion Variables / Properties

	#region Engine Hooks

	public void OnTriggerEnter(Collider who)
	{
		if (! AllowedTags.Contains (who.tag)) {
			DebugMessage (who.name + " is a " + who.tag);
		} else {
			who.gameObject.SendMessage("OnDamageTaken", SendMessageOptions.DontRequireReceiver);
			
			ApplyDamage(who);
			ApplyKnockback(who);
		}

		ImpactDestruction();
	}

	#endregion Engine Hooks

	#region Methods

	private void ApplyDamage(Collider who)
	{
		HealthController health = who.gameObject.GetComponent<HealthController>();
		if (health != null)
			health.TakeDamage (DamageAmount);
		else
			DebugMessage(who.gameObject.name + " has no Health Controller.");
	}

	private void ApplyKnockback(Collider who)
	{
		SidescrollingMovement movement = who.gameObject.GetComponent<SidescrollingMovement>();
		if(movement != null)
			movement.RepelFromObject(gameObject, DamageForce);
		else
			DebugMessage(who.gameObject.name + " has no Sidescrolling Movement.");
	}

	private void ImpactDestruction()
	{
		if (! DestroyOnImpact) 
			return;

		DebugMessage(name + " hit something.  Destroying.");
		
		if(DestroyEffect != null)
			GameObject.Instantiate(DestroyEffect, transform.position, transform.rotation);
		
		Destroy(gameObject);
	}

	#endregion Methods
}
