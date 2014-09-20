using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class BattleTrigger : DebuggableBehavior
{
	#region Variables / Properties

	public string AffectedTag = "Player";

	public AudioClip BattleTheme;
	public string BattleScene;
	public List<string> EnemyNames;

	private BattleManager _battleManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_battleManager = BattleManager.Instance;
	}

	public void OnTriggerEnter(Collider e)
	{
		if(e.tag != AffectedTag)
			return;

		_battleManager.PrepareBattle(EnemyNames, BattleScene, BattleTheme);
		// TODO: Trigger Battle Transition...
		_battleManager.InitiateBattle();
	}

	#endregion Engine Hooks
}
