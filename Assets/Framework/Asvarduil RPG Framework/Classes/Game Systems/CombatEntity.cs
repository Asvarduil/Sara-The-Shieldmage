using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public abstract class CombatEntity
{
    #region Variables / Properties

    public string Name;
    public GameObject BattlePrefab;
    public HealthSystem Health;
    public List<ModifiableStat> ModifiableStats;
    public List<Ability> Abilities;

    public List<AbilityEffect> ActiveEffects;

    /// <summary>
    /// A List of abilities that the entity has and is capable of actually using.
    /// </summary>
    public List<Ability> AvailableAbilities
    {
        get { return Abilities.Where(a => a.IsAvailable && a.AtbCost <= _MaxATB).ToList(); }
    }

    /// <summary>
    /// Where on the battle arena to place an effect.
    /// </summary>
    public Vector3 ScenePosition { get; set; }

    /// <summary>
    /// The instance of the battle piece being used in the current battle.
    /// </summary>
    public BattleEntity BattlePiece { get; set; }

    /// <summary>
    /// Rule-specific shortcut to get the Max ATB stat's calculated value.
    /// </summary>
    private int _MaxATB
    {
        get 
        {
            ModifiableStat stat = GetStatByName("Max ATB");
            if (stat == default(ModifiableStat))
                throw new InvalidOperationException("No 'Max ATB' stat exists on this combat entity!");

            return stat.ModifiedValue;
        }
    }

    #endregion Variables / Properties

    #region Methods

    public virtual void OnATBTick()
    {
        Health.Regenerate();
        CheckForBuffExpiration();
    }

    public virtual int GetModifiedStatValue(string statName)
    {
        int result = Health.GetStatByName(statName);
        if (result > -1)
            return result;

        ModifiableStat stat = GetStatByName(statName);
        if (stat != default(ModifiableStat))
            return stat.ModifiedValue;

        return -1;
    }

    private void CheckForBuffExpiration()
    {
        for (int j = 0; j < ActiveEffects.Count; j++)
        {
            AbilityEffect currentEffect = ActiveEffects[j];

            if (currentEffect.IsExpired)
            {
                RemoveActiveEffect(currentEffect);
            }
        }
    }

    public void AddActiveEffect(AbilityEffect effect)
    {
        ActiveEffects.Add(effect);
        ApplyAbilityEffect(effect);
    }

    public void RemoveActiveEffect(AbilityEffect effect)
    {
        ActiveEffects.Remove(effect);
        UndoAbilityEffect(effect);
    }

    public void ApplyAbilityEffect(AbilityEffect effect)
    {
        int amount = effect.ActualAmount;
        ApplyAbilityEffectToStat(effect, amount);
    }

    public void UndoAbilityEffect(AbilityEffect effect)
    {
        int amount = -effect.ActualAmount;
        ApplyAbilityEffectToStat(effect, amount);
    }

    private void ApplyAbilityEffectToStat(AbilityEffect effect, int amount)
    {
        var affectedStat = GetStatByName(effect.TargetStat);
        if (affectedStat == default(ModifiableStat))
        {
            Health.ApplyAbilityEffect(effect, amount);
        }
        else
        {
            if (amount > 0)
                affectedStat.Increase(amount);
            else
                affectedStat.Reduce(amount);
        }
    }

    public ModifiableStat GetStatByName(string name)
    {
        return ModifiableStats.FirstOrDefault(s => s.Name == name);
    }

    #endregion Methods
}
