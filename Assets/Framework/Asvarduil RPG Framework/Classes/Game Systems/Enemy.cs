using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class Enemy : CombatEntity, ICloneable
{
	#region Variables / Properties

    public Func<List<Ability>, Ability> DetermineAction
    {
        get 
        {
            if (BattlePiece == null)
                throw new InvalidOperationException("There is no BattleEntity script on the Enemy's prefab.");

            if (BattlePiece.AI == null)
                throw new InvalidOperationException("The AI object did not get initialized on the Battle Piece.");

            return (abilities) => BattlePiece.AI.DetermineAction(abilities); 
        }
    }

    public Func<List<CombatEntity>, List<CombatEntity>> DetermineTarget
    {
        get 
        {
            if (BattlePiece == null)
                throw new InvalidOperationException("There is no BattleEntity script on the Enemy's prefab.");

            if (BattlePiece.AI == null)
                throw new InvalidOperationException("The AI object did not get initialized on the Battle Piece.");

            return (target) => BattlePiece.AI.DetermineTarget(target); 
        }
    }

	public List<ItemDrop> Drops;

	#endregion Variables / Properties

	#region Methods

    public virtual object Clone()
    {
        var clone = new Enemy
        {
            Name = this.Name,
            BattlePrefab = this.BattlePrefab,
            Health = new HealthSystem(),
            ModifiableStats = new List<ModifiableStat>(),
            AbilityNames = new List<string>(),
            Abilities = new List<Ability>(),
            CounterAttackName = this.CounterAttackName,
            ActiveEffects = new List<AbilityEffect>(),
            Drops = new List<ItemDrop>()
        };

        // Map Health System...
        clone.Health.HP = Health.HP;
        clone.Health.MaxHP = Health.MaxHP;
        clone.Health.RegenRate = Health.RegenRate;
        clone.Health.RegenAmount = Health.RegenAmount;

        // Map Modifiable Stats...
        for (int i = 0; i < ModifiableStats.Count; i++)
        {
            var sourceStat = ModifiableStats[i];
            var clonedStat = sourceStat.Clone() as ModifiableStat;

            clone.ModifiableStats.Add(clonedStat);
        }

        // Map Abilities
        for (int i = 0; i < AbilityNames.Count; i++ )
        {
            var sourceName = AbilityNames[i];
            clone.AbilityNames.Add(sourceName);
        }

        for (int i = 0; i < Abilities.Count; i++)
        {
            var sourceAbility = Abilities[i];
            var clonedAbility = sourceAbility.Clone() as Ability;

            clone.Abilities.Add(clonedAbility);
        }

        // Map existing buffs.
        for (int i = 0; i < ActiveEffects.Count; i++)
        {
            var sourceBuff = ActiveEffects[i];
            var clonedBuff = sourceBuff.Clone() as AbilityEffect;

            clone.ActiveEffects.Add(clonedBuff);
        }

        // Map drop table...
        for (int i = 0; i < Drops.Count; i++)
        {
            var sourceDrop = Drops[i];
            var clonedDrop = sourceDrop.Clone() as ItemDrop;

            clone.Drops.Add(clonedDrop);
        }

        return clone;
    }

    public IEnumerable<InventoryItem> RollForLoot()
    {
        List<InventoryItem> loot = new List<InventoryItem>();
        for (int i = 0; i < Drops.Count; i++)
        {
            var drop = Drops[i];
            if (drop.RollForThisDrop())
            {
                var item = drop.Item.Clone() as InventoryItem;
                item.Quantity = drop.RollForQuantity();
                loot.Add(item);
            }
        }

        return loot;
    }

	#endregion Methods
}
