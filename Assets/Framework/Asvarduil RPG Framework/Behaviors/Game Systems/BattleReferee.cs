using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class BattleReferee : ManagerBase<BattleReferee>
{
	#region Variables / Properties

	public bool EvaluateBattleState = true;
	public string TitleScene;

	public List<PlayableCharacter> Players;
	public List<Enemy> Enemies;

	public AudioClip VictoryTheme;
	public AudioClip GameOverTheme;

	public bool BattleInProgress
	{
		get { return LivingPlayerCount > 0 && LivingEnemyCount > 0; }
	}

	public int LivingPlayerCount
	{
		get { return Players.Where(p => !p.Health.IsDead).ToList().Count; }
	}

	public int LivingEnemyCount
	{
		get { return Enemies.Where(e => !e.Health.IsDead).ToList().Count; }
	}

	private Maestro _maestro;
	private PartyManager _partyManager;
	private BattleManager _battleManager;
	private TransitionManager _transitionManager;

	private VictoryPresenter _victory;
	private DefeatPresenter _defeat;
	private LootPresenter _loot;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_maestro = Maestro.Instance;
		_partyManager = PartyManager.Instance;
		_battleManager = BattleManager.Instance;
		_transitionManager = TransitionManager.Instance;

		LoadPlayers();
	}

	public void Update()
	{
		if(! EvaluateBattleState)
			return;

		if(BattleInProgress)
		{
			// TODO: Increment all entities' ATB stat by their ATB speed stat.
			return;
		}

		if(LivingPlayerCount == 0)
		{
			Defeat();
			EvaluateBattleState = false;
		}

		if(LivingEnemyCount == 0)
		{
			Victory();
			EvaluateBattleState = false;
		}
	}

	#endregion Engine Hooks

	#region Methods

	private void LoadPlayers()
	{
		Players = _partyManager.FindAvailableCharacters().ToList();
	}
	
	private void LoadEnemies()
	{
		// TODO: Call the enemy loader, and load enemies that way.
		Enemies = new List<Enemy>();
	}

	private void Victory()
	{
		_maestro.ChangeTunes(VictoryTheme);
		_victory.SetVisibility(true);
	}

	private void RollForLoot()
	{
		List<InventoryItem> loot = new List<InventoryItem>();

		for(int i = 0; i < Enemies.Count; i++)
		{
			Enemy enemy = Enemies[i];
			List<InventoryItem> enemyLoot = enemy.RollForLoot().ToList();
			loot.AddRange(enemyLoot);
		}

		if(loot.Count == 0)
		{
			ReturnToOriginalScene();
			return;
		}

		// TODO: Pass list of obtained loot to the Loot Presenter!
		_loot.SetVisibility(true);
	}

	private void ReturnToOriginalScene()
	{
		_transitionManager.ChangeScenes(true);
	}

	private void Defeat()
	{
		_maestro.ChangeTunes(GameOverTheme);
		_defeat.SetVisibility(true);
	}

	private void ReturnToTitleScreen()
	{
		Application.LoadLevel(TitleScene);
	}

	#endregion Methods
}
