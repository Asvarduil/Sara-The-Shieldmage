using UnityEngine;
using System.Collections;

public class THJSidescrollingMovement : MonoBehaviour
{
	#region Variables / Properties
	
	public bool CanMove = true;
	public bool AtJumpApex = false;
	public float walkSpeed = 5.0f;
	public float jumpForce = 20.0f;
	public float jumpLockout = 0.5f;
	public float jumpDecayRate = 0.9f;
	
	public Vector3 HurtForce = new Vector3(4, 4, 0);
	
	public bool facingRight;
	public bool isGrounded;
	public bool isHit;
	
	private float _lastJump;
	public Vector3 _friction;
	private Vector3 _moveVelocity = Vector3.zero;
	private CharacterController _controller;
	
	public CollisionFlags CollisionDirection { get; private set; }
	
	public bool HitHead 
	{ 
		get 
		{ 
			return (CollisionDirection & CollisionFlags.CollidedAbove) != CollisionFlags.None; 
		} 
	}
	
	public bool TouchingWall
	{
		get
		{
			return (CollisionDirection & CollisionFlags.Sides) != CollisionFlags.None;
		}
	}
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	public void Start()
	{
		_controller = GetComponent<CharacterController>();
	}
	
	public void Update()
	{	
		isGrounded = _controller.isGrounded;
		
		if(_controller.isGrounded)
		{
			isHit = false;
		}
		else
		{
			if(HitHead)
			{
				AtJumpApex = true;
				HaltJump();
			}
			
			_moveVelocity.y += Physics.gravity.y * Time.deltaTime;
			if(Mathf.Abs(_moveVelocity.y - 0.0f) < 0.1f)
			{
				AtJumpApex = true;
				HaltJump();
			}
			else
			{
				AtJumpApex = false;
			}
		}
		
		CollisionDirection = _controller.Move(_moveVelocity * Time.deltaTime);
	}
	
	#endregion Engine Hooks
	
	#region Messages
	
	public void TakeDamage(int damage)
	{
		isHit = true;
	}
	
	#endregion Messages
	
	#region Public Methods
	
	public void AddForce(Vector3 force)
	{
		_moveVelocity = force;
	}
	
	public void ClearMovement()
	{
		_moveVelocity.x = 0;
		_moveVelocity.z = 0;
	}
	
	public void MoveRight()
	{
		if(! CanMove)
			return;
		
		_moveVelocity.x = walkSpeed;
		facingRight = true;
	}
	
	public void MoveLeft()
	{
		if(! CanMove)
			return;
			
		_moveVelocity.x = -1 * walkSpeed;
		facingRight = false;
	}
	
	public void Jump()
	{
		if(Time.time < _lastJump + jumpLockout)
			return;
		
		if(! _controller.isGrounded)
			return;
		
		_lastJump = Time.time;
		_moveVelocity.y = jumpForce;
	}
	
	public void PartialJump()
	{
		_lastJump = Time.time;
		_moveVelocity.y = jumpForce;
	}
	
	public void SlowJump()
	{
		_moveVelocity.y *= jumpDecayRate;
	}
	
	public void HaltJump()
	{
		_moveVelocity.y = -2;
	}
	
	#endregion Public Methods
}
