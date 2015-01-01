using System;
using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public class AbilityEffect
{
	#region Variables / Properties

	public string Name;

    public string PowerStat;
    public int EffectFloor;
    public float EffectMultiplier;
    public int MinimumRandomEffect;
    public int MaximumRandomEffect;
    public string TargetStat;

    public bool IsBuff = false;
    public float Duration;
    public float ApplyTime;

    public BattleEntityFeedbackType FeedbackType;
    public string FeedbackValue;

    public int ActualAmount { get; private set; }

    public float ExpireTime
    {
        get { return ApplyTime + Duration; }
    }

    public bool IsExpired
    {
        get { return Time.time >= ExpireTime; }
    }

    public CombatEntity Source { get; private set; }

	#endregion Variables / Properties

    #region Methods

    public void PerformEffectCalculation(CombatEntity source)
    {
        if (source == null)
            throw new ArgumentNullException("The source entity cannot be null!");

        Source = source;

        // If there is no power stat driving the effectiveness of the attack,
        // Use 0 as the contributed power.
        int powerValue = string.IsNullOrEmpty(PowerStat) 
            ? 0 
            : source.GetStatByName(PowerStat).Value;

        // Roll for some randomized variance, and store that as well.
        int randomValue = Random.Range(MinimumRandomEffect, MaximumRandomEffect);

        // Build and return the composite total effect.
        ActualAmount = (int)((EffectFloor + powerValue) * EffectMultiplier) + randomValue;
    }

    #endregion Methods
}
