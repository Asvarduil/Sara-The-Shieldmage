using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public enum BattleType
{
	NormalBattle,
	BossBattle,
	FinalBattle
}

public class ConversationBattleEvents : DebuggableBehavior
{
	#region Variables / Properties
	
	public AudioClip NormalBattleTheme;
	public AudioClip BossBattleTheme;
	public AudioClip FinalBattleTheme;

	private BattleManager _battleManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_battleManager = BattleManager.Instance;
	}

	#endregion Engine Hooks

	#region Messages

	public void TriggerBattle(List<string> args)
	{
		if(args.Count < 3)
			throw new Exception("TriggerBattle takes three string arguments!");

		string battleType = args[0];
		AudioClip battleTheme = GetThemeFromBattleType(battleType);

		string battleScene = args[1];
		if(string.IsNullOrEmpty(battleScene))
		   throw new ArgumentNullException("Trigger battle requires a battle scene!");

		string enemyList = args[2];
		if(string.IsNullOrEmpty(enemyList))
			throw new Exception("TriggerBattle requires at least one enemy, separated by pipes ('|')");

		List<string> enemyNames = enemyList.Split('|').ToList();

		_battleManager.PrepareBattle(enemyNames, battleScene, battleTheme);
		_battleManager.InitiateBattle();
	}

	#endregion Messages

	#region Methods

	private AudioClip GetThemeFromBattleType(string battleType)
	{
		BattleType typeValue = (BattleType) Enum.Parse(typeof(BattleType), battleType);

		switch(typeValue)
		{
			case BattleType.NormalBattle:
				return NormalBattleTheme;

			case BattleType.BossBattle:
				return BossBattleTheme;

			case BattleType.FinalBattle:
				return FinalBattleTheme;

			default:
				return null;
		}
	}

	#endregion Methods
}
