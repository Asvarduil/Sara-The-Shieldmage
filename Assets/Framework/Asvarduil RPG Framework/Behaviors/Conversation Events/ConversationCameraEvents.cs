using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationCameraEvents : ConversationEventBase
{
    #region Variables / Properties

    private Fader Fader
    {
        get { return GameObject.FindObjectOfType<Fader>(); }
    }

    #endregion Variables / Properties

    #region Hooks

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("FadeOut", FadeOut);
        _controller.RegisterEventHook("FadeIn", FadeIn);
    }

    #endregion Hooks

    #region Methods

    public IEnumerator FadeOut(List<string> args)
    {
        float fadeRate = Fader.FadeRate;
        if(args != null
           && args.Count == 1)
        {
            fadeRate = Convert.ToSingle(args[0]);
        }

        Fader.FadeOut(fadeRate);

        while (Fader.ScreenShown)
            yield return 0;
    }

    public IEnumerator FadeIn(List<string> args)
    {
        float fadeRate = Fader.FadeRate;
        if (args != null
           && args.Count == 1)
        {
            fadeRate = Convert.ToSingle(args[0]);
        }

        Fader.FadeIn(fadeRate);

        while (Fader.ScreenHidden)
            yield return 0;
    }

    #endregion Methods
}
