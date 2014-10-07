using UnityEngine;
using System.Collections.Generic;

public class ResourceConversationEvents : DebuggableBehavior
{
	#region Variables / Properties

	private HealthController _health;
	private ManaController _mana;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		GameObject player = FindObjectOfType<SidescrollingPlayerControl>().gameObject;
		_health = player.GetComponent<HealthController>();
		_mana = player.GetComponent<ManaController>();
	}

	#endregion Engine Hooks

	#region Messages

	public void IncreaseMaxMana(List<string> args)
	{
		_mana.IncrementMax();
	}

	public void IncreaseMaxHealth(List<string> args)
	{
		_health.IncrementMax();
	}

	#endregion Messages
}
