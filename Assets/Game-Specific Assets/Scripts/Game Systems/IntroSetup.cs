using System;
using System.Collections.Generic;
using UnityEngine;

public class IntroSetup : DialogueController
{
    #region Variables / Properties

    private bool _hasStartedDialogue = false;

    private EntityText _introText;
    private DialogueController _dialogue;
    private ConversationNPCEvents _npcEvents;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _introText = GetComponent<EntityText>();

        _dialogue = DialogueController.Instance;
        _npcEvents = ConversationNPCEvents.Instance;

        SetupScene();
    }

    private void SetupScene()
    {
        SpawnGuards();
        InitiateIntroConversation();
    }

    //public void Update()
    //{
    //    if (_hasStartedDialogue)
    //        return;

    //    InitiateIntroConversation();
    //}

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

    private void InitiateIntroConversation()
    {
        _dialogue.PresentEntityText(_introText);
        //Destroy(gameObject);
    }

    #endregion Methods
}
