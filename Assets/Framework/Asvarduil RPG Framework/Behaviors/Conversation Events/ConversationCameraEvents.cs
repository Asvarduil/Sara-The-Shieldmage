using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationCameraEvents : DebuggableBehavior
{
    #region Variables / Properties

    private Fader _fader;

    #endregion Variables / Properties

    #region Methods

    public void Start()
    {
        _fader = GameObject.FindObjectOfType<Fader>();
    }

    public void FadeOut(List<string> args)
    {
        float fadeRate = _fader.FadeRate;
        if(args != null
           && args.Count == 1)
        {
            fadeRate = Convert.ToSingle(args[0]);
        }

        StartCoroutine(ActuallyFadeOut(fadeRate));
    }

    private IEnumerator ActuallyFadeOut(float fadeRate)
    {
        _fader.FadeOut(fadeRate);

        while (_fader.ScreenShown)
            yield return 0;
    }

    public void FadeIn(List<string> args)
    {
        float fadeRate = _fader.FadeRate;
        if (args != null
           && args.Count == 1)
        {
            fadeRate = Convert.ToSingle(args[0]);
        }

        StartCoroutine(ActuallyFadeIn(fadeRate));
    }

    private IEnumerator ActuallyFadeIn(float fadeRate)
    {
        _fader.FadeIn(fadeRate);

        while (_fader.ScreenHidden)
            yield return 0;
    }

    #endregion Methods
}
