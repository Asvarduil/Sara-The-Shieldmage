using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ConversationFactionEvents : ConversationEventBase
{
    #region Variables / Properties

    private FactionManager _factions;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _factions = FactionManager.Instance;
        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("RaiseReputationWithFaction", RaiseReputationWithFaction);
        _controller.RegisterEventHook("LowerReputationWithFaction", LowerReputationWithFaction);
    }

    #endregion Hooks

    #region Methods

    private IEnumerator RaiseReputationWithFaction(List<string> args)
    {
        if (args.Count < 2)
            throw new InvalidOperationException("RaiseReputationWithFaction requires the name of the faction and the amount of reputation to gain.");

        string factionName = args[0];
        int reputationGain = Convert.ToInt32(args[1]);

        _factions.RaiseFactionReputation(factionName, reputationGain);

        yield return null;
    }

    private IEnumerator LowerReputationWithFaction(List<string> args)
    {
        if (args.Count < 2)
            throw new InvalidOperationException("RaiseReputationWithFaction requires the name of the faction and the amount of reputation to lose.");

        string factionName = args[0];
        int reputationLoss = Convert.ToInt32(args[1]);

        _factions.LowerFactionReputation(factionName, reputationLoss);

        yield return null;
    }

    #endregion Methods
}
