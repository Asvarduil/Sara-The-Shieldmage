using UnityEngine;
using System;
using System.Collections;

public class CameraTrigger : DebuggableBehavior 
{	
	#region Variables / Properties
	
	public Vector3 newRotation = new Vector3(25.0f, 0.0f, 0.0f);
	public float newDistance = 10.0f;
	
	private RPGCamera _camera;
	private Vector3 _originalRotation;
	private float _originalDistance;
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	// Use this for initialization
	void Start () 
	{
		_camera = (RPGCamera) GameObject.FindObjectOfType(typeof(RPGCamera));
		if(_camera == null)
		{
			throw new Exception("Could not find an RPG Camera in the scene!");
		}
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider who)
	{
		if(who.tag == "Player")
		{
			DebugMessage("A player has entered the camera trigger!");
			_originalRotation = _camera.transform.rotation.eulerAngles;
			_originalDistance = _camera.distance;
		
			_camera.AlterCamera(newRotation, newDistance);
		}
	}
	
	void OnTriggerExit(Collider who)
	{
		if(who.tag == "Player")
		{
			DebugMessage("A player has left the trigger...");
			_camera.AlterCamera(_originalRotation, _originalDistance);
		}
	}
	
	#endregion Engine Hooks
	
	#region Methods
	#endregion Methods
}
