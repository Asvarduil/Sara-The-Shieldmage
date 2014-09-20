using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class JrpgMapControlSystem : DebuggableBehavior, ISuspendable
{
	#region Variables / Properties

	// Idle animations
	public bool CanMove = true;
	public bool IsIdle = true;
	public List<CharacterControlHook> MoveControls;
	public List<CharacterControlHook> IdleControls;
	public CharacterControlDirection Direction = CharacterControlDirection.South;

	private CharacterControlHook _currentMoveHook;
	private CharacterControlHook _currentIdleHook;

	private ControlManager _controlManager;
	private PedestrianMovement _movement;
	private AsvarduilSpriteSystem _sprite;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_controlManager = ControlManager.Instance;
		_movement = GetComponent<PedestrianMovement>();
		_sprite = GetComponentInChildren<AsvarduilSpriteSystem>();

		_currentIdleHook = FindCurrentIdleHook();
	}

	public void Update()
	{
		CollectInput();
		PerformAnimation();
	}

	#endregion Engine Hooks

	#region Methods

	public void Suspend()
	{
		CanMove = false;
	}

	public void Resume()
	{
		CanMove = true;
	}

	private void CollectInput()
	{
		if(! CanMove)
			return;

		_currentMoveHook = FindCurrentMoveHook();
		IsIdle = _currentMoveHook == default(CharacterControlHook);
		
		if(! IsIdle)
		{
			Direction = _currentMoveHook.Direction;
			_currentIdleHook = FindCurrentIdleHook();
			_movement.Move(_currentMoveHook.MoveDirection);
		}
	}

	private CharacterControlHook FindCurrentMoveHook()
	{
		var result = MoveControls.FirstOrDefault(h => h.TestHook(_controlManager));
		if(result != default(CharacterControlHook))
			DebugMessage("New move hook: " + result.Name);

		return result;
	}

	private CharacterControlHook FindCurrentIdleHook()
	{
		var result = IdleControls.FirstOrDefault(h => h.FacingDirection(Direction));
		return result;
	}

	private void PerformAnimation()
	{
		if(IsIdle)
			_sprite.PlaySingleFrame(_currentIdleHook.Animation);
		else
			_sprite.PlaySingleFrame(_currentMoveHook.Animation);
	}

	#endregion Methods
}
