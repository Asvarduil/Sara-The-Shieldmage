using System;
using System.Linq;
using System.Collections.Generic;

public enum AbilityResourceUsageType
{
	Immediate,
	Channeled
}

[Serializable]
public class Ability : INamed
{
	#region Variables / Properties

	public string Name;
	public string Description;
	public bool Available;
	public float AtbCost;
	public AbilityResourceUsageType ResourceUse;

	public List<AbilityEffect> Effects;

	public string PresentableName
	{
		get { return Name; }
	}

	public bool IsAvailable
	{
		get { return Available; }
	}

	#endregion Variables / Properties
}
