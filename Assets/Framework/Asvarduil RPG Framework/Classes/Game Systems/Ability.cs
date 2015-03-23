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
public class Ability : INamed, ICloneable
{
	#region Variables / Properties

	public string Name;
	public string Description;
	public GameObject BattleEffect;
    public string BattleText;
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

    #region Methods

    public object Clone()
    {
        var clone = new Ability
        {
            Name = this.Name,
            Description = this.Description,
            BattleEffect = this.BattleEffect,
            Available = this.Available,
            AtbCost = this.AtbCost,
            TargetType = this.TargetType,
            Effects = new List<AbilityEffect>(),
            ActionAnimation = this.ActionAnimation,
            ReceiptAnimation = this.ReceiptAnimation
        };

        for (int i = 0; i < Effects.Count; i++)
        {
            var sourceEffect = Effects[i];
            var clonedEffect = sourceEffect.Clone() as AbilityEffect;

            clone.Effects.Add(clonedEffect);
        }

        return clone;
    }

    #endregion Methods
}
