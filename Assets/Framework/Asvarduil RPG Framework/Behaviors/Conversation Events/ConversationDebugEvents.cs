using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationDebugEvents : ConversationEventBase
{
    #region Variables / Properties

    #endregion Variables / Properties

    #region Hooks

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("DebugMessage", DebugMessage);
    }

    #endregion Hooks

    #region Methods

    public IEnumerator DebugMessage(List<string> args)
    {
        if (args.Count == 1)
        {
            DebugMessage(args[0]);
        }
        else if (args.Count == 2)
        {
            LogLevel level = (LogLevel) Enum.Parse(typeof(LogLevel), args[1]);
            DebugMessage(args[0], level);
        }

        yield return null;
    }

    #endregion Methods
}
