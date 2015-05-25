using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityRandom = UnityEngine.Random;

public class ConversationCameraEvents : ConversationEventBase
{
    #region Variables / Properties

    private RPGCamera Camera
    {
        get { return GameObject.FindObjectOfType<RPGCamera>(); }
    }

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
        _controller.RegisterEventHook("QuakeCamera", QuakeCamera);
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

    public IEnumerator QuakeCamera(List<string> args)
    {
        if (args == null || args.Count < 2)
            throw new ArgumentException("QuakeCamera requires the duration of the quake as well as the magnitude!");

        float quakeDuration = Convert.ToSingle(args[0]);
        float quakeMagnitude = Convert.ToSingle(args[1]);

        Vector3 originalOffset = Camera.offset;

        float quakeStart = Time.time;
        while(Time.time < quakeStart + quakeDuration)
        {
            Vector3 cameraOffset = new Vector3();
            cameraOffset.x = UnityRandom.Range(0, quakeMagnitude);
            cameraOffset.y = UnityRandom.Range(0, quakeMagnitude);
            cameraOffset.z = UnityRandom.Range(0, quakeMagnitude);

            Camera.offset = cameraOffset;

            yield return null;
        }

        Camera.offset = originalOffset;
    }

    #endregion Methods
}
