using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationNemesisEvents : ConversationEventBase
{
    #region Variables / Properties

    private NemesisManager _nemesis;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _nemesis = NemesisManager.Instance;

        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("CompleteNemesisPlan", CompleteNemesisPlan);
        _controller.RegisterEventHook("FailNemesisPlan", FailNemesisPlan);
    }

    #endregion Hooks

    #region Methods

    public IEnumerator CompleteNemesisPlan(List<string> args)
    {
        if (args.Count < 1)
            throw new ArgumentException("CompleteNemesisPlan requires the name of the nemesis group for whom to complete their current plan.");

        string nemesisPartyName = args[0];
        var nemesisParty = _nemesis.GetNemesisPartyByName(nemesisPartyName);
        var newObjective = nemesisParty.ProceedToPlanOutcome(NemesisPlanOutcome.Success);

        //_controller.ExecuteDialogueEvent()

        yield return null;
    }

    public IEnumerator FailNemesisPlan(List<string> args)
    {
        if (args.Count < 1)
            throw new ArgumentException("CompleteNemesisPlan requires the name of the nemesis group for whom to fail their current plan.");

        string nemesisPartyName = args[0];
        var nemesisParty = _nemesis.GetNemesisPartyByName(nemesisPartyName);
        var newObjective = nemesisParty.ProceedToPlanOutcome(NemesisPlanOutcome.Failed);

        yield return null;
    }

    #endregion Methods
}
