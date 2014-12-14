using System;

[Serializable]
public class AbilityEffect
{
	#region Variables / Properties

	public string Name;

	// Source Power
	public string PowerStat;
	public float PowerRatio = 1.0f;
	public float Accuracy = 1.0f;

	public string MultiplierStat;
	public float MultiplierRatio = 1.0f;

	// Things to do to the target...
	public string TargetStat;
	public string StatusEffect;

	#endregion Variables / Properties
}
