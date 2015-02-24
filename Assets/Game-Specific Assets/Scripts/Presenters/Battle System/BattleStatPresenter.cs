using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class BattleStatPresenter : DebuggableBehavior
{
    #region Variables / Properties

    public List<BattleCharacterStatPresenter> CharacterStatPresenters;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        List<BattleCharacterStatPresenter> statPresenters = GetComponentsInChildren<BattleCharacterStatPresenter>().ToList();
    }

    public void BindCharacterDisplay(CombatEntity entity, int displayId)
    {
        var presenter = CharacterStatPresenters[displayId];
        presenter.PresentGUI(true);
        presenter.BindCharacter(entity);
        presenter.UpdateATB();
        presenter.UpdateHealth();
    }

    public void HideCharacterDisplay(int displayId)
    {
        CharacterStatPresenters[displayId].PresentGUI(false);
    }

    public void UpdateHealth(CombatEntity character)
    {
        var presenter = CharacterStatPresenters.FirstOrDefault(p => p.Character.Name == character.Name);
        if (presenter == default(BattleCharacterStatPresenter))
            return;

        DebugMessage(presenter.Character.Name + "'s HP is now: " + presenter.Character.Health.HP + "/" + presenter.Character.Health.MaxHP);
        presenter.UpdateHealth();
    }

    public void UpdateATB(int displayId)
    {
        CharacterStatPresenters[displayId].UpdateATB();
    }

    public void UpdateATB(CombatEntity character)
    {
        var presenter = CharacterStatPresenters.FirstOrDefault(p => p.Character == character);
        if (presenter == default(BattleCharacterStatPresenter))
            return;

        presenter.UpdateATB();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
