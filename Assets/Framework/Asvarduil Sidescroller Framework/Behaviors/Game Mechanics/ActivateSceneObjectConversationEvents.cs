using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class ActivateSceneObjectConversationEvents : DebuggableBehavior
{
	#region Variables / Properties

	public List<GameObject> ImportantObjects;

	#endregion Variables / Properties

	#region Hooks

	#endregion Hooks

	#region Methods

	public void EnableObject(List<string> args)
	{
		string objectName = args[0];
		if (string.IsNullOrEmpty (objectName)) 
			throw new ArgumentException("Enable Object requires the name of an object to enable!");

		GameObject importantObject = ImportantObjects.FirstOrDefault(o => o.name == objectName);
		if (importantObject == default(GameObject))
			throw new Exception ("Could not find game object " + objectName);

		importantObject.SetActive(true);
	}

	public void DisableObject(List<string> args)
	{
		string objectName = args[0];
		if (string.IsNullOrEmpty (objectName)) 
			throw new ArgumentException("Enable Object requires the name of an object to enable!");
		
		GameObject importantObject = ImportantObjects.FirstOrDefault(o => o.name == objectName);
		if (importantObject == default(GameObject))
			throw new Exception ("Could not find game object " + objectName);
		
		importantObject.SetActive(false);
	}

	#endregion Methods
}
