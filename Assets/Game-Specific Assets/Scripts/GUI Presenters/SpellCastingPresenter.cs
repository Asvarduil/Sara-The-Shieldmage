using UnityEngine;
using System.Collections;

public class SpellCastingPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilImage Background;
	public AsvarduilImage SpellIcon;
	public AsvarduilLabel SpellLabel;

	#endregion Variables / Properties

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);
		
		Background.TargetTint.a = opacity;
		SpellIcon.TargetTint.a = opacity;
		SpellLabel.TargetTint.a = opacity;
	}
	
	public override void DrawMe()
	{
		Background.DrawMe();
		SpellIcon.DrawMe();
		SpellLabel.DrawMe();
	}
	
	public override void Tween()
	{
		Background.Tween();
		SpellIcon.Tween();
		SpellLabel.Tween();
	}

	#endregion Hooks

	#region Methods

	public void UpdateInterface(Spell spell)
	{
		SpellIcon.Image = spell.Thumbnail;
		SpellLabel.Text = spell.Name;
	}

	#endregion Methods
}
