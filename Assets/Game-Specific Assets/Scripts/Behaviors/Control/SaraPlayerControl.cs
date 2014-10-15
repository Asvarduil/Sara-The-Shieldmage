using UnityEngine;
using System;
using System.Collections;

public class SaraPlayerControl : SidescrollingPlayerControl 
{
	#region Variables / Properties
	
	public string spellCastAxis;
	public SaraControlState controlState;
	
	private bool _isCasting = false;

	private const float _stateChangeLockout = 0.25f;
	private float _lastChange;

	private SpellManager _spellManager;

	#endregion Variables / Properties

	#region Hooks

	public override void Start()
	{
		base.Start();
		_spellManager = SpellManager.Instance;
	}

	public override void ProcessAxes ()
	{
		if(! canAcceptInput)
			return;

		if(!_isCasting)
		{
			DetectHorizontalMovement();
			DetectJumpCommand();
			DetectSpellcasting();
		}
		else
		{
			DetectSpellPlacement();
		}
	}

	public override void DetermineControlState()
	{
		_isJumping = _movement.MovementType == SidescrollingMovementType.Jumping;
		_isFalling = _movement.MovementType == SidescrollingMovementType.Falling;

		controlState = isFacingRight ? SaraControlState.IdleRight : SaraControlState.IdleLeft;

		if(_isHit)
		{
			controlState = isFacingRight ? SaraControlState.HitRight : SaraControlState.HitLeft;
			return;
		}

		if(_isCasting)
		{
			controlState = isFacingRight ? SaraControlState.CastRight : SaraControlState.CastLeft;
			return;
		}

		if(_isFalling)
		{
			controlState = isFacingRight ? SaraControlState.FallRight : SaraControlState.FallLeft;
			return;
		}

		if(_isJumping)
		{
			controlState = isFacingRight ? SaraControlState.JumpRight : SaraControlState.JumpLeft;
			return;
		}

		if(_isMovingHorizontally)
		{
			controlState = isFacingRight ? SaraControlState.MoveRight : SaraControlState.MoveLeft;
			return;
		}
	}

	#endregion Hooks

	#region Methods

	// TODO: These methods are going to interact poorly with pausing/unpausing the menu.

	public void DetectSpellcasting()
	{
		if(! _control.GetAxisDown(spellCastAxis))
			return;

		if(Time.time < _lastChange + _stateChangeLockout)
			return;

		_isCasting = true;
		_lastChange = Time.time;
		_movement.Suspend();
		_spellManager.PrepareSpell();
	}

	private void DetectSpellPlacement()
	{
		if(! _control.GetAxisUp(spellCastAxis))
			return;

		_isCasting = false;
		_movement.Resume();
		_spellManager.CastSpell();
	}

	#endregion Methods
}
