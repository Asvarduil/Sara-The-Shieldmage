using System;
using UnityEngine;

[Serializable]
public class CharacterControlHook
{
	#region Variables / Properties

	public string Name;
	public string Animation;
	public string InputAxis;
	public bool TriggerIfPositive;
	public CharacterControlDirection Direction;
	public Vector3 MoveDirection;

	#endregion Variables / Properties

	#region Methods

	public bool TestHook(ControlManager controls)
	{
		return TriggerIfPositive
			? controls.GetPositiveAxis(InputAxis)
			: controls.GetNegativeAxis(InputAxis);
	}

	public bool FacingDirection(CharacterControlDirection direction)
	{
		return direction == Direction;
	}

	#endregion Methods
}
