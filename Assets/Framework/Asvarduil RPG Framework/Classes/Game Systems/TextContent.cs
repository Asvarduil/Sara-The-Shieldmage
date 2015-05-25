using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class TextContent : IJsonSavable
{
	#region Variables / Properties

	public string Speaker;
	public string Dialogue;
    public List<DialogueOption> Options;
	public List<GameEvent> DialogueEvents;
    public List<GameEvent> SequentialEvents;

	#endregion Variables / Properties

    #region Constructor

    public TextContent()
    {
    }

    public TextContent(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructor

    #region Methods

    public void ImportState(JSONClass state)
    {
        Speaker = state["Speaker"];
        Dialogue = state["Dialogue"];

        Options = state["Options"].AsArray.UnfoldJsonArray<DialogueOption>();
        DialogueEvents = state["DialogueEvents"].AsArray.UnfoldJsonArray<GameEvent>();
        SequentialEvents = state["SequentialEvents"].AsArray.UnfoldJsonArray<GameEvent>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Speaker"] = new JSONData(Speaker);
        state["Dialogue"] = new JSONData(Dialogue);
        state["Options"] = Options.FoldList();
        state["DialogueEvents"] = DialogueEvents.FoldList();
        state["SequentialEvents"] = SequentialEvents.FoldList();

        return state;
    }

    #endregion Methods
}
