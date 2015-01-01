using UnityEngine;
using System.Collections;

public class PauseInterface : PresenterBase
{
	#region Variables / Properties
	
	public AsvarduilButton PauseButton;

    private ControlManager _controls;
	private PauseController _controller;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();
        _controls = ControlManager.Instance;
		_controller = PauseController.Instance;

		if(_controller == null)
			DebugMessage("Could not find a Pause Controller instance!", LogLevel.Warning);
	}

	#endregion Engine Hooks

	#region Methods

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		PauseButton.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
        if(_controls.GetAxisDown("Pause"))
        {
            _maestro.PlayOneShot(ButtonSound);
            _controller.TogglePause();
        }

        //if(PauseButton.IsClicked())
        //{
        //    _maestro.PlayOneShot(ButtonSound);
        //    _controller.Pause();
        //}
	}

	public override void Tween()
	{
		PauseButton.Tween();
	}

	#endregion Methods
}
