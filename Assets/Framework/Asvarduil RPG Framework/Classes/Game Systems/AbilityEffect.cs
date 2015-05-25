using System;
using UnityEngine;
using SimpleJSON;

using Random = UnityEngine.Random;

[Serializable]
public class AbilityEffect : ICloneable, IJsonSavable
{
	#region Variables / Properties

	public string Name;
    public bool AffectsUser = false;
    public bool AffectsDeadCharacters = false;

    public string PowerStat;
    public int EffectFloor;
    public float EffectMultiplier;
    public int MinimumRandomEffect;
    public int MaximumRandomEffect;
    public string TargetStat;

    public bool IsBuff = false;
    public float Duration;
    public float ApplyTime;

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

    #region Constructor

    public AbilityEffect()
    {
    }

    #endregion Constructor

    #region Methods

    public object Clone()
    {
        var clone = new AbilityEffect()
        {
            Name = this.Name,
            AffectsDeadCharacters = this.AffectsDeadCharacters,
            AffectsUser = this.AffectsUser,
            PowerStat = this.PowerStat,
            EffectFloor = this.EffectFloor,
            EffectMultiplier = this.EffectMultiplier,
            MinimumRandomEffect = this.MinimumRandomEffect,
            MaximumRandomEffect = this.MaximumRandomEffect,
            TargetStat = this.TargetStat,
            IsBuff = this.IsBuff,
            Duration = this.Duration
        };

        return clone;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        AffectsDeadCharacters = state["AffectsDeadCharacters"].AsBool;
        AffectsUser = state["AffectsUser"].AsBool;
        PowerStat = state["PowerStat"];
        EffectFloor = state["EffectFloor"].AsInt;
        EffectMultiplier = state["EffectMultiplier"].AsFloat;
        MinimumRandomEffect = state["MinimumRandomEffect"].AsInt;
        MaximumRandomEffect = state["MaximumRandomEffect"].AsInt;
        TargetStat = state["TargetStat"];
        IsBuff = state["IsBuff"].AsBool;
        Duration = state["Duration"].AsFloat;
        ApplyTime = 0.0f;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["AffectsDeadCharacters"] = new JSONData(AffectsDeadCharacters);
        state["AffectsUser"] = new JSONData(AffectsUser);
        state["PowerStat"] = new JSONData(PowerStat);
        state["EffectFloor"] = new JSONData(EffectFloor);
        state["EffectMultiplier"] = new JSONData(EffectMultiplier);
        state["MinimumRandomEffect"] = new JSONData(MinimumRandomEffect);
        state["MaximumRandomEffect"] = new JSONData(MaximumRandomEffect);
        state["TargetStat"] = new JSONData(TargetStat);
        state["IsBuff"] = new JSONData(IsBuff);
        state["Duration"] = new JSONData(Duration);

        return state;
    }

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
