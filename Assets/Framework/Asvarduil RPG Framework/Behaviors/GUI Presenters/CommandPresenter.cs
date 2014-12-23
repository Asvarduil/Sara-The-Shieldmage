using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class CommandPresenter : PresenterBase
{
	#region Enumerations

	private enum CommandPresenterState
	{
		Standby,
		WaitingCommand,
		WaitingTarget,
		CommandReady
	}

	#endregion Enumerations

	#region Variables / Properties

	public AudioClip PromptSound;
	public AsvarduilBox Background;
	public AsvarduilLabel CharacterName;
	public List<AsvarduilButton> Commands;

	private CommandPresenterState _state;
	private Ability _selectedAbility;

	private ICombatEntity _target;
	private PlayableCharacter _character;
	private BattleReferee _referee;

	#endregion Variables / Properties

	#region Hooks

	public override void Start()
	{
		base.Start();

		_referee = GetComponentInParent<BattleReferee>();
	}

	public override void Update()
	{
		base.Update();

		if(_state == CommandPresenterState.CommandReady)
		{
			_state = CommandPresenterState.Standby;
			_referee.UseAbility(_character, _target, _selectedAbility);
		}
	}

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		Background.TargetTint.a = opacity;
		CharacterName.TargetTint.a = opacity;

		AsvarduilButton command;
		for (int i = 0; i < Commands.Count; i++) 
		{
			command = Commands[i];
			command.TargetTint.a = opacity;
		}
	}

	public override void DrawMe()
	{
		GUI.skin = Skin;

		Background.DrawMe();
		CharacterName.DrawMe();

		AsvarduilButton command;

		if (_character == null)
			return;

		if (_character.Abilities.IsNullOrEmpty())
			return;

		for (int i = 0; i < _character.Abilities.Count; i++) 
		{
			command = Commands[i];
			if(command.IsClicked())
			{
				_maestro.PlayOneShot(ButtonSound);

				_selectedAbility = _character.Abilities[i];

				switch(_selectedAbility.TargetType)
				{
					case AbilityTargetType.Self:
						_state = CommandPresenterState.CommandReady;
						_target = _character;	
						break;

					case AbilityTargetType.TargetEnemy:
					case AbilityTargetType.TargetAlly:
						_state = CommandPresenterState.WaitingTarget;
						_referee.PromptForTarget(_selectedAbility.TargetType);
						// TODO: Request target from the Controller.
						break;

					// TODO: Implement target all enemies/allies
					default:
						DebugMessage("Target Type: " + _selectedAbility.TargetType + " not implemented.");
						break;
				}
			}
		}
	}

	public override void Tween()
	{
		Background.Tween();
		CharacterName.Tween();

		AsvarduilButton command;
		for (int i = 0; i < Commands.Count; i++) 
		{
			command = Commands[i];
			command.Tween();
		}
	}

	#endregion Hooks

	#region Methods

	public void Prompt(PlayableCharacter character)
	{
		SetVisibility(true);

		if(PromptSound != null)
			_maestro.PlayOneShot(PromptSound);

		_character = character;
		CharacterName.Text = character.Name;

		Ability ability;
		AsvarduilButton command;
		for (int i = 0; i < character.Abilities.Count; i++) 
		{
			ability = character.Abilities[i];
			if(! ability.IsAvailable)
				continue;

			command = Commands[i];
			command.ButtonText = ability.Name;
		}

		for (int i = Commands.Count - 1; i > character.Abilities.Count - 1; i--)
		{
			command = Commands[i];
			command.ButtonText = string.Empty;
		}
	}

	public void ApplyTarget(ICombatEntity target)
	{
		_target = target;
		_state = CommandPresenterState.CommandReady;
	}

	#endregion Methods
}
