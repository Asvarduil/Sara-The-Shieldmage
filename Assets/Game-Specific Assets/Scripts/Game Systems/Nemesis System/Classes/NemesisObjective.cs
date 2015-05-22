using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class NemesisObjective
{
    #region Variables / Properties

    public int ObjectiveId;
    public float DurationToCompletion;
    public string Description;
    public List<string> MisinformationDescriptions;
    public List<NemesisContingency> Outcomes;

    #endregion Variables / Properties

    #region Methods

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["ObjectiveId"] = new JSONData(ObjectiveId);
        state["DurationToCompletion"] = new JSONData(DurationToCompletion);
        state["Description"] = new JSONData(Description);

        state["MisinformationDescriptions"] = new JSONArray();
        for (int i = 0; i < MisinformationDescriptions.Count; i++)
        {
            string current = MisinformationDescriptions[i];
            state["MisinformationDescriptions"].Add(current);
        }

        state["Outcomes"] = new JSONArray();
        for (int i = 0; i < Outcomes.Count; i++)
        {
            NemesisContingency current = Outcomes[i];
            state["Outcomes"].Add(current.ExportState());
        }

        return state;
    }

    #endregion Methods
}
