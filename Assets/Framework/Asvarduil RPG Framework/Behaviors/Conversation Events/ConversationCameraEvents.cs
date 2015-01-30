using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationCameraEvents : DebuggableBehavior
{
    #region Variables / Properties

    private Fader Fader
    {
        get { return GameObject.FindObjectOfType<Fader>(); }
    }

    #endregion Variables / Properties

    #region Methods

    public void FadeOut(List<string> args)
    {
        float fadeRate = Fader.FadeRate;
        if(args != null
           && args.Count == 1)
        {
            fadeRate = Convert.ToSingle(args[0]);
        }

        StartCoroutine(ActuallyFadeOut(fadeRate));
    }

    private IEnumerator ActuallyFadeOut(float fadeRate)
    {
        Fader.FadeOut(fadeRate);

        while (Fader.ScreenShown)
            yield return 0;
    }

    public void FadeIn(List<string> args)
    {
        float fadeRate = Fader.FadeRate;
        if (args != null
           && args.Count == 1)
        {
            fadeRate = Convert.ToSingle(args[0]);
        }

        StartCoroutine(ActuallyFadeIn(fadeRate));
    }

    private IEnumerator ActuallyFadeIn(float fadeRate)
    {
        Fader.FadeIn(fadeRate);

        while (Fader.ScreenHidden)
            yield return 0;
    }

    #endregion Methods
}
