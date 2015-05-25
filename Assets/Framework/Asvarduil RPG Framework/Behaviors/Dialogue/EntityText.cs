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

        TextThreads = threads.AsArray.UnfoldJsonArray<TextThread>();
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
