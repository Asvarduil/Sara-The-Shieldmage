using System;

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

    public BattleEntityFeedbackType FeedbackType;
    public string FeedbackValue;

	#endregion Variables / Properties

    #region Methods

    public int PerformEffectCalculation(CombatEntity source)
    {
        if (source == null)
            throw new ArgumentNullException("The source entity cannot be null!");

        // If there is no power stat driving the effectiveness of the attack,
        // Use 0 as the contributed power.
        int powerValue = string.IsNullOrEmpty(PowerStat) 
            ? 0 
            : source.GetStatByName(PowerStat).Value;

        // Roll for some randomized variance, and store that as well.
        int randomValue = Random.Range(MinimumRandomEffect, MaximumRandomEffect);

        // Build and return the composite total effect.
        int effect = (int)((EffectFloor + powerValue) * EffectMultiplier) + randomValue;

        return effect;
    }

    #endregion Methods
}
