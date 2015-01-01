using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public abstract class CombatEntity
{
    #region Variables / Properties

    public string Name;
    public GameObject BattlePrefab;
    public HealthSystem Health;
    public List<ModifiableStat> ModifiableStats;
    public List<Ability> Abilities;
    public string CounterAttackName;

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

    /// <summary>
    /// Rule-specific shortcut to get the Counter Rate's calculated value.
    /// </summary>
    private int _CounterRate
    {
        get
        {
            ModifiableStat stat = GetStatByName("Counter Rate");
            if (stat == default(ModifiableStat))
                throw new InvalidOperationException("No 'Counter Rate' stat exists on this combat entity!");

            return stat.ModifiedValue;
        }
    }

    /// <summary>
    /// Gets an ability to use as a counterattack based on the given counterattack name.
    /// </summary>
    private Ability CounterAttack
    {
        get { return Abilities.FirstOrDefault(a => a.Name == CounterAttackName); }
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

    public void ClearActiveEffects()
    {
        for(int i = 0; i < ActiveEffects.Count; i++)
        {
            AbilityEffect current = ActiveEffects[i];
            RemoveActiveEffect(current);
        }
    }

    private void CheckForBuffExpiration()
    {
        for (int i = 0; i < ActiveEffects.Count; i++)
        {
            AbilityEffect currentEffect = ActiveEffects[i];

            if (currentEffect.IsExpired)
            {
                RemoveActiveEffect(currentEffect);
            }
        }
    }

    public void PrepareCounterAttack(CombatEntity source)
    {
        Health.OnDamageTaken = () =>
        {
            int roll = Random.Range(1, 100);
            if (roll <= _CounterRate)
                BattleReferee.Instance.UseAbility(CounterAttack, this, source);
        };
    }

    public void AddActiveEffect(AbilityEffect effect)
    {
        if (IsBuffRefreshed(effect))
            return;

        ActiveEffects.Add(effect);
        ApplyAbilityEffect(effect);
    }

    private bool IsBuffRefreshed(AbilityEffect effect)
    {
        if (!ActiveEffects.Any(e => e.Name == effect.Name))
            return false;

        // If an effect already exists on the entity, update the effect apply time so the
        // buff gets refreshed.
        var existingEffect = ActiveEffects.FirstOrDefault(e => e.Name == effect.Name);
        existingEffect.ApplyTime = effect.ApplyTime;
        return true;
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
