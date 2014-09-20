using UnityEngine;
using System.Collections;

public class PedestrianMovement : MonoBehaviour 
{
	#region Variables / Properties

	public bool CanMove = true;
	public float MoveSpeed = 1.0f;
	public float Gravity = 9.0f;

	private CharacterController _controller;
	private Vector3 _moveDirection;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_controller = GetComponent<CharacterController>();
	}

	#endregion Engine Hooks

	#region Methods

	public void Move(Vector3 direction)
	{
		if(CanMove
		   && _controller.isGrounded)
		{
			direction *= MoveSpeed;
			direction *= Time.deltaTime;
		}
		else
		{
			direction = Vector3.zero;
		}

		direction.y -= Gravity * Time.deltaTime;

		_controller.Move(direction);
	}

	#endregion Methods
}
