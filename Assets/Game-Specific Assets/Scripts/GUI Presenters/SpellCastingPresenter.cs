using UnityEngine;
using System.Collections;

public class SpellCastingPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilImage Background;
	public AsvarduilImage SpellIcon;

	#endregion Variables / Properties

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);
		
		Background.TargetTint.a = opacity;
		SpellIcon.TargetTint.a = opacity;
	}
	
	public override void DrawMe()
	{
		Background.DrawMe();
		SpellIcon.DrawMe();
	}
	
	public override void Tween()
	{
		Background.Tween();
		SpellIcon.Tween();
	}

	#endregion Hooks

	#region Methods

	public void UpdateInterface(Spell spell)
	{
		SpellIcon.Image = spell.Thumbnail;
	}

	#endregion Methods
}
