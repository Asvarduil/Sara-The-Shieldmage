using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationQuestEvents : ConversationEventBase
{
    #region Variables / Properties

    private SequenceManager _sequence;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _sequence = SequenceManager.Instance;
        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("UpdateQuest", UpdateQuest);
    }

    #endregion Hooks

    #region Messages

    public IEnumerator UpdateQuest(List<string> args)
    {
        if (args.IsNullOrEmpty())
            throw new ArgumentNullException("UpdateQuest requires the name of the quest, the new description, and the phase counter.");

        string questName = args[0];
        string questDescription = args[1];
        int questPhase = Convert.ToInt32(args[2]);

        _sequence.UpdateQuest(questName, questDescription);
        _sequence.UpdateSequence(questName, questPhase);

        yield return null;
    }

    #endregion Messages
}
