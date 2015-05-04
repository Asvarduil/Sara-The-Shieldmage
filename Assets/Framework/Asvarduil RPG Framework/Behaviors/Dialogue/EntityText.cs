using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using SimpleJSON;

public class EntityText : DebuggableBehavior
{
	#region Variables / Properties

	public bool IsTalking = false;
	public string AffectedTag = "Player";

	public List<TextThread> TextThreads;

	private SequenceManager _sequenceManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_sequenceManager = SequenceManager.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public void MapBlob(string blob)
	{
		var parsed = JSON.Parse(blob);
		var threads = parsed["TextThreads"].AsArray;
		
		TextThreads = new List<TextThread>();
		foreach(var thread in threads.Childs)
		{
			TextThread newThread = new TextThread();
			
			newThread.IsDefaultThread = thread["IsDefaultThread"].AsBool;
			newThread.Name = thread["Name"];
			
			var sequenceState = thread["SequenceState"].AsObject;
			SequenceRange newState = new SequenceRange();
			newState.Name = sequenceState["Name"];
			newState.MinCounter = sequenceState["MinCounter"].AsInt;
			newState.MaxCounter = sequenceState["MaxCounter"].AsInt;
			newThread.SequenceRange = newState;
			
			newThread.TextContent = new List<TextContent>();
			var content = thread["TextContent"].AsArray;
			foreach(var contentItem in content.Childs)
			{
				TextContent newTextContent = new TextContent();
				newTextContent.Speaker = contentItem["Speaker"];
				newTextContent.Dialogue = contentItem["Dialogue"];
				newTextContent.DialogueEvents = new List<DialogueEvent>();

				// Map Dialogue Options...
				newTextContent.Options = new List<DialogueOption>();
				var dialogueOptions = contentItem["Options"];
				foreach(var dialogueOption in dialogueOptions.Childs)
				{
					DialogueOption newOption = new DialogueOption();
					newOption.Text = dialogueOption["Text"];
					newOption.TargetID = dialogueOption["TargetID"].AsInt;

					newTextContent.Options.Add(newOption);
				}

				// Map Dialogue Events...
				var dialogueEvents = contentItem["DialogueEvents"].AsArray;
				foreach(var dialogueEvent in dialogueEvents.Childs)
				{
					DialogueEvent newDialogueEvent = new DialogueEvent();
					newDialogueEvent.MessageName = dialogueEvent["MessageName"];
					newDialogueEvent.Args = new List<string>();
					
					var eventArgs = dialogueEvent["Args"].AsArray;
					foreach(var eventArg in eventArgs.Childs)
					{
						newDialogueEvent.Args.Add(eventArg);
					}
					
					newTextContent.DialogueEvents.Add(newDialogueEvent);
				}
				
				newThread.TextContent.Add(newTextContent);
			}
			
			TextThreads.Add(newThread);
		}
	}

	public TextThread GetThreadByName(string threadName)
	{
		if(string.IsNullOrEmpty(threadName))
		   throw new ArgumentNullException("Must specify a thread name!");

		TextThread result = TextThreads.FirstOrDefault(t => t.Name == threadName);
		return result;
	}

	public TextThread GetCurrentTextThread()
	{
		// Get the Default thread...
		TextThread result = TextThreads.FirstOrDefault(t => t.IsDefaultThread == true);

		// Find the first text entry that is in a supported dialogue range.
		TextThread specificResult = TextThreads.FirstOrDefault(t => _sequenceManager.EvaluateRange(t.SequenceRange));
		if(specificResult != default(TextThread))
			result = specificResult;

		return result;
	}

	#endregion Methods
}
