using System.Collections.Generic;

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

    #endregion Methods
}
