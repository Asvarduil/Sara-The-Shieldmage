using UnityEngine;
using System;
using System.Collections;

public class SaraPlayerControl : SidescrollingPlayerControl 
{
	#region Variables / Properties
	
	public string spellCastAxis;
	public string attackAxis;
	public SaraControlState controlState;

	private bool _isAttacking = false;
	private bool _isCasting = false;

	private const float _attackLockout = 0.25f;
	private const float _spellCastLockout = 0.25f;

	private float _lastAttack;
	private float _lastSpellCast;

	private SpellManager _spellManager;
	private SaraMeleeAttack _meleeAttack;

	#endregion Variables / Properties

	#region Hooks

	public override void Start()
	{
		base.Start();
		_spellManager = SpellManager.Instance;
		_meleeAttack = GetComponent<SaraMeleeAttack>();
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
			DetectAttacking();
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

		if(_isAttacking)
		{
			controlState = isFacingRight ? SaraControlState.AttackRight : SaraControlState.AttackLeft;
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

	public void DetectAttacking()
	{
		if(_control.GetAxisUp(attackAxis))
		{
			_isAttacking = false;	
		}
		else if(_control.GetAxisDown(attackAxis))
		{
			if(Time.time < _lastAttack + _attackLockout)
				return;

			_isAttacking = true;
			_lastAttack = Time.time;
			_isMovingHorizontally = false;

			_meleeAttack.LaunchAttack(isFacingRight);
		}
	}

	public void DetectSpellcasting()
	{
		if(! _control.GetAxisDown(spellCastAxis))
			return;

		if(Time.time < _lastSpellCast + _spellCastLockout)
			return;

		_isCasting = true;
		_lastSpellCast = Time.time;
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
