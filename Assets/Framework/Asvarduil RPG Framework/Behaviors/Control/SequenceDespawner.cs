using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class SequenceDespawner : MonoBehaviour 
{
	#region Variables / Properties

	public List<SequenceRange> SequenceRanges;

	private SequenceManager _sequenceManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_sequenceManager = SequenceManager.Instance;

		bool shouldStayActive = SequenceRanges.Any(s => _sequenceManager.EvaluateRange(s));
		if(! shouldStayActive)
			gameObject.SetActive(false);
	}

	#endregion Engine Hooks
}
