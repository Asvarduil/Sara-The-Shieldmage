using UnityEngine;
using System;
using System.Collections;

public abstract class DebuggableBehavior : MonoBehaviour 
{
	#region Enumerations

	public enum LogLevel
	{
		Information,
		Warning,
		LogicError,
	}

	#endregion Enumerations

	#region Variables / Properties

	public bool DebugMode = false;
	public bool ShowTimestamps = false;

	#endregion Variables / Properties

	#region Methods

	public void DebugMessage(string message, LogLevel level = LogLevel.Information)
	{
		if(! DebugMode)
			return;

		if(ShowTimestamps)
			message = DateTime.Now.ToString("HH:mm:ss") + ": " + message;

		switch(level)
		{
			case LogLevel.Information:
				Debug.Log(message);
				break;

			case LogLevel.Warning:
				Debug.LogWarning(message);
				break;

			case LogLevel.LogicError:
				Debug.LogError(message);
				break;

			default:
				throw new Exception("Unexpected log level! " + level.ToString()
			                        + Environment.NewLine + " message: " + message);
		}
	}

	#endregion Methods
}
