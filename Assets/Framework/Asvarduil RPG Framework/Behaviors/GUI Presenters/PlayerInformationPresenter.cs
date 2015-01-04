using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleCharacterInfo
{
    #region Variables / Properties

    public AsvarduilImage Background;
    public AsvarduilImage Portrait;
    public AsvarduilImage Overlay;
    public HealthBitPresenter HealthBits;
    public ATBBitPresenter ATBBits;

    public CombatEntity Character { get; private set; }

    #endregion Variables / Properties

    #region Hooks

    public void SetOpacity(float opacity)
    {
        Background.TargetTint.a = opacity;
        Portrait.TargetTint.a = opacity;
        Overlay.TargetTint.a = opacity;
        HealthBits.ResourceImage.TargetTint.a = opacity;
        ATBBits.ResourceImage.TargetTint.a = opacity;
    }

    public void DrawMe()
    {
        Background.DrawMe();
        Portrait.DrawMe();
        Overlay.DrawMe();
        HealthBits.DrawMe();
        ATBBits.DrawMe();
    }

    public void Tween()
    {
        Background.Tween();
        Portrait.Tween();
        Overlay.Tween();
        HealthBits.Tween();
        ATBBits.Tween();
    }

    #endregion Hooks

    #region Methods

    public void BindCharacter(CombatEntity character)
    {
        Character = character;
    }

    public void UpdateATB()
    {
        int ATB = Character.GetStatByName("ATB").Value;
        int MaxATB = Character.GetStatByName("Max ATB").Value;
        ATBBits.UpdateImage(ATB, MaxATB);
    }

    public void UpdateHealth()
    {
        HealthBits.UpdateImage(Character.Health.HP, Character.Health.MaxHP);
    }

    #endregion Methods
}

public class PlayerInformationPresenter : PresenterBase
{
	#region Variables / Properties

    public List<BattleCharacterInfo> BattleCharacterPresenters = new List<BattleCharacterInfo>();

	#endregion Variables / Properties

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

        for(int i = 0; i < BattleCharacterPresenters.Count; i++)
        {
            BattleCharacterPresenters[i].SetOpacity(opacity);
        }
	}

	public override void DrawMe()
	{
		for (int i = 0; i < BattleCharacterPresenters.Count; i++)
        {
            BattleCharacterPresenters[i].DrawMe();
        }
	}

	public override void Tween()
	{
        for (int i = 0; i < BattleCharacterPresenters.Count; i++)
        {
            BattleCharacterPresenters[i].Tween();
        }
	}

	#endregion Hooks

	#region Methods

    public void BindCharacterDisplay(CombatEntity entity, int displayId)
    {
        var presenter = BattleCharacterPresenters[displayId];
        presenter.BindCharacter(entity);
        presenter.UpdateATB();
        presenter.UpdateHealth();
    }

    public void UpdateHealth(CombatEntity character)
    {
        var presenter = BattleCharacterPresenters.FirstOrDefault(p => p.Character.Name == character.Name);
        if (presenter == default(BattleCharacterInfo))
            return;

        //DebugMessage("Updated " + presenter.Character.Name + "'s Health Presenter!");
        DebugMessage(presenter.Character.Name + "'s HP is now: " + presenter.Character.Health.HP + "/" + presenter.Character.Health.MaxHP);
        presenter.UpdateHealth();
    }

    public void UpdateATB(int displayId)
    {
        BattleCharacterPresenters[displayId].UpdateATB();
    }

    public void UpdateATB(CombatEntity character)
    {
        var presenter = BattleCharacterPresenters.FirstOrDefault(p => p.Character == character);
        if (presenter == default(BattleCharacterInfo))
            return;

        presenter.UpdateATB();
    }

	#endregion Methods
}
