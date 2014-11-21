using UnityEngine;
using System.Collections;

public class SwellingObject : MonoBehaviour 
{
	#region Variables / Properties

	public Vector3 GrowthScale;
	public Vector3 TargetScale;

	private const float growthEpsilon = 0.1f;

	#endregion Variables / Properties

	#region Hooks

	public void Update()
	{
		if (Vector3.Distance(transform.localScale, TargetScale) < growthEpsilon)
			return;

		transform.localScale += GrowthScale; 
	}

	#endregion Hooks

	#region Methods

	#endregion Methods
}
