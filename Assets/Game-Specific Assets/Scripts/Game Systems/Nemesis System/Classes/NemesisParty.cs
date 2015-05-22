using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class NemesisParty
{
    #region Variables / Properties

    public string NemesisPartyName;
    public string CurrentLocation;
    public List<NemesisEnemy> NemesisPartyMembers;
    public NemesisProgression NemesisStrategy;

    public NemesisObjective CurrentObjective
    {
        get { return NemesisStrategy.CurrentObjective; }
    }

    #endregion Variables / Properties

    #region Methods

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["NemesisPartyName"] = new JSONData(NemesisPartyName);
        state["CurrentLocation"] = new JSONData(CurrentLocation);

        state["NemesisPartyMembers"] = new JSONArray();
        for (int i = 0; i < NemesisPartyMembers.Count; i++)
        {
            NemesisEnemy current = NemesisPartyMembers[i];
            state["NemesisPartyMembers"].Add(current.ExportState());
        }

        state["NemesisProgression"] = NemesisStrategy.ExportState();

        return state;
    }

    public NemesisContingency ProceedToPlanOutcome(NemesisPlanOutcome outcome)
    {
        var contingencyResult = NemesisStrategy.ProceedToPlanOutcome(outcome);
        return contingencyResult;
    }

    #endregion Methods
}
