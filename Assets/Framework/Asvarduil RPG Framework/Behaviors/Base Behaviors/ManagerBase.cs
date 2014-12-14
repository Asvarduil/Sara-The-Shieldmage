using UnityEngine;
using System;
using System.Collections;

using UnityObject = UnityEngine.Object;

public class ManagerBase<T>
	: DebuggableBehavior
	where T : UnityObject
{
	#region Variables / Properties

	private static T _instance;
	public static T Instance
	{
		get
		{
			if(_instance == null)
				_instance = FindObjectOfType<T>();

			return _instance;
		}
	}

	#endregion Variables / Properties

	#region Methods

	#endregion Methods
}
