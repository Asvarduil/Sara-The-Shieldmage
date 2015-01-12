using UnityEngine;
using System.Collections;

public class PausePresenter : PresenterBase 
{
	#region Variables / Properties

	public AsvarduilButton EquipmentButton;
	public AsvarduilButton ItemsButton;
	public AsvarduilButton MagicButton;
    public AsvarduilButton QuestButton;
	public AsvarduilButton SettingsButton;
	public AsvarduilButton SaveButton;
	public AsvarduilButton ResumeButton;

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

		EquipmentButton.TargetTint.a = opacity;
		ItemsButton.TargetTint.a = opacity;
		MagicButton.TargetTint.a = opacity;
        QuestButton.TargetTint.a = opacity;
		SettingsButton.TargetTint.a = opacity;
		SaveButton.TargetTint.a = opacity;
		ResumeButton.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		if(EquipmentButton.IsClicked())
		{
			DebugMessage("The user is viewing stats and equipment...");
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenEquipment();
		}

		if(ItemsButton.IsClicked())
		{
			DebugMessage("The user is viewing inventory...");
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenItems();
		}

		if(MagicButton.IsClicked())
		{
			DebugMessage("The user is viewing available magic...");
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenMagic();
		}

        if(QuestButton.IsClicked())
        {
            DebugMessage("The user is viewing active quests...");
            _maestro.PlayOneShot(ButtonSound);
            _controller.OpenQuests();
        }

		if(ResumeButton.IsClicked())
		{
			DebugMessage("The user is resuming play...");
			_maestro.PlayOneShot(ButtonSound);
			_controller.Unpause();
		}

		if(SaveButton.IsClicked())
		{
			DebugMessage("The user is saving the game...");
			// TODO: Implement state save
		}

		if(SettingsButton.IsClicked())
		{
			DebugMessage("The user is accessing Settings...");
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenSettings();
		}
	}

	public override void Tween()
	{
		ItemsButton.Tween();
		EquipmentButton.Tween();
		MagicButton.Tween();
        QuestButton.Tween();
		SettingsButton.Tween();
		SaveButton.Tween();
		ResumeButton.Tween();
	}

	#endregion Methods
}
