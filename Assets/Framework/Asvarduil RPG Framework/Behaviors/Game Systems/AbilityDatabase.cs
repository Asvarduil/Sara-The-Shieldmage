using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class AbilityDatabase : ManagerBase<AbilityDatabase>
{
    #region Variables / Properties

    public bool TryDownloadingBlob = false;

    public TextAsset LocalBlob;
    public string RemoteBlobUrl;
    public string RawBlob;

    public List<GameObject> AllVisualEffects;
    public List<Ability> Abilities;
    public List<AbilityEffect> AbilityEffects;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        LoadItemsFromJson();
    }

    #endregion Hooks

    #region Data Management Methods

    public GameObject GetVisualEffectByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        return AllVisualEffects.FirstOrDefault(v => v.name == name);
    }

    public AbilityEffect GetAbilityEffectByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        return AbilityEffects.FirstOrDefault(e => e.Name == name);
    }

    public Ability GetAbilityByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        return Abilities.FirstOrDefault(a => a.Name == name);
    }

    #endregion Data Management Methods

    #region Data Access Methods

    public void LoadItemsFromJson()
    {
        if (TryDownloadingBlob)
            StartCoroutine(DownloadBlob());

        if (string.IsNullOrEmpty(RawBlob))
        {
            RawBlob = FetchLocalBlob();

            if (string.IsNullOrEmpty(RawBlob))
                return;
            else
                MapBlob();
        }
        else
        {
            MapBlob();
        }
    }

    private IEnumerator DownloadBlob()
    {
        if (!TryDownloadingBlob)
            yield break;

        WWW dataInterface = new WWW(RemoteBlobUrl);

        while (!dataInterface.isDone)
            yield return 0;

        RawBlob = dataInterface.text;

        // TODO: Check that the blob has not been corrupted.
        //       If it has, set the raw blob to empty.

        dataInterface.Dispose();
    }

    protected string FetchLocalBlob()
    {
        if (LocalBlob == null)
            return string.Empty;

        return LocalBlob.text;
    }

    public void MapBlob()
    {
        JSONNode parsed = JSON.Parse(RawBlob);

        MapAbilityEffects(parsed);
        MapAbilities(parsed);
    }

    private void MapAbilityEffects(JSONNode parsed)
    {
        var effects = parsed["Effects"].AsArray;
        AbilityEffects = new List<AbilityEffect>();
        foreach (var effect in effects.Childs)
        {
            AbilityEffect abilityEffect = new AbilityEffect();

            abilityEffect.Name = effect["Name"];
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
            newAbility.Available = true;
            newAbility.AtbCost = ability["ATBCost"].AsInt;
            newAbility.ActionAnimation = ability["ActionAnimation"];
            newAbility.ReceiptAnimation = ability["ReceiptAnimation"];

            string targetType = ability["TargetType"];
            newAbility.TargetType = (AbilityTargetType)Enum.Parse(typeof(AbilityTargetType), targetType);

            var abilityEffects = ability["AbilityEffects"].AsArray;
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
