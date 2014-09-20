using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueLoader : JsonBlobLoaderBase
{
	#region Variables / Properties

	private EntityText _entityText;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		LoadDialogueFromJson();
	}

	#endregion Engine Hooks

	#region Blob Retrieval

	[ContextMenu("Sync Local Object to JSON data")]
	public void LoadDialogueFromJson()
	{
		_entityText = GetComponent<EntityText>();

		if(TryDownloadingBlob)
			StartCoroutine(DownloadBlob());
		
		// First: Is the downloaded blob empty?  If so, use a local data file.
		if(string.IsNullOrEmpty(RawBlob))
		{
			RawBlob = FetchLocalBlob();
			
			// Second: Is the local data file empty?  If so, don't go any further,
			//         because this is a waste of time.
			if(string.IsNullOrEmpty(RawBlob))
				return;
			else
				_entityText.MapBlob(RawBlob);
		}
		else
		{
			_entityText.MapBlob(RawBlob);
		}
	}

	#endregion Blob Retrieval
}
