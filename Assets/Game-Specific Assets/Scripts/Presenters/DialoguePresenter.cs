using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

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
            Button button = DialogueButtons[i];
            DialogueOption option = _currentContent.Options[i];
            if (string.IsNullOrEmpty(option.Text))
            {
                ActivateButton(button, false);
                continue;
            }

            ActivateButton(button, true);
            Text buttonText = button.GetComponentInChildren<Text>();
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
            _controller.ExecuteDialogueEvent(dialogueEvent.MessageName, dialogueEvent.Args);
        }

        // Execute all sequential dialogue events for this piece of content...
        StartCoroutine(ExecuteSequentialEvents(content.SequentialEvents));
    }

    #endregion Hooks

    #region Methods

    private IEnumerator ExecuteSequentialEvents(List<DialogueEvent> events)
    {
        for (int i = 0; i < events.Count; i++)
        {
            DialogueEvent current = events[i];
            yield return _controller.ExecuteDialogueEvent(current.MessageName, current.Args);
        }
    }

    #endregion Methods
}
