using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class PlayableCharacter
{
	#region Variables / Properties

	public string Name;

	public bool IsUsable;
	public int PartyIndex = 0;
	
	public HealthSystem Health;
	public List<LevelupSystem> LevelableStats;
	public List<ModifiableStat> ModifiableStats;

	public InventoryItem Weapon;
	public InventoryItem Armor;
	public InventoryItem Accessory;

	public List<Ability> Abilities;

	#endregion Variables / Properties

	#region Methods

	public int GetModifiedStatValue(string statName)
	{
		int result = Health.GetStatByName(statName);
		if(result > -1)
			return result;

		LevelupSystem system = GetLevelSystemByName(statName);
		if(system != default(LevelupSystem))
			return system.Level;

		ModifiableStat stat = GetStatByName(statName);
		if(stat != default(ModifiableStat))
			return stat.ModifiedValue;

		return -1;
	}

	public LevelupSystem GetLevelSystemByName(string name)
	{
		return LevelableStats.FirstOrDefault(s => s.Name == name);
	}

	public ModifiableStat GetStatByName(string name)
	{
		return ModifiableStats.FirstOrDefault(s => s.Name == name);
	}

	public void UnequipItem(ItemType slot)
	{
		switch(slot)
		{
			case ItemType.Weapon:
				RemoveBonuses(Weapon);
				break;

			case ItemType.Armor:
				RemoveBonuses(Armor);
				break;

			case ItemType.Accessory:
				RemoveBonuses(Accessory);
				break;

			default:
				throw new Exception("Unexpected item slot: " + slot);
		}
	}

	public void ApplyItem(InventoryItem item)
	{
		ApplyEffects(item);
	}

	public void EquipItem(ItemType slot, InventoryItem equipment)
	{
		switch(slot)
		{
			case ItemType.Weapon:
				Weapon = equipment;
				break;

			case ItemType.Armor:
				Armor = equipment;
				break;

			case ItemType.Accessory:
				Accessory = equipment;
				break;

			default:
				throw new Exception("Unexpected Equipment type: " + slot);
		}

		ApplyBonuses(equipment);
	}

	public void ApplyEffects(InventoryItem item)
	{
		foreach(ItemEffect effect in item.ConsumeEffects)
		{
			AddItemEffect(effect);
		}
	}

	public void ApplyBonuses(InventoryItem item)
	{
		foreach(ItemEffect effect in item.EquipmentEffects)
		{
			AddItemEffect(effect);
		}
	}

	public void RemoveBonuses(InventoryItem item)
	{
		foreach(ItemEffect effect in item.EquipmentEffects)
		{
			RemoveItemEffect(effect);
		}
	}

	public void AddItemEffect(ItemEffect effect)
	{
		bool applied = Health.ApplyItemEffect(effect);
		if(applied)
			return;

		ModifiableStat stat = GetStatByName(effect.TargetStat);
		if(stat != default(ModifiableStat))
		{
			stat.FixedModifier += effect.FixedEffect;
			stat.ScalingModifier += effect.ScalingEffect - 1;
		}
	}

	public void RemoveItemEffect(ItemEffect effect)
	{
		bool applied = Health.RemoveItemEffect(effect);
		if(applied)
			return;
		
		ModifiableStat stat = GetStatByName(effect.TargetStat);
		if(stat != default(ModifiableStat))
		{
			stat.FixedModifier -= effect.FixedEffect;
			stat.ScalingModifier -= effect.ScalingEffect - 1;
		}
	}

	#endregion Methods
}
