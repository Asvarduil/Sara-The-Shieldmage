using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class TextThread : IJsonSavable
{
	#region Variables / Properties

	public string Name;
	public bool IsDefaultThread = false;
	public SequenceRange SequenceRange;
	public List<TextContent> TextContent;

	#endregion Variables / Properties

    #region Constructors

    public TextThread()
    {
    }

    #endregion Constructors

    #region Methods

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        IsDefaultThread = state["IsDefaultThread"].AsBool;
        SequenceRange = new SequenceRange(state["SequenceRange"].AsObject);
        TextContent = state["TextContent"].AsArray.UnfoldJsonArray<TextContent>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["IsDefaultThread"] = new JSONData(IsDefaultThread);
        state["SequenceRange"] = SequenceRange.ExportState();
        state["TextContent"] = TextContent.FoldList();

        return state;
    }

    #endregion Methods
}
