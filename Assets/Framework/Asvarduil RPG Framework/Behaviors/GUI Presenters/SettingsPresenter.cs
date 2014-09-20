using UnityEngine;
using System.Collections;

public class SettingsPresenter : PresenterBase
{
	#region Variables / Properties
	
	public AsvarduilButton BackButton;
	
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

		BackButton.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		if(BackButton.IsClicked())
		{
			DebugMessage("The user is returning to the Pause menu...");
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenPauseMenu();
		}
	}

	public override void Tween()
	{
		BackButton.Tween();
	}

	#endregion Methods
}
