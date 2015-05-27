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
public class NemesisObjective : ICloneable, IJsonSavable
{
    #region Variables / Properties

    public string Name;
    public int ObjectiveId;
    public NemesisObjectiveDuration Duration;
    public string Description;
    public List<string> MisinformationDescriptions;
    public List<NemesisContingency> Outcomes;

    #endregion Variables / Properties

    #region Constructor

    public NemesisObjective()
    {
    }

    public NemesisObjective(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructor

    #region Methods

    public object Clone()
    {
        var clone = new NemesisObjective
        {
            Name = this.Name,
            ObjectiveId = this.ObjectiveId,
            Duration = this.Duration,
            Description = this.Description,
            MisinformationDescriptions = new List<string>(),
            Outcomes = this.Outcomes.CopyList()
        };

        for (int i = 0; i < MisinformationDescriptions.Count; i++)
        {
            clone.MisinformationDescriptions.Add(MisinformationDescriptions[i]);
        }

        return clone;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        ObjectiveId = Int32.Parse(state["ObjectiveId"]);
        Duration = state["Duration"].ToEnum<NemesisObjectiveDuration>();
        Description = state["Description"];

        MisinformationDescriptions = new List<string>();
        foreach(var lie in state["MisinformationDescriptions"].Childs)
        {
            MisinformationDescriptions.Add(lie);
        }

        Outcomes = state["Outcomes"].AsArray.UnfoldJsonArray<NemesisContingency>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["ObjectiveId"] = new JSONData(ObjectiveId);
        state["Duration"] = new JSONData(Duration.ToString());
        state["Description"] = new JSONData(Description);

        state["MisinformationDescriptions"] = new JSONArray();
        for (int i = 0; i < MisinformationDescriptions.Count; i++)
        {
            string current = MisinformationDescriptions[i];
            state["MisinformationDescriptions"].Add(current);
        }

        state["Outcomes"] = Outcomes.FoldList();

        return state;
    }

    #endregion Methods
}
