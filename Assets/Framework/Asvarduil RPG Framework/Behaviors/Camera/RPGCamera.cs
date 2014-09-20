using UnityEngine;
using System;
using System.Collections;

public class RPGCamera : DebuggableBehavior 
{	
	#region Variables / Properties

	public bool followEntity = true;

	public Transform target;
	public float distance = 30.0f;
	public float cameraLag = 1.0f;
	public Vector3 offset;
	
	private float _x;
	private float _y;
	
	#endregion Variables / Properties
	
	#region Engine Hooks

	public void Start () 
	{
		DebugMessage("Initial Rotation: " + transform.rotation.eulerAngles);
		
		AcquireRotation();
		if(rigidbody)
			rigidbody.freezeRotation = true;
		
		DebugMessage("Post Setup Rotation: " + transform.rotation.eulerAngles);
	}
	
	public void Update() 
	{
		if(!followEntity)
			return;

		if(!target)
			return;

		UpdateTransform();
	}
	
	#endregion Engine Hooks
	
	#region Methods
	
	public void SetTarget(GameObject targetObject)
	{
		target = targetObject.transform;
	}

	public void UpdateTransform()
	{
		Quaternion rotation = Quaternion.Euler(_y, _x, 0.0f);
		Vector3 position = transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position + offset;
				
		transform.rotation = rotation;
		transform.position = position;
		//transform.position = Vector3.Lerp(transform.position, position, cameraLag);
		
		DebugMessage("Rotation: " + transform.rotation.eulerAngles + Environment.NewLine
		             + "Position: " + transform.position + Environment.NewLine
		             + "Distance From Player: " + Vector3.Distance(transform.position, target.transform.position));
	}
	
	/// <summary>
	/// Alters the camera's rotation and distance from the targeted object.
	/// </summary>
	/// <param name='newRotation'>New rotation.</param>
	/// <param name='newDistance'>New distance.</param>
	public void AlterCamera(Vector3 newRotation, float newDistance)
	{
		// Prevents another programmer from using a negative value...
		newDistance = Mathf.Abs (newDistance);
		if(Mathf.Abs (newDistance - 0.0f) <= 0.0001) 
			throw new ArgumentException("newDistance cannot be 0.0!");
		
		_x = newRotation.y;
		_y = newRotation.x;
		distance = newDistance;
	}
	
	private void AcquireRotation()
	{
		Vector3 angles = transform.rotation.eulerAngles;
		
		_x = angles.y;
		_y = angles.x;
	}
	
	#endregion Methods
}
