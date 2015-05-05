using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSetup : DialogueController
{
    #region Variables / Properties

    private bool _hasStartedDialogue = false;

    private Fader _fader;
    private EntityText _introText;
    private ControlManager _controls;
    private DialogueController _dialogue;
    private ConversationNPCEvents _npcEvents;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _fader = FindObjectOfType<Fader>();
        _introText = GetComponent<EntityText>();

        _controls = ControlManager.Instance;
        _dialogue = DialogueController.Instance;
        _npcEvents = ConversationNPCEvents.Instance;

        SetupScene();
    }

    private void SetupScene()
    {
        _controls.RadiateSuspendCommand();

        SpawnGuards();
        StartCoroutine(InitiateIntroConversation());
    }

    #endregion Hooks

    #region Methods

    private void SpawnGuards()
    {
        List<List<string>> npcs = new List<List<string>>();

        npcs.Add(new List<string>
        {
            "NPCs/Intro - Guard (Map)",
            "0.0, 1.0, -1.0",
            "Intro - Guard A"
        });

        npcs.Add(new List<string>
        {
            "NPCs/Intro - Guard (Map)",
            "1.0, 1.0, -1.0",
            "Intro - Guard B"
        });

        for (int i = 0; i < npcs.Count; i++)
        {
            List<string> currentNPC = npcs[i];
            _npcEvents.SpawnNPC(currentNPC);
        }
    }

    private IEnumerator InitiateIntroConversation()
    {
        while (_fader.ScreenHidden)
            yield return null;

        _dialogue.PresentEntityText(_introText);
    }

    #endregion Methods
}
