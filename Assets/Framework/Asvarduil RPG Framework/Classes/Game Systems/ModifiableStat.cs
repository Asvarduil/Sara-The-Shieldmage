using System;

[Serializable]
public class ModifiableStat
{
	#region Variables / Properties

	public string Name;
	public int Value;

	public int FixedModifier = 0;
	public float ScalingModifier = 1.0f;

	public int ModifiedValue
	{
		get { return ((int)(Value * ScalingModifier)) + FixedModifier; }
	}

	#endregion Variables / Properties
}
