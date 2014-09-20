using UnityEngine;

public class SidescrollingPlayerControl : DebuggableBehavior, ISuspendable
{
	#region Variables / Properties

	public bool canAcceptInput = true;
	public bool isFacingRight = true;

	private ControlManager _control;
	private SidescrollingMovement _movement;
	private SidescrollingAnimationController _animation;
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	public void Start()
	{
		_control = ControlManager.Instance;
		_movement = GetComponent<SidescrollingMovement>();
		_animation = GetComponent<SidescrollingAnimationController>();
	}
	
	public void Update()
	{
		PrepMovementFrame();

		if(canAcceptInput)
		{
			DetectHorizontalMovement();
			DetectJumpCommand();
		}

		_animation.Animate();
		_movement.MoveCharacter();
	}
	
	#endregion Engine Hooks
	
	#region Methods

	public void Suspend()
	{
		canAcceptInput = false;
	}

	public void Resume()
	{
		canAcceptInput = true;
	}

	private void PrepMovementFrame()
	{
		_movement.ClearHorizontalMovement();
		_animation.isMovingHorizontallyThisTick = false;
	}

	private void DetectHorizontalMovement()
	{
		if(_control.GetAxis("Horizontal") > 0)
		{
			_animation.isMovingHorizontallyThisTick = true;
			isFacingRight = true;
		}
		
		if(_control.GetAxis("Horizontal") < 0)
		{
			_animation.isMovingHorizontallyThisTick = true;
			isFacingRight = false;
		}

		_animation.isFacingRight = isFacingRight;
		if(_animation.isMovingHorizontallyThisTick)
		{		
			_movement.MoveHorizontally(isFacingRight);
		}
	}

	private void DetectJumpCommand()
	{
		if(_control.GetAxis("Jump") == 0)
			return;

		_movement.Jump();
	}

	#endregion Methods
}

