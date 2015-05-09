using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ConversationTextEvents : ConversationEventBase
{
    #region Variables / Properties

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("EndConversation", EndConversation);
    }

    #endregion Hooks

    #region Methods

    private IEnumerator EndConversation(List<string> args)
    {
        _controller.EndConversation();

        yield return null;
    }

    #endregion Methods
}
