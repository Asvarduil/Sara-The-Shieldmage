using UnityEngine;
using System.Collections;

public class MemberStatPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilBox StatViewBox;
	
	public AsvarduilImage AttackIcon;
	public AsvarduilLabel AttackLabel;
	
	public AsvarduilImage MagicIcon;
	public AsvarduilLabel MagicLabel;
	
	public AsvarduilImage HealthIcon;
	public AsvarduilLabel HealthLabel;
	
	public AsvarduilImage MaxAtbIcon;
	public AsvarduilLabel MaxAtbLabel;
	
	public AsvarduilImage AtbSpeedIcon;
	public AsvarduilLabel AtbSpeedLabel;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();
	}

	#endregion Engine Hooks

	#region Methods

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		StatViewBox.TargetTint.a = opacity;
		
		HealthIcon.TargetTint.a = opacity;
		HealthLabel.TargetTint.a = opacity;
		
		MagicIcon.TargetTint.a = opacity;
		AttackIcon.TargetTint.a = opacity;
		MaxAtbIcon.TargetTint.a = opacity;
		AtbSpeedIcon.TargetTint.a = opacity;
		
		MagicIcon.TargetTint.a = opacity;
		AttackIcon.TargetTint.a = opacity;
		MaxAtbIcon.TargetTint.a = opacity;
		AtbSpeedIcon.TargetTint.a = opacity;
		MagicLabel.TargetTint.a = opacity;
		AttackLabel.TargetTint.a = opacity;
		MaxAtbLabel.TargetTint.a = opacity;
		AtbSpeedLabel.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		StatViewBox.DrawMe();
		
		HealthIcon.DrawMe();
		HealthLabel.DrawMe();
		
		MagicIcon.DrawMe();
		AttackIcon.DrawMe();
		MaxAtbIcon.DrawMe();
		AtbSpeedIcon.DrawMe();
		MagicLabel.DrawMe();
		AttackLabel.DrawMe();
		MaxAtbLabel.DrawMe();
		AtbSpeedLabel.DrawMe();
	}

	public override void Tween()
	{
		StatViewBox.Tween();
		
		HealthIcon.Tween();
		HealthLabel.Tween();
		
		MagicIcon.Tween();
		AttackIcon.Tween();
		MaxAtbIcon.Tween();
		AtbSpeedIcon.Tween();
		MagicLabel.Tween();
		AttackLabel.Tween();
		MaxAtbLabel.Tween();
		AtbSpeedLabel.Tween();
	}

	public void ReloadCharacterStats(PlayableCharacter character)
	{
		int hp = character.GetModifiedStatValue("HP");
		int maxHP = character.GetModifiedStatValue("Max HP");
		HealthLabel.Text = string.Format("{0}/{1}", hp, maxHP);
		
		MagicLabel.Text = character.GetModifiedStatValue("Magic").ToString();
		AttackLabel.Text = character.GetModifiedStatValue("Attack").ToString();
		MaxAtbLabel.Text = character.GetModifiedStatValue("Max ATB").ToString();
		AtbSpeedLabel.Text = character.GetModifiedStatValue("ATB Speed").ToString();
	}

	#endregion Methods
}
