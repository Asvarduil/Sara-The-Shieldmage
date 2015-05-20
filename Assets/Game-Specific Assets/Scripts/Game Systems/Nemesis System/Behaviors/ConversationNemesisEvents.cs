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

        var contingency = nemesisParty.ProceedToPlanOutcome(NemesisPlanOutcome.Success);
        _controller.ExecuteDialogueEvent(contingency.OutcomeEvent, contingency.OutcomeEventParams);

        yield return null;
    }

    public IEnumerator FailNemesisPlan(List<string> args)
    {
        if (args.Count < 1)
            throw new ArgumentException("CompleteNemesisPlan requires the name of the nemesis group for whom to fail their current plan.");

        string nemesisPartyName = args[0];
        var nemesisParty = _nemesis.GetNemesisPartyByName(nemesisPartyName);
        
        var contingency = nemesisParty.ProceedToPlanOutcome(NemesisPlanOutcome.Failed);
        _controller.ExecuteDialogueEvent(contingency.OutcomeEvent, contingency.OutcomeEventParams);

        yield return null;
    }

    public IEnumerator NextNemesisPlan(List<string> args)
    {
        if (args.Count < 1)
            throw new ArgumentException("NextNemesisPlan requires the name of the nemesis group for whom to fail their current plan.");

        string nemesisPartyName = args[0];
        var nemesisParty = _nemesis.GetNemesisPartyByName(nemesisPartyName);
        
        var contingency = nemesisParty.ProceedToPlanOutcome(NemesisPlanOutcome.NotApplicable);
        _controller.ExecuteDialogueEvent(contingency.OutcomeEvent, contingency.OutcomeEventParams);

        yield return null;
    }

    #endregion Methods
}
