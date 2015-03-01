using System;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Button InteractButton;

    private Action _onInteract;
    private DialogueController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _controller = DialogueController.Instance;
    }

    public void SetInteractText(string interactText)
    {
        Text buttonText = InteractButton.GetComponentInChildren<Text>();
        buttonText.text = interactText;
    }

    public void ReadyInteractEvent(Action onInteract)
    {
        _onInteract = onInteract;
    }

    public void Interact()
    {
        _onInteract();
        PresentGUI(false);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
