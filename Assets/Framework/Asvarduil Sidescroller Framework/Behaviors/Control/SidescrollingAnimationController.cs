using UnityEngine;
using System.Collections;

public class SidescrollingAnimationController : DebuggableBehavior
{
	#region Variables / Properties

	public bool isFacingRight = true;
	public bool isMovingHorizontallyThisTick = false;

	public string idleRightAnimation = "Idle - R";
	public string idleLeftAnimation = "Idle - L";
	public string runRightAnimation = "Run - R";
	public string runLeftAnimation = "Run - L";
	public string jumpRightAnimation = "Jump - R";
	public string jumpLeftAnimation = "Jump - L";
	public string fallRightAnimation = "Fall - R";
	public string fallLeftAnimation = "Fall - L";
	
	public string activeAnimation = "Idle - R";

	private SidescrollingMovement _movement;
	private AsvarduilSpriteSystem _sprite;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_movement = GetComponent<SidescrollingMovement>();
		_sprite = GetComponentInChildren<AsvarduilSpriteSystem>();
		
		_sprite.SetAnimation(activeAnimation);
	}

	#endregion Engine Hooks

	#region Methods

	private void UpdateCurrentAnimation()
	{
		switch(_movement.MovementType)
		{
			case SidescrollingMovementType.Grounded:
				if(isMovingHorizontallyThisTick)
					activeAnimation = isFacingRight ? runRightAnimation : runLeftAnimation;
				else
					activeAnimation = isFacingRight ? idleRightAnimation : idleLeftAnimation;
				break;
				
			case SidescrollingMovementType.Jumping:
				// TODO: Implement jumping animations
				activeAnimation = isFacingRight ? jumpRightAnimation : jumpLeftAnimation;
				break;
				
			case SidescrollingMovementType.Falling:
				// TODO: Implement falling animations
				activeAnimation = isFacingRight ? fallRightAnimation : fallLeftAnimation;
				break;
				
			default:
				DebugMessage("Unexpected movement type: " + _movement.MovementType);
				break;
		}
	}

	public void Animate()
	{
		UpdateCurrentAnimation();
		_sprite.PlaySingleFrame(activeAnimation);
	}

	#endregion Methods
}
