using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class PlayableCharacter : CombatEntity
{
	#region Variables / Properties

	public bool IsUsable;
	public int PartyIndex = 0;

	public InventoryItem Weapon;
	public InventoryItem Armor;
	public InventoryItem Accessory;

	#endregion Variables / Properties

	#region Methods

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
