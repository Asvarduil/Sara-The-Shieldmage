using UnityEngine;
using System.Collections;

public class CameraFreezeTrigger : DebuggableBehavior
{
	#region Variables / Properties

	public string tagToReactTo = "Player";
	private RPGCamera _cam;
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	public void Start()
	{
		_cam = FindObjectOfType<RPGCamera>();
	}
	
	public void OnTriggerEnter(Collider who)
	{
		if(who.tag != tagToReactTo)
			return;
		
		DebugMessage("A player has entered the cam freeze trigger.");
		_cam.followEntity = false;
	}
	
	public void OnTriggerExit(Collider who)
	{
		if(who.tag != tagToReactTo)
			return;
		
		DebugMessage("A player has left the cam freeze trigger.");
		_cam.followEntity = true;
	}
	
	#endregion Engine Hooks
}
