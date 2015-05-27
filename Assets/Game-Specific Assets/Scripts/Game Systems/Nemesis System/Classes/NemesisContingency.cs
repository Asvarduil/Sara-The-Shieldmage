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
public class NemesisContingency : ICloneable, IJsonSavable
{
    #region Variables / Properties

    public NemesisPlanOutcome State;
    public int NextObjectiveId;
    public List<GameEvent> Events;

    #endregion Variables / Properties

    #region Constructors

    public NemesisContingency()
    {
    }

    public NemesisContingency(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructors

    #region Methods

    public object Clone()
    {
        var clone = new NemesisContingency
        {
            State = this.State,
            NextObjectiveId = this.NextObjectiveId,
            Events = this.Events.CopyList()
        };

        return clone;
    }

    public void ImportState(JSONClass state)
    {
        State = state["State"].ToEnum<NemesisPlanOutcome>();
        NextObjectiveId = Int32.Parse(state["NextObjectiveId"]);
        Events = state["Events"].AsArray.UnfoldJsonArray<GameEvent>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["State"] = new JSONData(State.ToString());
        state["NextObjectiveId"] = new JSONData(NextObjectiveId);
        state["Events"] = Events.FoldList();

        return state;
    }

    #endregion Methods
}
