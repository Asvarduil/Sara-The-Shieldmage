using UnityEngine;

public abstract class SidescrollingPlayerControl : DebuggableBehavior, ISuspendable
{
	#region Variables / Properties

	public bool canAcceptInput = true;
	public bool canMove = true;
	public bool canJump = true;
	public bool isFacingRight = true;

	protected bool _isHit = false;
	protected bool _isFalling = false;
	protected bool _isJumping = false;
	protected bool _isMovingHorizontally = false;

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
		DetermineControlState();

		_animation.Animate();
		_movement.MoveCharacter();
	}
	
	#endregion Engine Hooks
	
	#region Methods

	public virtual void Suspend()
	{
		canAcceptInput = false;
		_isJumping = false;
		_isMovingHorizontally = false;
	}

	public virtual void Resume()
	{
		canAcceptInput = true;
	}

	public virtual void OnDamageTaken()
	{
		_isHit = true;
	}

	public virtual void ProcessAxes()
	{
		if(! canAcceptInput)
			return;

		DetectHorizontalMovement();
		DetectJumpCommand();
	}

	public abstract void DetermineControlState();

	private void PrepMovementFrame()
	{
		_movement.ClearHorizontalMovement();
		_isMovingHorizontally = false;

		// If you're previously hit, you're hit until you're grounded again.
		if(_isHit)
			_isHit = _movement.MovementType != SidescrollingMovementType.Grounded;
	}

	protected void DetectHorizontalMovement()
	{
		if(! canMove)
			return;

		if(_control.GetAxis("Horizontal") > 0)
		{
			_isMovingHorizontally = true;
			isFacingRight = true;
		}
		
		if(_control.GetAxis("Horizontal") < 0)
		{
			_isMovingHorizontally = true;
			isFacingRight = false;
		}

		if(_isMovingHorizontally)
		{		
			_movement.MoveHorizontally(isFacingRight);
		}
	}

	protected void DetectJumpCommand()
	{
		if(! canJump)
			return;

		if(_control.GetAxis("Jump") == 0)
		{
			if(_movement.MovementType == SidescrollingMovementType.Jumping)
				_movement.HaltJump();

			return;
		}

		_movement.Jump();
		_isJumping = true;
	}

	#endregion Methods
}

