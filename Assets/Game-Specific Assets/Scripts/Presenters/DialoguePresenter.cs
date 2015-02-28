using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialoguePresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text SpeakerName;
    public Text SpeakerText;
    public List<Button> DialogueButtons;

    private DialogueController _controller;
    private TextContent _currentContent;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _controller = DialogueController.Instance;
    }

    public void AdvanceThread(int optionIndex)
    {
        _controller.AdvanceThread(_currentContent.Options[optionIndex].TargetID);
    }

    public void PresentText(TextContent content)
    {
        _currentContent = content;

        DebugMessage("Options to be shown: " + _currentContent.Options.Count);

        SpeakerName.text = _currentContent.Speaker;
        SpeakerText.text = _currentContent.Dialogue;

        // Load the contents of the buttons...
        for (int i = 0; i < _currentContent.Options.Count; i++)
        {
            DialogueOption option = _currentContent.Options[i];
            if (string.IsNullOrEmpty(option.Text))
                continue;

            Text buttonText = DialogueButtons[i].GetComponentInChildren<Text>();
            buttonText.text = option.Text;
        }

        // Hide any buttons that have no associated content...
        for (int i = DialogueButtons.Count - 1; i > _currentContent.Options.Count - 1; i--)
        {
            Button current = DialogueButtons[i];
            ActivateButton(current, false);
        }

        // Execute all dialogue events for this piece of content...
        for (int i = 0; i < content.DialogueEvents.Count; i++)
        {
            DialogueEvent dialogueEvent = content.DialogueEvents[i];

            DebugMessage("Firing conversation event: " + dialogueEvent.MessageName + "...");
            _controller.SendMessage(dialogueEvent.MessageName, dialogueEvent.Args, SendMessageOptions.DontRequireReceiver);
        }
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
