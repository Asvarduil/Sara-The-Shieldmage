using System;
using System.Collections.Generic;
using SimpleJSON;

public enum NemesisObjectiveDuration
{
    Short,
    Medium,
    Long
}

[Serializable]
public class NemesisObjective
{
    #region Variables / Properties

    public string Name;
    public int ObjectiveId;
    public NemesisObjectiveDuration Duration;
    public string Description;
    public List<string> MisinformationDescriptions;
    public List<NemesisContingency> Outcomes;

    #endregion Variables / Properties

    #region Methods

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["ObjectiveId"] = new JSONData(ObjectiveId);
        state["DurationToCompletion"] = new JSONData(Duration.ToString());
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
