using UnityEngine;
using System.Collections;

public class PropelledObject : MonoBehaviour 
{
	#region Variables / Properties

	public Vector3 velocity;

	#endregion Variables / Properties

	#region Hooks

	public void FixedUpdate()
	{
		transform.Translate(velocity * Time.deltaTime);
	}

	#endregion Hooks
}
