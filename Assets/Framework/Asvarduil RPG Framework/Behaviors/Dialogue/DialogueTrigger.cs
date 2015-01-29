using UnityEngine;
using System.Collections;

public class DialogueTrigger : DebuggableBehavior
{
    #region Variables / Properties

    public string AllowedTag = "Player";

    private EntityText _text;
    private ControlManager _controls;
    private DialogueController _controller;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _text = GetComponent<EntityText>();
        _controls = ControlManager.Instance;
        _controller = DialogueController.Instance;
    }

    public void OnTriggerEnter(Collider who)
    {
        if (who.tag != AllowedTag)
            return;

        _controls.RadiateSuspendCommand();
        _controller.PresentEntityText(_text);
    }

    #endregion Hooks
}
