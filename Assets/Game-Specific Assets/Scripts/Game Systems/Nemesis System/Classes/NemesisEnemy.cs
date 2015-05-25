using System;
using SimpleJSON;

[Serializable]
public class NemesisEnemy : Enemy, IJsonSavable
{
    #region Variables / Properties

    #endregion Variables / Properties

    #region Constructor

    public NemesisEnemy()
    {
    }

    #endregion Constructor

    #region Hooks

    #endregion Hooks

    #region Methods

    public void ImportState(JSONClass state)
    {
        // TODO: Implement...
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        // TODO: Implement...

        return state;
    }

    #endregion Methods
}
