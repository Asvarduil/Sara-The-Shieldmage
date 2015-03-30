using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class AbilityDatabase : DatabaseBase<AbilityDatabase>
{
    #region Variables / Properties

    public List<GameObject> AllVisualEffects;
    public List<Ability> Abilities;
    public List<AbilityEffect> AbilityEffects;

    #endregion Variables / Properties

    #region Data Management Methods

    public void HydrateCombatEntityAbilitiesFromList(CombatEntity entity)
    {
        List<Ability> abilities = new List<Ability>();

        for(int i = 0; i < entity.AbilityNames.Count; i++)
        {
            string currentName = entity.AbilityNames[i];
            Ability ability = GetAbilityByName(currentName).Clone() as Ability;
            abilities.Add(ability);
        }

        entity.Abilities = abilities;
    }

    public GameObject GetVisualEffectByName(string name)
    {
        GameObject result = null;
        for(int i = 0; i < AllVisualEffects.Count; i++)
        {
            GameObject current = AllVisualEffects[i];
            if (current.name != name)
                continue;

            result = current;
            break;
        }

        DebugMessage("Ability Visual Effect " + name + " " + (result == null ? "wasn't" : "was") + " found.");
        return result;
    }

    public AbilityEffect GetAbilityEffectByName(string name)
    {
        DebugMessage("Trying to find ability effect " + name + " out of a repository of " + AbilityEffects.Count + " effects...");

        AbilityEffect result = null;
        for(int i = 0; i < AbilityEffects.Count; i++)
        {
            AbilityEffect current = AbilityEffects[i];
            if (current.Name != name)
                continue;

            result = current;
            break;
        }

        DebugMessage("Ability Effect " + name + " " + (result == null ? "wasn't" : "was") + " found.");
        return result;
    }

    public Ability GetAbilityByName(string name)
    {
        DebugMessage("Trying to find ability " + name + " out of a repository of " + Abilities.Count + " abilities...");

        Ability result = null;
        for (int i = 0; i < Abilities.Count; i++ )
        {
            Ability current = Abilities[i];
            if (current.Name != name)
                continue;

            result = current;
            break;
        }

        DebugMessage("Ability " + name + " " + (result == null ? "wasn't" : "was") + " found.");
        return result;
    }

    #endregion Data Management Methods

    #region Data Access Methods

    protected override void MapBlob()
    {
        JSONNode parsed = JSON.Parse(RawBlob);

        MapAbilityEffects(parsed);
        MapAbilities(parsed);

        IsLoaded = Abilities.Count > 0;
    }

    private void MapAbilityEffects(JSONNode parsed)
    {
        var effects = parsed["Effects"].AsArray;
        AbilityEffects = new List<AbilityEffect>();
        foreach (var effect in effects.Childs)
        {
            AbilityEffect abilityEffect = new AbilityEffect();

            abilityEffect.Name = effect["Name"];
            abilityEffect.AffectsUser = effect["AffectsUser"].AsBool;
            abilityEffect.AffectsDeadCharacters = effect["AffectsDeadCharacters"].AsBool;
            abilityEffect.IsBuff = effect["IsBuff"].AsBool;
            abilityEffect.Duration = effect["Duration"].AsFloat;
            abilityEffect.ApplyTime = 0.0f;

            abilityEffect.PowerStat = effect["PowerStat"];
            abilityEffect.TargetStat = effect["TargetStat"];
            abilityEffect.EffectFloor = effect["EffectFloor"].AsInt;
            abilityEffect.EffectMultiplier = effect["EffectMultiplier"].AsFloat;
            abilityEffect.MinimumRandomEffect = effect["MinimumRandomEffect"].AsInt;
            abilityEffect.MaximumRandomEffect = effect["MaximumRandomEffect"].AsInt;

            AbilityEffects.Add(abilityEffect);
        }
    }

    private void MapAbilities(JSONNode parsed)
    {
        var abilities = parsed["Abilities"].AsArray;
        Abilities = new List<Ability>();
        foreach (var ability in abilities.Childs)
        {
            Ability newAbility = new Ability();

            newAbility.Name = ability["Name"];
            newAbility.Description = ability["Description"];
            newAbility.BattleEffect = GetVisualEffectByName(ability["VisualEffect"]);
            newAbility.BattleText = ability["BattleText"] ?? string.Empty;
            newAbility.Available = true;
            newAbility.AtbCost = ability["ATBCost"].AsInt;
            newAbility.ActionAnimation = ability["ActionAnimation"];
            newAbility.ReceiptAnimation = ability["ReceiptAnimation"];

            string targetType = ability["TargetType"];
            newAbility.TargetType = (AbilityTargetType)Enum.Parse(typeof(AbilityTargetType), targetType);

            var abilityEffects = ability["Effects"].AsArray;
            newAbility.Effects = new List<AbilityEffect>();

            foreach(string effect in abilityEffects.Childs)
            {
                AbilityEffect newEffect = GetAbilityEffectByName(effect).Clone() as AbilityEffect;
                if (newEffect == null)
                    throw new InvalidOperationException("There is no ability effect named " + effect);

                newAbility.Effects.Add(newEffect);
            }
            
            Abilities.Add(newAbility);
        }
    }

    #endregion Data Access Methods
}
