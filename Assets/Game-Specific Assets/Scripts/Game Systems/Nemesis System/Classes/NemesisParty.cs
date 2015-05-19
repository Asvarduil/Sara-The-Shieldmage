using System;
using System.Collections.Generic;

[Serializable]
public class NemesisParty
{
    #region Variables / Properties

    public string NemesisPartyName;
    public List<NemesisEnemy> NemesisPartyMembers;
    public NemesisProgression NemesisStrategy;

    #endregion Variables / Properties

    #region Methods

    public NemesisObjective ProceedToPlanOutcome(NemesisPlanOutcome outcome)
    {
        var newObjective = NemesisStrategy.ProceedToPlanOutcome(outcome);
        return newObjective;
    }

    #endregion Methods
}
