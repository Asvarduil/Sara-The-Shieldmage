using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityTargetType
{
	Self,
	AllEnemy,
	AllAlly,
	All,
	TargetEnemy,
	TargetAlly,
	Targeted
}

[Serializable]
public class Ability : INamed
{
	#region Variables / Properties

	public string Name;
	public string Description;
	public GameObject BattleEffect;
	public bool Available;
	public int AtbCost;
	public AbilityTargetType TargetType;

	public List<AbilityEffect> Effects;

    public string ActionAnimation;
    public string ReceiptAnimation;

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
