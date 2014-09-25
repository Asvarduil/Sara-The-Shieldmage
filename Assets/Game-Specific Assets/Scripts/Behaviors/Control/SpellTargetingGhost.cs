using UnityEngine;
using System.Collections;

public class SpellTargetingGhost : DebuggableBehavior
{
	#region Variables

	public float effectDepth = 6.5f;

	#endregion Variables

	#region Engine Hooks

	public void Update()
	{
		Vector3 pos = Input.mousePosition;
		pos.z = effectDepth;
		pos = Camera.main.ScreenToWorldPoint(pos);

		transform.position = pos;
	}

	#endregion Engine Hooks
}
