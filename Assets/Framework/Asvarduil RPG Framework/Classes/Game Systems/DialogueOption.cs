using System;
using SimpleJSON;

[Serializable]
public class DialogueOption : IJsonSavable
{
	#region Variables / Properties

	public string Text;
	public int TargetID;

	#endregion Variables / Properties

    #region Constructors

    public DialogueOption()
    {
    }

    #endregion Constructors

    #region Methods

    public void ImportState(JSONClass state)
    {
        Text = state["Text"];
        TargetID = state["TargetID"].AsInt;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Text"] = new JSONData(Text);
        state["TargetID"] = new JSONData(TargetID);

        return state;
    }

    #endregion Methods
}
