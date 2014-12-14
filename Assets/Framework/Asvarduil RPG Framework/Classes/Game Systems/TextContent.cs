using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextContent
{
	#region Variables / Properties

	public string Speaker;
	public string Dialogue;
	public List<DialogueEvent> DialogueEvents;
	public List<DialogueOption> Options;

	#endregion Variables / Properties
}
