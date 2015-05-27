using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

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
public class Ability : INamed, ICloneable, IJsonSavable
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

    #region Constructor

    public Ability()
    {
    }

    #endregion Constructor

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

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Description = state["Description"];
        BattleEffect = AbilityDatabase.Instance.GetVisualEffectByName(state["BattleEffect"]);
        Available = true;
        AtbCost = state["ATBCost"].AsInt;
        TargetType = state["TargetType"].ToEnum<AbilityTargetType>();

        var abilityEffects = state["AbilityEffects"];
        Effects = new List<AbilityEffect>();
        foreach (string effect in abilityEffects.Childs)
        {
            AbilityEffect newEffect = AbilityDatabase.Instance.GetAbilityEffectByName(effect).Clone() as AbilityEffect;
            if (newEffect == null)
                throw new InvalidOperationException("There is no ability effect named " + effect);

            Effects.Add(newEffect);
        }
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Description"] = new JSONData(Description);
        state["BattleEffect"] = new JSONData(BattleEffect.name);
        state["ATBCost"] = new JSONData(AtbCost);
        state["TargetType"] = new JSONData(TargetType.ToString());

        state["AbilityEffects"] = new JSONArray();
        for (int i = 0; i < Effects.Count; i++)
        {
            JSONData current = new JSONData(Effects[i].Name);
            state["AbilityEffects"].Add(current);
        }

        return state;
    }

    #endregion Methods
}
