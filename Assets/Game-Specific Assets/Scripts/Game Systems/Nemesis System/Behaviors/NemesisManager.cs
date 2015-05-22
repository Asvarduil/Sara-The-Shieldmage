using System.Collections.Generic;
using SimpleJSON;

public class NemesisManager : ManagerBase<NemesisManager>
{
    #region Variables / Properties

    public List<NemesisParty> NemesisParties;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public NemesisParty GetNemesisPartyByName(string nemesisPartyName)
    {
        NemesisParty result = null;
        for(int i = 0; i < NemesisParties.Count; i++)
        {
            NemesisParty current = NemesisParties[i];
            if (current.NemesisPartyName == nemesisPartyName)
            {
                result = current;
                break;
            }
        }

        return result;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["NemesisParties"] = new JSONArray();
        for (int i = 0; i < NemesisParties.Count; i++)
        {
            NemesisParty current = NemesisParties[i];
            state["NemesisParties"].Add(current.ExportState());
        }

        return state;
    }

    #endregion Methods
}
