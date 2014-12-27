using UnityEngine;
using System.Collections.Generic;

public class TargetingPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilBox Background;
	public AsvarduilLabel PromptText;
	public List<AsvarduilButton> TargetButtons;

	private BattleReferee _referee;
	private List<CombatEntity> _entities;

	#endregion Variables / Properties

	#region Hooks

	public override void Start()
	{
		base.Start();

		_referee = BattleReferee.Instance;
	}

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		Background.TargetTint.a = opacity;
		PromptText.TargetTint.a = opacity;

		AsvarduilButton button;
		for(int i = 0; i < TargetButtons.Count; i++)
		{
			button = TargetButtons[i];
			button.TargetTint.a = opacity;
		}
	}

	public override void DrawMe()
	{
		if (_entities.IsNullOrEmpty()) 
		{
			DebugMessage("The entity list is null or empty!");
			return;
		}

		Background.DrawMe();
		PromptText.DrawMe();

		AsvarduilButton button;
		for(int i = 0; i < _entities.Count; i++)
		{
			// Show the option if the possible target exist.
			if(_entities[i] == null)
				continue;

			// Don't show the option for dead targets.
			if(_entities[i].HealthSystem.IsDead)
				continue;

			button = TargetButtons[i];
			if(button.IsClicked())
			{
				_maestro.PlayOneShot(ButtonSound);

				SetVisibility(false);
				var target = _entities[i];
				_referee.ApplyTargetToCommand(target);
			}
		}
	}

	public override void Tween()
	{
		Background.Tween();
		PromptText.Tween();
		
		AsvarduilButton button;
		for(int i = 0; i < TargetButtons.Count; i++)
		{
			button = TargetButtons[i];
			button.Tween();
		}
	}

	#endregion Hooks

	#region Methods

	public void Prompt(List<CombatEntity> entities, string targetPrompt = "")
	{
		PromptText.Text = targetPrompt;
		_entities = entities;

		// Set up buttons for all live enemies...
		AsvarduilButton button;
		for(int i = 0; i < entities.Count; i++)
		{
			button = TargetButtons[i];
			button.ButtonText = entities[i].EntityName;
		}

		// Hide any unused buttons!
		for (int i = TargetButtons.Count - 1; i > entities.Count - 1; i--) 
		{
			button = TargetButtons[i];
			button.ButtonText = string.Empty;
		}

		SetVisibility(true);
	}

	#endregion Methods
}
