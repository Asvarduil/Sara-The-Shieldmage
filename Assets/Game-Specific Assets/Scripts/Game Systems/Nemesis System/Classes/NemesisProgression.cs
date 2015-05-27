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

    // Delegate for the fitness function when a rethink occurs.
    public Func<List<NemesisObjective>, bool> IsAcceptablePlan;

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
        if (IsAcceptablePlan == null)
            throw new InvalidOperationException("Cannot rethink a step without a fitness function for the plan.");

        var plan = Objectives.CopyList();
        do
        {
            // Generate new replacement step.

            // Mutate the plan.
            var newStep = (NemesisObjective) CurrentObjective.Clone();
        } while (!IsAcceptablePlan(plan));

        Objectives = plan;
    }

    public void RethinkPlan()
    {
        // TODO: Rethink Nemesis plan.
        if (IsAcceptablePlan == null)
            throw new InvalidOperationException("Cannot rethink a plan without a fitness function for the plan.");

        List<NemesisObjective> plan = null;
        do
        {
            // Regenerate plan.
            plan = new List<NemesisObjective>();
            int planStepCount = UnityRandom.Range(4, 8);

            // Mutate the plan.
        } while (!IsAcceptablePlan(plan));

        Objectives = plan;
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
