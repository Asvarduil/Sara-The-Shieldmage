using System;
using System.Collections.Generic;
using SimpleJSON;

using UnityRandom = UnityEngine.Random;

[Serializable]
public class NemesisProgression : IJsonSavable
{
    #region Variables / Properties

    public int CurrentObjectiveId;
    public List<NemesisObjective> Objectives;

    public NemesisObjective CurrentObjective
    {
        get { return Objectives[CurrentObjectiveId]; }
    }

    #endregion Variables / Properties

    #region Constructor

    public NemesisProgression()
    {
    }

    public NemesisProgression(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructor

    #region Methods

    public void ImportState(JSONClass state)
    {
        CurrentObjectiveId = Int32.Parse(state["CurrentObjectiveId"]);
        Objectives = state["Objectives"].AsArray.UnfoldJsonArray<NemesisObjective>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();
        
        state["CurrentObjectiveId"] = new JSONData(CurrentObjectiveId);
        state["Objectives"] = Objectives.FoldList();

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
