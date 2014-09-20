using UnityEngine;
using System;
using System.Collections;

public class THJRPGCamera : MonoBehaviour {
	
	#region Variables / Properties
	
	public bool DebugMode = true;
	
	public Transform target;
	public float distance = 30.0f;
	
	private float x;
	private float y;
	
	#endregion Variables / Properties
	
	#region Engine Hooks

	// Use this for initialization
	void Start () 
	{
		if(DebugMode)
			Debug.Log("Initial Rotation: " + transform.rotation.eulerAngles);
		
		//AcquireRotation();
		if(rigidbody)
			rigidbody.freezeRotation = true;
		
		if(DebugMode)
			Debug.Log("Post Setup Rotation: " + transform.rotation.eulerAngles);
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(!target)
			return;

		Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
		Vector3 position = transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
		
		transform.rotation = rotation;
        transform.position = position;
		
		if(DebugMode)
		{
			Debug.Log("Rotation: " + transform.rotation.eulerAngles + Environment.NewLine
				      + "Position: " + transform.position + Environment.NewLine
				      + "Distance From Player: " + Vector3.Distance(transform.position, target.transform.position));
		}
	}
	
	#endregion Engine Hooks
	
	#region Methods
	
	public void SetTarget(GameObject targetObject)
	{
		target = targetObject.transform;
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
		
		x = newRotation.y;
		y = newRotation.x;
		distance = newDistance;
	}
	
	private void AcquireRotation()
	{
		Vector3 angles = transform.rotation.eulerAngles;
		
		x = angles.y;
		y = angles.x;
	}
	
	#endregion Methods
}
