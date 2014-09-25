using UnityEngine;

public class SidescrollingPlayerControl : DebuggableBehavior, ISuspendable
{
	#region Variables / Properties

	public bool canAcceptInput = true;
	public bool isFacingRight = true;

	protected ControlManager _control;
	protected SidescrollingMovement _movement;
	protected SidescrollingAnimationController _animation;
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	public virtual void Start()
	{
		_control = ControlManager.Instance;
		_movement = GetComponent<SidescrollingMovement>();
		_animation = GetComponent<SidescrollingAnimationController>();
	}
	
	public void Update()
	{
		PrepMovementFrame();

		ProcessAxes();

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

	public virtual void ProcessAxes()
	{
		if(! canAcceptInput)
			return;

		DetectHorizontalMovement();
		DetectJumpCommand();
	}

	private void PrepMovementFrame()
	{
		_movement.ClearHorizontalMovement();
		_animation.isMovingHorizontallyThisTick = false;
	}

	protected void DetectHorizontalMovement()
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

	protected void DetectJumpCommand()
	{
		if(_control.GetAxis("Jump") == 0)
		{
			if(_movement.MovementType == SidescrollingMovementType.Jumping)
				_movement.HaltJump();

			return;
		}

		_movement.Jump();
	}

	#endregion Methods
}

