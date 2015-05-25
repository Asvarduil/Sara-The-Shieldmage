using System;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class SequenceRange : IJsonSavable
{
    #region Variables / Properties

    public string Name;
	public int MinCounter;
	public int MaxCounter;

    #endregion Variables / Properties

    #region Constructor

    public SequenceRange()
    {
    }

    public SequenceRange(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructor

    #region Methods

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        MinCounter = state["MinCounter"].AsInt;
        MaxCounter = state["MaxCounter"].AsInt;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["MinCounter"] = new JSONData(MinCounter);
        state["MaxCounter"] = new JSONData(MaxCounter);

        return state;
    }

    #endregion Methods
}
