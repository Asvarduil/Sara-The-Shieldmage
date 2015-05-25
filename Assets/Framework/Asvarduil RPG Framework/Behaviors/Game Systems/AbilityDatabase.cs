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
        AbilityEffects = parsed["Effects"].AsArray.UnfoldJsonArray<AbilityEffect>();
    }

    private void MapAbilities(JSONNode parsed)
    {
        Abilities = parsed["Abilities"].AsArray.UnfoldJsonArray<Ability>();
    }

    #endregion Data Access Methods
}
