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
    public List<GameEvent> Events;

    #endregion Variables / Properties

    #region Methods

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["State"] = new JSONData(State.ToString());
        state["NextObjectiveId"] = new JSONData(NextObjectiveId);
        state["Events"] = new JSONArray();

        for (int i = 0; i < Events.Count; i++)
        {
            GameEvent current = Events[i];

            JSONClass savedEvent = new JSONClass();
            savedEvent["Event"] = new JSONData(current.Event);
            savedEvent["EventArgs"] = new JSONArray();

            for(int j = 0; j < current.EventArgs.Count; j++)
            {
                string currentArg = current.EventArgs[j];

                JSONData arg = new JSONData(currentArg);
                savedEvent["EventArgs"].Add(arg);
            }
        }

        return state;
    }

    #endregion Methods
}
