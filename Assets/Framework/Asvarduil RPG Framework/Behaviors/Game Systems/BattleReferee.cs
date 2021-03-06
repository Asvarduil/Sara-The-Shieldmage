﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BattleReferee : ManagerBase<BattleReferee>
{
	#region Variables / Properties

	private const string ATBName = "ATB";
	private const string maxATBName = "Max ATB";
	private const string ATBSpeedName = "ATB Speed";

    private float _lastATBPollTime;
    private float _ATBPollRate = 0.5f;

	public bool EvaluateBattleState = true;
	public string TitleScene;

	public List<PlayableCharacter> Players;
	public List<Enemy> Enemies;

	public List<GameObject> PlayerPositions;
	public List<GameObject> EnemyPositions;

	public AudioClip VictoryTheme;

    public List<CombatEntity> AllCombatEntities
    {
        get 
        {
            List<CombatEntity> allEntities = new List<CombatEntity>();
            allEntities.AddRange(Players.Select(p => p as CombatEntity));
            allEntities.AddRange(Enemies.Select(e => e as CombatEntity));

            return allEntities;
        }
    }

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

    private Fader _fader;
	private Maestro _maestro;
    private EnemyDatabase _enemyDB;
    private AbilityDatabase _abilityDB;
	private PartyManager _partyManager;
	private BattleManager _battleManager;
	private TransitionManager _transitionManager;

    private BattleStatPresenter _battleStats;
	private BattleTargetPresenter _targeting;
	private BattleCommandPresenter _command;
	private BattleVictoryPresenter _victory;
    private BattleTextPresenter _message;
	private BattleLootPresenter _loot;

	#endregion Variables / Properties

    #region Hook Variables

    private PlayableCharacter player;
    private Enemy enemy;
    private ModifiableStat ATB;
    private ModifiableStat speed;
    private ModifiableStat maxATB;

    #endregion Hook Variables

    #region Engine Hooks

    public void Start()
	{
		_maestro = Maestro.Instance;
		_enemyDB = EnemyDatabase.Instance;
        _abilityDB = AbilityDatabase.Instance;
		_partyManager = PartyManager.Instance;
		_battleManager = BattleManager.Instance;
		_transitionManager = TransitionManager.Instance;

		_loot = GetComponentInChildren<BattleLootPresenter>();
        _battleStats = GetComponentInChildren<BattleStatPresenter>();
		_command = GetComponentInChildren<BattleCommandPresenter>();
		_targeting = GetComponentInChildren<BattleTargetPresenter>();
        _victory = GetComponentInChildren<BattleVictoryPresenter>();
        _message = GetComponentInChildren<BattleTextPresenter>();

		_maestro.ChangeTunes(_battleManager.BattleTheme);
		LoadPlayers();
		LoadEnemies();
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

	#region Initialization Methods

	private void LoadPlayers()
	{
		Players = _partyManager.FindAvailableCharacters().ToList();
		
		// If no players in the party manager, use what's in the Referee already.
		if (Players.Count == 0)
			return;
		
        // For each player, spawn their battle piece, and enable their character display.
        for (int i = 0; i < _battleStats.CharacterStatPresenters.Count; i++) 
		{
            if(i > Players.Count - 1)
            {
                _battleStats.HideCharacterDisplay(i);
                continue;
            }

			PlayableCharacter player = Players[i];
			
			if(player.BattlePrefab != null)
			{
				Vector3 position = PlayerPositions[i].transform.position;
                player.ScenePosition = position;
                player.BattlePiece = ((GameObject)GameObject.Instantiate(player.BattlePrefab, position, Quaternion.identity)).GetComponent<BattleEntity>();
			}
			else
			{
				DebugMessage(player.Name + " has no Battle Prefab!", LogLevel.LogicError);
			}

            _battleStats.BindCharacterDisplay(player, i);
            player.Health.OnHealthChanged = () => _battleStats.UpdateHealth(player);
            _abilityDB.HydrateCombatEntityAbilitiesFromList(player);
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
                enemy.ScenePosition = position;

                var battlePieceInstance = (GameObject)GameObject.Instantiate(enemy.BattlePrefab, position, Quaternion.identity);
                enemy.BattlePiece = battlePieceInstance.GetComponent<BattleEntity>();
			}
			
			return;
		}
		
		Enemies = new List<Enemy>();
		for(int i = 0; i < _battleManager.EnemyNames.Count; i++)
		{
            if (i > EnemyPositions.Count - 1)
                throw new InvalidOperationException(string.Format("Enemy #{0} has nowhere to be spawned!", (i + 1)));

			string enemyName = _battleManager.EnemyNames[i];
			Enemy enemy = _enemyDB.FindEnemyByName(enemyName).Clone() as Enemy;
			Enemies.Add(enemy);
			
			if(enemy.BattlePrefab != null)
			{
				Vector3 position = EnemyPositions[i].transform.position;
                enemy.ScenePosition = position;

                var battlePieceInstance = (GameObject) GameObject.Instantiate(enemy.BattlePrefab, position, Quaternion.identity);
				enemy.BattlePiece = battlePieceInstance.GetComponent<BattleEntity>();
			}
			else
			{
				DebugMessage(enemy.Name + " has no Battle Prefab!", LogLevel.LogicError);
			}
		}
	}

	#endregion Initialization Methods

	#region ATB Methods

	private void AdvanceATB()
	{
        float currentTime = Time.time;
        if (currentTime < _lastATBPollTime + _ATBPollRate)
            return;

        _lastATBPollTime = currentTime;
		AdvancePlayerATB();
		AdvanceEnemyATB();
	}
	
	private void AdvancePlayerATB()
	{
		for (int i = 0; i < Players.Count; i++) 
		{
			player = Players[i];
			
			// Dead players can't act!
			if(player.Health.IsDead)
				continue;

            player.OnATBTick();
			
			ATB = player.GetStatByName(ATBName);
			maxATB = player.GetStatByName(maxATBName);
			speed = player.GetStatByName(ATBSpeedName);
			
			if(ATB.Value < maxATB.Value)
			{
				ATB.Value += speed.Value;
                _battleStats.UpdateATB(i);
			}
			
			if(ATB.Value >= maxATB.Value)
			{
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
		for (int i = 0; i < Enemies.Count; i++) 
		{
			enemy = Enemies[i];
			
			// Dead players can't act!
			if(enemy.Health.IsDead)
				continue;

            enemy.OnATBTick();
			
			ATB = enemy.GetStatByName(ATBName);
			maxATB = enemy.GetStatByName(maxATBName);
			speed = enemy.GetStatByName(ATBSpeedName);
			
			if(ATB.Value < maxATB.Value)
			{
				ATB.Value += speed.Value;
			}
			
			if(ATB.Value >= maxATB.Value)
			{
				DebugMessage("Enemy " + enemy.Name + " is acting!");

                Ability selectedAbility = enemy.DetermineAction(enemy.Abilities);
                List<CombatEntity> targets = enemy.DetermineTarget(AllCombatEntities);

                for (int j = 0; j < targets.Count; j++)
                {
                    CombatEntity target = targets[j];
                    UseAbility(selectedAbility, enemy, target);
                }
			}
		}
	}

	#endregion ATB Methods

    #region Buff Management Methods

    public void ApplyEffectToTargetAsBuff(AbilityEffect effect, CombatEntity target)
    {
        DebugMessage("Applying " + effect.Name + " as a (de-)buff to " + target.Name + " for " + effect.Duration + " seconds.");

        effect.ApplyTime = Time.time;
        target.AddActiveEffect(effect);
    }

    #endregion Buff Management Methods

    #region Battle Action Methods

    public void PromptForTarget(AbilityTargetType targetType)
	{
		switch (targetType) 
		{
			case AbilityTargetType.TargetAlly:
				List<CombatEntity> players = Players.Select(p => p as CombatEntity).ToList();
				_targeting.Prompt(players, "Allies");
				break;
				
			case AbilityTargetType.TargetEnemy:
				List<CombatEntity> enemies = Enemies.Select(e => e as CombatEntity).ToList();
				_targeting.Prompt(enemies, "Enemies");
				break;
				
			default:
				DebugMessage("Target Type " + targetType + " is not supported.");
                return;
		}
	}
	
	public void ApplyTargetToPlayerCommand(CombatEntity target)
	{
        _commandOpen = false;
		_command.ApplyTarget(target);
	}

    public void UseAbilityOnAllTargets(Ability ability, CombatEntity source)
    {
        List<CombatEntity> targets = GetTargetListForAbility(ability.TargetType);

        for(int i = 0; i < targets.Count; i++)
        {
            CombatEntity target = targets[i];
            UseAbility(ability, source, target);
        }
    }

    private List<CombatEntity> GetTargetListForAbility(AbilityTargetType targetType)
    {
        List<CombatEntity> targets = new List<CombatEntity>();
        switch (targetType)
        {
            case AbilityTargetType.AllAlly:
                targets = Players.Select(p => p as CombatEntity).ToList();
                break;

            case AbilityTargetType.AllEnemy:
                targets = Enemies.Select(e => e as CombatEntity).ToList();
                break;

            case AbilityTargetType.All:
                targets = AllCombatEntities;
                break;

            default:
                DebugMessage("Target Type " + targetType + " is not supported.");
                break;
        }

        _commandOpen = false;
        _command.PresentGUI(false);

        return targets;
    }
	
	public void UseAbility(Ability ability, CombatEntity source, CombatEntity target)
	{
        // Code that does not include blocking animations.
        // BUG: When using a Self ability, only the Receipt Animation shows.
        //      I need to do something where the action animations shows, then
        //      the effect appears, then the receipt animation shows, then
        //      all Ability Effects are applied and ATB deducted from the source.
        _message.ShowMessage(ability.BattleText);

        source.BattlePiece.PlayAnimation(ability.ActionAnimation);
        CreateAbilityVisualEffect(ability, target);

        target.BattlePiece.PlayAnimation(ability.ReceiptAnimation);
        ApplyAbilityEffect(ability, source, target);

        ModifiableStat atbStat = source.GetStatByName(ATBName);
        atbStat.Value -= ability.AtbCost;
        DebugMessage("User " + source.Name + "'s ATB was reduced by " + ability.AtbCost + ", to " + atbStat.Value);
        if (source is PlayableCharacter)
            _battleStats.UpdateATB(source);
	}

    private void CreateAbilityVisualEffect(Ability ability, CombatEntity target)
    {
        if (ability.BattleEffect != null)
            GameObject.Instantiate(ability.BattleEffect, target.ScenePosition, Quaternion.identity);
        else
            DebugMessage("The ability needs to create a self-destructing 'battle effect'!", LogLevel.LogicError);
    }
	
	public void ApplyAbilityEffect(Ability ability, CombatEntity source, CombatEntity target)
    {
        if (ability == null
           || ability.Effects == null)
            throw new ArgumentNullException("An ability with at least one effect must be passed in.");

        target.PrepareCounterAttack(source);

        for(int i = 0; i < ability.Effects.Count; i++)
        {
            AbilityEffect effect = ability.Effects[i];

            // Stuff that can't affect dead characters shouldn't.
            if (target.Health.IsDead && !effect.AffectsDeadCharacters)
                continue;

            // First, make the appropriate effect calculation.
            effect.PerformEffectCalculation(source);

            if (effect.AffectsUser)
            {
                if (!effect.IsBuff)
                    ApplyEffectToTargetAsImmediate(effect, source);
                else
                    ApplyEffectToTargetAsBuff(effect, source);
            }
            else
            {
                if (!effect.IsBuff)
                    ApplyEffectToTargetAsImmediate(effect, target);
                else
                    ApplyEffectToTargetAsBuff(effect, target);
            }
        }
    }

    private void ApplyEffectToTargetAsImmediate(AbilityEffect effect, CombatEntity target)
    {
        DebugMessage("Applying effect " + effect.Name + ", which will change stat " + effect.TargetStat + " by " + effect.ActualAmount);

        // Then, apply the effect to the target.
        target.ApplyAbilityEffect(effect);

        // Tell the battle piece to provide feedback to the player.
        if (target.Health.IsDead)
            target.BattlePiece.DoDeathSequence();
    }

	#endregion Battle Action Methods

	#region End of Battle Methods

	private void Victory()
	{
        ClearBattleEffects();
        _command.PresentGUI(false);
        _targeting.PresentGUI(false);

        // TODO: Victory animations...
		_maestro.ChangeTunes(VictoryTheme);
        _victory.PresentGUI(true);
	}

    private void ClearBattleEffects()
    {
        for(int i = 0; i < Players.Count; i++)
        {
            CombatEntity current = Players[i];
            current.GetStatByName(ATBName).Value = 0;
            current.ClearActiveEffects();
        }
    }
	
	public void RollForLoot()
	{
		List<InventoryItem> loot = new List<InventoryItem>();
		
		for(int i = 0; i < Enemies.Count; i++)
		{
			Enemy enemy = Enemies[i];
			List<InventoryItem> enemyLoot = enemy.RollForLoot().ToList();
			//loot.AddRange(enemyLoot);

            for(int j = 0; j < enemyLoot.Count; j++)
            {
                InventoryItem currentLoot = enemyLoot[j];
                
                // Try to find an item in the loot list that has the same name.
                // If found, simply add onto the quantity.
                bool lootingItAlready = false;
                for(int k = 0; k < loot.Count; k++)
                {
                    InventoryItem existingLoot = loot[k];
                    if (existingLoot.Name != currentLoot.Name)
                        continue;

                    lootingItAlready = true;
                    existingLoot.Quantity += currentLoot.Quantity;
                    break;
                }

                if (lootingItAlready)
                    continue;

                loot.Add(currentLoot);
            }
		}
		
		if(loot.Count == 0)
		{
			ReturnToOriginalScene();
			return;
		}

        PartyInventory inventory = InventoryManager.Instance.ActiveInventory;
        for (int i = 0; i < loot.Count; i++ )
        {
            InventoryItem item = loot[i];
            inventory.GainItem(item, item.Quantity);
        }

        _loot.ShowLoot(loot);
	}
	
	public void ReturnToOriginalScene()
	{
		_transitionManager.ChangeScenes(true);
	}
	
	private void Defeat()
	{
        _command.PresentGUI(false);
        _targeting.PresentGUI(false);

        StartCoroutine(FadeToTitle());
	}

    private IEnumerator FadeToTitle()
    {
        _fader = FindObjectOfType<Fader>();
        _fader.FadeOut();

        while (!_fader.ScreenHidden)
            yield return 0;

        Application.LoadLevel("Title");
    }
	
	private void ReturnToTitleScreen()
	{
		Application.LoadLevel(TitleScene);
	}

	#endregion End of Battle Methods
}
