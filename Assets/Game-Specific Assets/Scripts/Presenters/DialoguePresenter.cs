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

        ExecuteImmediateEvents(content.DialogueEvents);
        StartCoroutine(ExecuteSequentialEvents(content.SequentialEvents));
    }

    #endregion Hooks

    #region Methods

    private void ExecuteImmediateEvents(List<DialogueEvent> events)
    {
        if (events.Count == 0)
            return;

        DebugMessage("Executing immediate events; processing " + events.Count + " events.");
        for (int i = 0; i < events.Count; i++)
        {
            DialogueEvent dialogueEvent = events[i];

            DebugMessage("Attempting to execute event: " + dialogueEvent.MessageName);
            StartCoroutine(_controller.ExecuteDialogueEvent(dialogueEvent.MessageName, dialogueEvent.Args));
        }
    }

    private IEnumerator ExecuteSequentialEvents(List<DialogueEvent> events)
    {
        if (events.Count == 0)
            yield break;

        DebugMessage("Executing sequential events; processing " + events.Count + " events.");
        for (int i = 0; i < events.Count; i++)
        {
            DialogueEvent current = events[i];
            yield return StartCoroutine(_controller.ExecuteDialogueEvent(current.MessageName, current.Args));
        }
    }

    #endregion Methods
}
