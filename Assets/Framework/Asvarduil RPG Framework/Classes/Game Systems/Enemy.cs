using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class Enemy : ICombatEntity
{
	#region Variables / Properties

	public string Name;
	public GameObject BattlePrefab;
	public HealthSystem Health;
	public List<ModifiableStat> ModifiableStats;
	public List<Ability> Abilities;
	public List<ItemDrop> Drops;

	public string EntityName
	{
		get { return Name; }
	}

	public HealthSystem HealthSystem
	{
		get { return Health; }
	}
	
	public List<ModifiableStat> StatSystems
	{
		get { return StatSystems; }
	}

	#endregion Variables / Properties

	#region Methods

	public int GetModifiedStatValue(string statName)
	{
		int result = Health.GetStatByName(statName);
		if(result > -1)
			return result;
		
		ModifiableStat stat = GetStatByName(statName);
		if(stat != default(ModifiableStat))
			return stat.ModifiedValue;
		
		return -1;
	}

	public ModifiableStat GetStatByName(string name)
	{
		return ModifiableStats.FirstOrDefault(s => s.Name == name);
	}

	public IEnumerable<InventoryItem> RollForLoot()
	{
		List<InventoryItem> loot = new List<InventoryItem>();
		for(int i = 0; i < Drops.Count; i++)
		{
			if(Drops[i].RollForThisDrop())
				loot.Add(Drops[i].Item);
		}

		return loot;
	}

	#endregion Methods
}
