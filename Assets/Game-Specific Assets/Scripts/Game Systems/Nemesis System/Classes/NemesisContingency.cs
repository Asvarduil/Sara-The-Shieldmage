using System;
using System.Collections.Generic;

public enum NemesisPlanOutcome
{
    NotApplicable,
    Failed,
    Success
}

[Serializable]
public class NemesisContingency
{
    #region Variables / Properties

    public NemesisPlanOutcome State;
    public int NextObjectiveId;
    public string OutcomeEvent;
    public List<string> OutcomeEventParams;

    #endregion Variables / Properties

    #region Methods

    #endregion Methods
}
