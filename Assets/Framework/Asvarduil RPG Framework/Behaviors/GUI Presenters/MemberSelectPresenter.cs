using UnityEngine;
using System.Collections;

public class MemberSelectPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilBox PartySelectBox;
	public AsvarduilButton NextButton;
	public AsvarduilButton LastButton;
	public AsvarduilLabel MemberNameLabel;

	private PauseController _controller;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();
		_controller = PauseController.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		PartySelectBox.TargetTint.a = opacity;
		MemberNameLabel.TargetTint.a = opacity;
	}

	public void SetButtonVisibility(bool showButtons)
	{
		float opacity = DetermineOpacity(showButtons);

		NextButton.TargetTint.a = opacity;
		LastButton.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		PartySelectBox.DrawMe();
		MemberNameLabel.DrawMe();

		if(NextButton.IsClicked())
		{
			_controller.ChangeMember(1);
		}

		if(LastButton.IsClicked())
		{
			_controller.ChangeMember(-1);
		}
	}

	public override void Tween()
	{
		PartySelectBox.Tween();
		NextButton.Tween();
		LastButton.Tween();
		MemberNameLabel.Tween();
	}

	public void UpdateMemberName(PlayableCharacter character)
	{
		MemberNameLabel.Text = character.Name;
	}

	#endregion Methods
}
