using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class PersistentSingularObject : DebuggableBehavior
{
	#region Variables / Properties

	public bool OriginalObject = false;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Awake()
	{
		if(OriginalObject)
			return;

		// Not original object...see if there are any other PersistentSingularObjects.
		DebugMessage("Searching for other Persistant Singular Objects...");
		IEnumerable<PersistentSingularObject> objects = (PersistentSingularObject[]) FindObjectsOfType(typeof(PersistentSingularObject));
		PersistentSingularObject original = objects.FirstOrDefault(o => o.OriginalObject == true);

		if(original != default(PersistentSingularObject))
		{
			DebugMessage("An original Persistent Singular Object exists.  Self-destructing...");
			Destroy(gameObject);
			return;
		}

		DebugMessage("Setting self as the original, and becoming persistent.");
		OriginalObject = true;
		DontDestroyOnLoad(gameObject);
	}

	#endregion Engine Hooks
}
