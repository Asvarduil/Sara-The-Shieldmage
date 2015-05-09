using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSetup : DebuggableBehavior
{
    #region Variables / Properties

    private Fader _fader;
    private EntityText _introText;
    private ControlManager _controls;
    private DialogueController _dialogue;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _fader = FindObjectOfType<Fader>();
        _introText = GetComponent<EntityText>();

        _controls = ControlManager.Instance;
        _dialogue = DialogueController.Instance;

        SetupScene();
    }

    private void SetupScene()
    {
        _controls.RadiateSuspendCommand();
        StartCoroutine(InitiateIntroConversation());
    }

    #endregion Hooks

    #region Methods

    private IEnumerator InitiateIntroConversation()
    {
        while (_fader.ScreenHidden)
            yield return null;

        _dialogue.PresentEntityText(_introText);
    }

    #endregion Methods
}
