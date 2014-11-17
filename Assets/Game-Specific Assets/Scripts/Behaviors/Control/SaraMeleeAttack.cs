using UnityEngine;
using System.Collections;

public class SaraMeleeAttack : DebuggableBehavior
{
	#region Variables / Properties

	public GameObject attackEffect;

	public Vector3 rightOffset;
	public Vector3 leftOffset;
	public Vector3 rightRotation;
	public Vector3 leftRotation;

	#endregion Variables / Properties

	#region Methods

	public void LaunchAttack(bool isFacingRight)
	{
		Vector3 offset = isFacingRight ? rightOffset : leftOffset;
		Vector3 pos = transform.position + offset;

		Quaternion rot = Quaternion.Euler(isFacingRight ? rightRotation : leftRotation);

		GameObject.Instantiate (attackEffect, pos, rot);
	}

	#endregion Methods
}
