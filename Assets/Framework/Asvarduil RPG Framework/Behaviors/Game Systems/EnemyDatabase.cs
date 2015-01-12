using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class EnemyDatabase : DatabaseBase<EnemyDatabase>
{
	#region Variables / Properties

    public List<GameObject> BattlePrefabs;
	public List<Enemy> Enemies;

    private ItemDatabase _itemDB;
    private AbilityDatabase _abilityDB;

	#endregion Variables / Properties

	#region Hooks

    public override void Start()
    {
        if (UseExistingList)
            return;

        StartCoroutine(WaitForOtherRepositories());
    }

    private IEnumerator WaitForOtherRepositories()
    {
        _itemDB = ItemDatabase.Instance;
        _abilityDB = AbilityDatabase.Instance;

        while (!_itemDB.IsLoaded || !_abilityDB.IsLoaded)
        {
            yield return 0;
        }

        DebugMessage("Item DB and Ability DB are initialized.");
        LoadItemsFromJson();
    }

	#endregion Hooks

	#region Data Management Methods

	public Enemy FindEnemyByName(string enemyName)
	{
		Enemy result = null;
		for (int i = 0; i < Enemies.Count; i++) 
		{
			Enemy current = Enemies[i];
			if(current.Name != enemyName)
				continue;

			result = current;
			break;
		}

		return result;
	}

    public GameObject GetBattlePrefabByName(string name)
    {
        GameObject result = null;
        for(int i = 0; i < BattlePrefabs.Count; i++)
        {
            GameObject current = BattlePrefabs[i];
            if (current.name != name)
                continue;

            result = current;
            break;
        }

        return result;
    }

	#endregion Data Management Methods

    #region Data Access Methods

    protected override void MapBlob()
    {
        JSONNode parsed = JSON.Parse(RawBlob);

        var enemies = parsed["Enemies"].AsArray;
        Enemies = new List<Enemy>();
        foreach(var enemy in enemies.Childs)
        {
            Enemy newEnemy = new Enemy();

            newEnemy.Name = enemy["Name"];
            newEnemy.BattlePrefab = GetBattlePrefabByName(enemy["BattlePrefab"]);
            newEnemy.CounterAttackName = enemy["CounterAttackName"];

            MapEnemyHealthSystem(enemy, newEnemy);
            MapEnemyModifiableStats(enemy, newEnemy);
            MapEnemyActiveEffects(enemy, newEnemy);
            MapEnemyAbilities(enemy, newEnemy);
            MapEnemyItemDrops(enemy, newEnemy);

            Enemies.Add(newEnemy);
        }
    }

    private void MapEnemyHealthSystem(JSONNode enemy, Enemy newEnemy)
    {
        newEnemy.Health = new HealthSystem
        {
            HP = enemy["HP"].AsInt,
            MaxHP = enemy["MaxHP"].AsInt,
            RegenAmount = enemy["RegenAmount"].AsInt,
            RegenRate = enemy["RegenRate"].AsFloat
        };
    }

    private void MapEnemyModifiableStats(JSONNode enemy, Enemy newEnemy)
    {
        var modifiableStats = enemy["Stats"].AsArray;
        newEnemy.ModifiableStats = new List<ModifiableStat>();
        foreach (var stat in modifiableStats.Childs)
        {
            ModifiableStat newStat = new ModifiableStat
            {
                Name = stat["Name"],
                Value = stat["Value"].AsInt,
                FixedModifier = stat["FixedModifier"].AsInt,
                ScalingModifier = stat["ScalingModifier"].AsFloat
            };

            newEnemy.ModifiableStats.Add(newStat);
        }
    }

    private void MapEnemyActiveEffects(JSONNode enemy, Enemy newEnemy)
    {
        var activeEffects = enemy["ActiveEffects"].AsArray;
        newEnemy.ActiveEffects = new List<AbilityEffect>();
        foreach (string effectName in activeEffects.Childs)
        {
            AbilityEffect effect = _abilityDB.GetAbilityEffectByName(effectName);
            newEnemy.ActiveEffects.Add(effect);
        }
    }

    private void MapEnemyAbilities(JSONNode enemy, Enemy newEnemy)
    {
        var abilities = enemy["Abilities"].AsArray;
        newEnemy.Abilities = new List<Ability>();
        foreach (string abilityName in abilities.Childs)
        {
            DebugMessage("Loading ability " + abilityName + " into enemy " + newEnemy.Name);
            var ability = _abilityDB.GetAbilityByName(abilityName);

            newEnemy.Abilities.Add(ability);
        }
    }

    private void MapEnemyItemDrops(JSONNode enemy, Enemy newEnemy)
    {
        var itemDrops = enemy["Drops"].AsArray;
        newEnemy.Drops = new List<ItemDrop>();
        foreach (var drop in itemDrops.Childs)
        {
            ItemDrop newDrop = new ItemDrop
            {
                Item = _itemDB.FindItemWithName(drop["ItemName"]),

                DropRate = drop["DropRate"].AsInt,
                BaseAmount = drop["BaseAmount"].AsInt,
                MinimumBonus = drop["MinimumBonus"].AsInt,
                MaximumBonus = drop["MaximumBonus"].AsInt
            };

            newEnemy.Drops.Add(newDrop);
        }
    }

    #endregion Data Access Methods
}
