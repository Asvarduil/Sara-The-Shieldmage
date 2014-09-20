using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcMovement : DebuggableBehavior
{
	#region Variables / Properties

	public float StepDelay = 1.0f;
	public float StepDistance = 1.0f;

	public bool IsAutomaticallyMoving = true;
	public string AutomaticMovePattern = "NSEWN";

	public List<CharacterControlHook> MoveHooks;
	public List<CharacterControlHook> IdleHooks;

	private bool _isMoving = true;
	private AsvarduilSpriteSystem _sprite;
	private PedestrianMovement _movement;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_sprite = GetComponentInChildren<AsvarduilSpriteSystem>();
		_movement = GetComponent<PedestrianMovement>();
	}

	public void Update()
	{
		if(string.IsNullOrEmpty(AutomaticMovePattern))
		   return;

		if(! IsAutomaticallyMoving)
			return;

		// TODO: Obey the given sequence.
	}

	#endregion Engine Hooks

	#region Methods

	#endregion Methods
}
