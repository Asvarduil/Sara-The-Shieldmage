using System;

[Serializable]
public class ModifiableStat
{
	#region Variables / Properties

	public string Name;
	public int Value;
	private int _originalValue;

	public int FixedModifier = 0;
	public float ScalingModifier = 1.0f;

	public int ModifiedValue
	{
		get { return ((int)(Value * ScalingModifier)) + FixedModifier; }
	}

	#endregion Variables / Properties

	#region Constructor

	public ModifiableStat()
	{
		_originalValue = Value;
	}

	#endregion Constructor

	#region Methods

	public void Reset()
	{
		Value = _originalValue;
	}

	public void Increase(int amount)
	{
		Value += amount;
	}

	public void Reduce(int amount)
	{
		Value -= amount;
		if (Value <= 0)
			Value = 1;
	}

	#endregion Methods
}
