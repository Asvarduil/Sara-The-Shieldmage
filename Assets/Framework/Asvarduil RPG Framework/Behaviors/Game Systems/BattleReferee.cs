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

	public List<GameObject> PlayerPositions;
	public List<GameObject> EnemyPositions;

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
	private EnemyDatabase _enemies;
	private PartyManager _partyManager;
	private BattleManager _battleManager;
	private TransitionManager _transitionManager;

	private VictoryPresenter _victory;
	private BattlePresenter _battle;
	private DefeatPresenter _defeat;
	private LootPresenter _loot;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_maestro = Maestro.Instance;
		_enemies = EnemyDatabase.Instance;
		_partyManager = PartyManager.Instance;
		_battleManager = BattleManager.Instance;
		_transitionManager = TransitionManager.Instance;

		_loot = GetComponentInChildren<LootPresenter>();
		_battle = GetComponentInChildren<BattlePresenter>();
		_defeat = GetComponentInChildren<DefeatPresenter>();
		_victory = GetComponentInChildren<VictoryPresenter>();

		_maestro.ChangeTunes(_battleManager.BattleTheme);
		LoadPlayers();
		LoadEnemies();

		_battle.SetVisibility(true);
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

		// If no players in the party manager, use what's in the Referee already.
		if (Players.Count == 0)
			return;

		for (int i = 0; i < Players.Count; i++) 
		{
			PlayableCharacter player = Players[i];

			if(player.BattlePrefab != null)
			{
				Vector3 position = PlayerPositions[i].transform.position;
				GameObject.Instantiate(player.BattlePrefab, position, Quaternion.identity);
			}
			else
			{
				DebugMessage(player.Name + " has no Battle Prefab!", LogLevel.LogicError);
			}
		}
	}
	
	private void LoadEnemies()
	{
		// If no enemies in the battle manager, use what's in the referee already.
		if (_battleManager.EnemyNames.Count == 0) 
		{
			for(int i = 0; i < Enemies.Count; i++)
			{
				Enemy enemy = Enemies[i];
				Vector3 position = EnemyPositions[i].transform.position;
				GameObject.Instantiate(enemy.BattlePrefab, position, Quaternion.identity);
			}

			return;
		}

		Enemies = new List<Enemy>();
		for(int i = 0; i < _battleManager.EnemyNames.Count; i++)
		{
			string enemyName = _battleManager.EnemyNames[i];
			Enemy enemy = _enemies.FindEnemyByName(enemyName);
			Enemies.Add(enemy);

			if(enemy.BattlePrefab != null)
			{
				Vector3 position = EnemyPositions[i].transform.position;
				GameObject.Instantiate(enemy.BattlePrefab, position, Quaternion.identity);
			}
			else
			{
				DebugMessage(enemy.Name + " has no Battle Prefab!", LogLevel.LogicError);
			}
		}
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
