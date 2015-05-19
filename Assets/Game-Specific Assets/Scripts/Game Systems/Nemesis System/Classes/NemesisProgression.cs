using System;
using System.Collections.Generic;

[Serializable]
public class NemesisProgression
{
    #region Variables / Properties

    public int CurrentObjectiveId;
    public List<NemesisObjective> Objectives;

    #endregion Variables / Properties

    #region Methods

    public NemesisObjective ProceedToPlanOutcome(NemesisPlanOutcome outcome)
    {
        NemesisObjective current = Objectives[CurrentObjectiveId];
        NemesisObjective newObjective = null;
        for(int i = 0; i < current.Outcomes.Count; i++)
        {
            NemesisContingency contingency = current.Outcomes[i];
            if(contingency.State == outcome)
            {
                CurrentObjectiveId = contingency.NextObjectiveId;
                newObjective = Objectives[CurrentObjectiveId];
                break;
            }
        }

        return newObjective;
    }

    #endregion Methods
}
