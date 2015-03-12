using UnityEngine;
using System.Collections;

public class PedestrianMovement : DebuggableBehavior
{
	#region Variables / Properties

	public bool CanMove = true;
	public float MoveSpeed = 1.0f;

	private CharacterController _controller;
	private Vector3 _moveDirection;
	private CollisionFlags _collision;

	public CollisionFlags Collision
	{
		get { return _collision; }
	}

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
		if (! CanMove)
			return;

		Vector3 movement = (direction + Physics.gravity) * MoveSpeed * Time.deltaTime;
		_collision = _controller.Move(movement);
	}

	#endregion Methods
}
