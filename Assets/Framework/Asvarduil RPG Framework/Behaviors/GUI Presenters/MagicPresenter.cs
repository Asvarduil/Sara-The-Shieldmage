using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

public class MagicPresenter : PresenterBase 
{
	#region Variables / Properties

	public AsvarduilBox Background;
	public AsvarduilButtonGrid Skills;
	public AsvarduilBox CommandBar;
	public AsvarduilLabel DetailLabel;

	private Ability _currentAbility;

	#endregion Variables / Properties

	#region Engine Hooks

	#endregion Engine Hooks

	#region Methods

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		Background.TargetTint.a = opacity;
		Skills.TargetTint.a = opacity;

		HideCommandBar();
	}

	public override void DrawMe()
	{
		GUI.skin = Skin;

		Background.DrawMe();

		if(Skills.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_currentAbility = (Ability) Skills.SelectedObject;
			LoadAbilityDetails(_currentAbility);
		}

		CommandBar.DrawMe();
		DetailLabel.DrawMe();
	}

	public override void Tween()
	{
		Background.Tween();
		Skills.Tween();
		CommandBar.Tween();
		DetailLabel.Tween();
	}

	public void LoadAbilityDetails(Ability ability)
	{
		StringBuilder builder = new StringBuilder(ability.Name);
		builder.Append(" - ");
		builder.Append(ability.AtbCost);
		builder.Append(" ATB");
		if(ability.ResourceUse == AbilityResourceUsageType.Channeled)
			builder.Append("/second");

		builder.Append(Environment.NewLine);
		builder.Append(ability.Description);

		DetailLabel.Text = builder.ToString();

		CommandBar.TargetTint.a = 1.0f;
		DetailLabel.TargetTint.a = 1.0f;
	}

	public void HideCommandBar()
	{
		CommandBar.TargetTint.a = 0;
		DetailLabel.TargetTint.a = 0;
	}

	public void LoadSkills(List<INamed> items)
	{
		Skills.Refresh(items);
	}

	#endregion Methods
}
