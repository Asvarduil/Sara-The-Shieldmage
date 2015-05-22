using System;
using System.Collections.Generic;
using SimpleJSON;

using UnityRandom = UnityEngine.Random;

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

    public JSONClass ExportState()
    {
        // TODO: Serialize this object to a JSON blob.
        JSONClass state = new JSONClass();
        
        state["CurrentObjectiveId"] = new JSONData(CurrentObjectiveId);

        state["Objectives"] = new JSONArray();
        for (int i = 0; i < Objectives.Count; i++)
        {
            NemesisObjective current = Objectives[i];
            state["Objectives"].Add(current.ExportState());
        }

        return state;
    }

    public void RethinkCurrentStep()
    {
        // TODO: Rethink current step.
    }

    public void RethinkPlan()
    {
        // TODO: Rethink Nemesis plan.
    }

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
