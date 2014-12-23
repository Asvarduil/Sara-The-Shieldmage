using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class BattleReferee : ManagerBase<BattleReferee>
{
	#region Variables / Properties

	private const string ATBName = "ATB";
	private const string maxATBName = "Max ATB";
	private const string ATBSpeedName = "ATB Speed";

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

	private bool _commandOpen = false;

	private Maestro _maestro;
	private EnemyDatabase _enemies;
	private PartyManager _partyManager;
	private BattleManager _battleManager;
	private TransitionManager _transitionManager;

	private TargetingPresenter _targeting;
	private CommandPresenter _command;
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
		_command = GetComponentInChildren<CommandPresenter>();
		_targeting = GetComponentInChildren<TargetingPresenter>();

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
			AdvanceATB();
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

	public void PromptForTarget(AbilityTargetType targetType)
	{
		switch (targetType) 
		{
			case AbilityTargetType.TargetAlly:
				List<ICombatEntity> players = Players.Select(p => p as ICombatEntity).ToList();
				_targeting.Prompt(players, "Allies");
				break;

			case AbilityTargetType.TargetEnemy:
				List<ICombatEntity> enemies = Enemies.Select(e => e as ICombatEntity).ToList();
				_targeting.Prompt(enemies, "Enemies");
				break;

			default:
				DebugMessage("Target Type " + targetType + " is not supported.");
				break;
		}
	}

	public void ApplyTargetToCommand(ICombatEntity target)
	{
		_command.ApplyTarget(target);
	}

	public void UseAbility(ICombatEntity source, ICombatEntity target, Ability ability)
	{
		_commandOpen = false;
		_command.SetVisibility(false);

		DebugMessage("Character " + source.EntityName + " consumed " + ability.AtbCost + " ATB.");
		source.GetStatByName ("ATB").Value -= ability.AtbCost;

		// TODO: Instantiate ability in game world.
	}

	public void DamageEntity(ICombatEntity source, ICombatEntity target, int damage)
	{
		// TODO: Note the source of the damage for achievements or whatever.
		target.HealthSystem.TakeDamage(damage);
	}

	private void AdvanceATB()
	{
		AdvancePlayerATB();
		AdvanceEnemyATB();
	}

	private void AdvancePlayerATB()
	{
		PlayableCharacter player;

		ModifiableStat ATB;
		ModifiableStat speed;
		ModifiableStat maxATB;

		for (int i = 0; i < Players.Count; i++) 
		{
			player = Players[i];

			// Dead players can't act!
			if(player.Health.IsDead)
				continue;
			
			ATB = player.GetStatByName(ATBName);
			maxATB = player.GetStatByName(maxATBName);
			speed = player.GetStatByName(ATBSpeedName);
			
			if(ATB.Value < maxATB.Value)
			{
				DebugMessage("Player " + player.Name + " cannot execute a command yet.");
				ATB.Value += speed.Value;
			}
			
			if(ATB.Value >= maxATB.Value)
			{
				DebugMessage("Player " + player.Name + " is ready for a command.");
				if(! _commandOpen)
				{
					DebugMessage("Player " + player.Name + " is being prompted for a command.");
					_commandOpen = true;
					_command.Prompt(player);
				}
			}
		}
	}

	private void AdvanceEnemyATB()
	{
		Enemy enemy;

		ModifiableStat ATB;
		ModifiableStat speed;
		ModifiableStat maxATB;
		
		for (int i = 0; i < Enemies.Count; i++) 
		{
			enemy = Enemies[i];
			
			// Dead players can't act!
			if(enemy.Health.IsDead)
				continue;
			
			ATB = enemy.GetStatByName(ATBName);
			maxATB = enemy.GetStatByName(maxATBName);
			speed = enemy.GetStatByName(ATBSpeedName);
			
			if(ATB.Value < maxATB.Value)
			{
				DebugMessage("Enemy " + enemy.Name + " cannot execute a command yet.");
				ATB.Value += speed.Value;
			}
			
			if(ATB.Value >= maxATB.Value)
			{
				DebugMessage("Enemy " + enemy.Name + " is acting!");
				// TODO: Implement Enemy AI...
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
