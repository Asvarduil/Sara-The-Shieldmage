using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextContent
{
	#region Variables / Properties

	public string Speaker;
	public string Dialogue;
    public List<DialogueOption> Options;
	public List<DialogueEvent> DialogueEvents;
    public List<DialogueEvent> SequentialEvents;

	#endregion Variables / Properties
}
