using System;
using System.Collections.Generic;

[Serializable]
public class NemesisParty
{
    #region Variables / Properties

    public string NemesisPartyName;
    public string CurrentLocation;
    public List<NemesisEnemy> NemesisPartyMembers;
    public NemesisProgression NemesisStrategy;

    public NemesisObjective CurrentObjective
    {
        get { return NemesisStrategy.CurrentObjective; }
    }

    #endregion Variables / Properties

    #region Methods

    public NemesisContingency ProceedToPlanOutcome(NemesisPlanOutcome outcome)
    {
        var contingencyResult = NemesisStrategy.ProceedToPlanOutcome(outcome);
        return contingencyResult;
    }

    #endregion Methods
}
