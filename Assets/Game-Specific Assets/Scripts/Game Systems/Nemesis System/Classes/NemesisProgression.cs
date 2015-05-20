using System;
using System.Collections.Generic;

[Serializable]
public class NemesisProgression
{
    #region Variables / Properties

    public int CurrentObjectiveId;
    public List<NemesisObjective> Objectives;

    public NemesisObjective CurrentObjective
    {
        get { return Objectives[CurrentObjectiveId]; }
    }

    #endregion Variables / Properties

    #region Methods

    public NemesisContingency ProceedToPlanOutcome(NemesisPlanOutcome outcome)
    {
        NemesisContingency resultContingency = null;
        for(int i = 0; i < CurrentObjective.Outcomes.Count; i++)
        {
            NemesisContingency contingency = CurrentObjective.Outcomes[i];
            if(contingency.State == outcome)
            {
                CurrentObjectiveId = contingency.NextObjectiveId;
                resultContingency = contingency;
                break;
            }
        }

        return resultContingency;
    }

    #endregion Methods
}
