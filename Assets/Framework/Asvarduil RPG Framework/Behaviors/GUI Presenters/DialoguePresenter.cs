using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class DialoguePresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilBox Background;
	public AsvarduilLabel SpeakerName;
	public AsvarduilLabel SpeakerText;

	public List<AsvarduilButton> AdvanceButtons;
	public float AdvanceLockout = 0.25f;

	private float _lastAdvance = 0f;
	private ControlManager _control;
	private TextContent _currentContent;
	private DialogueController _dialogueController;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();

		_control = ControlManager.Instance;
		_dialogueController = DialogueController.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		Background.TargetTint.a = opacity;
		SpeakerName.TargetTint.a = opacity;
		SpeakerText.TargetTint.a = opacity;

		foreach(AsvarduilButton button in AdvanceButtons)
		{
			button.TargetTint.a = opacity;
		}
	}

	public override void DrawMe()
	{
		Background.DrawMe();
		SpeakerName.DrawMe();
		SpeakerText.DrawMe();

		if(_currentContent == null)
			return;

		for(int i = 0; i < AdvanceButtons.Count; i++)
	    {
			if(i >= _currentContent.Options.Count)
				return;

			AsvarduilButton button = AdvanceButtons[i];

		    if((button.IsClicked() || _control.GetAxisDown(button.ActivationAxis))
			   && Time.time >= _lastAdvance + AdvanceLockout)
		    {
		        _dialogueController.AdvanceThread(_currentContent.Options[i].TargetID);
		        break;
		    }
		}
	}

	public override void Tween()
	{
		Background.Tween();
		SpeakerName.Tween();
		SpeakerText.Tween();

		foreach(AsvarduilButton button in AdvanceButtons)
		{
		    button.Tween();
		}
	}

	public void PresentText(TextContent content)
	{
		_lastAdvance = Time.time;
		_currentContent = content;

		DebugMessage("Options to be shown: " + _currentContent.Options.Count);

		SpeakerName.Text = _currentContent.Speaker;
		SpeakerText.Text = _currentContent.Dialogue;

		foreach(AsvarduilButton button in AdvanceButtons)
		{
			button.ButtonText = string.Empty;
			button.TargetTint.a = 0;
		}

		int i = 0;
		foreach(DialogueOption option in _currentContent.Options)
		{
			if(string.IsNullOrEmpty(option.Text))
				continue;

		    AdvanceButtons[i].ButtonText = option.Text;
			AdvanceButtons[i].TargetTint.a = 1;
			i++;
		}

		foreach(DialogueEvent dialogueEvent in content.DialogueEvents)
		{
			DebugMessage("Firing conversation event: " + dialogueEvent.MessageName + "...");
			_dialogueController.SendMessage(dialogueEvent.MessageName, dialogueEvent.Args, SendMessageOptions.DontRequireReceiver);
		}
	}

	#endregion Methods	
}
