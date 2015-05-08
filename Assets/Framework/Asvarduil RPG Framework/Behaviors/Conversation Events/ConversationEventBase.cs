using UnityEngine;
using System.Collections;

public abstract class ConversationEventBase : DebuggableBehavior
{
    #region Variables / Properties

    protected DialogueController _controller;

    #endregion Variables / Properties

    #region Hooks

    public virtual void Start()
    {
        _controller = DialogueController.Instance;
        RegisterEventHooks();
    }

    #endregion Hooks

    #region Methods

    protected abstract void RegisterEventHooks();

    #endregion Methods
}
