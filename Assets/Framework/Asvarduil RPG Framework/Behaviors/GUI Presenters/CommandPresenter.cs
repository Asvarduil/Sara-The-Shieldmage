using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class CommandPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilBox Background;
	public AsvarduilLabel CharacterName;
	public List<AsvarduilButton> Commands;

	private PlayableCharacter _character;
	private BattleReferee _referee;

	#endregion Variables / Properties

	#region Hooks

	public override void Start()
	{
		base.Start();

		_referee = GetComponentInParent<BattleReferee>();
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

		Ability usedAbility;
		AsvarduilButton command;
		for (int i = 0; i < _character.Abilities.Count; i++) 
		{
			command = Commands[i];
			if(command.IsClicked())
			{
				_maestro.PlayOneShot(ButtonSound);

				usedAbility = _character.Abilities[i];
				_referee.UseAbility(_character, usedAbility);
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

		_character = character;
		CharacterName.Text = character.Name;

		Ability ability;
		AsvarduilButton command;
		for (int i = 0; i < character.Abilities.Count; i++) 
		{
			ability = character.Abilities[i];
			command = Commands[i];
			command.ButtonText = ability.Name;
		}

		for (int i = Commands.Count - 1; i > character.Abilities.Count - 1; i--)
		{
			command = Commands[i];
			command.ButtonText = string.Empty;
		}
	}

	#endregion Methods
}
