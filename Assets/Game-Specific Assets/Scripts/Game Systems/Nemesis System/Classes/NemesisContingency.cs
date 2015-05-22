using System;
using System.Collections.Generic;
using SimpleJSON;

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

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["State"] = new JSONData(State.ToString());
        state["NextObjectiveId"] = new JSONData(NextObjectiveId);
        state["OutcomeEvent"] = new JSONData(OutcomeEvent);

        state["OutcomeEventArgs"] = new JSONArray();
        for (int i = 0; i < OutcomeEventParams.Count; i++)
        {
            string current = OutcomeEventParams[i];
            state["OutcomeEventArgs"] = new JSONData(current);
        }

        return state;
    }

    #endregion Methods
}
