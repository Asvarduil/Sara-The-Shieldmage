using UnityEngine;
using System;
using System.Collections;

public class SaraPlayerControl : SidescrollingPlayerControl 
{
	#region Enumerations

	public enum SaraControlState
	{
		Moving,
		Casting
	}

	#endregion Enumerations

	#region Variables / Properties
	
	public SaraControlState controlState;

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

		switch(controlState)
		{
			case SaraControlState.Moving:
				DetectHorizontalMovement();
				DetectJumpCommand();
				DetectSpellcasting();
				break;

			case SaraControlState.Casting:
				DetectSpellPlacement();
				DetectCanceledCasting();
				break;

			default:
				throw new Exception("Unexpected SaraPlayerControl state: " + controlState);
		}
	}

	#endregion Hooks

	#region Methods

	// TODO: These methods are going to interact poorly with pausing/unpausing the menu.

	public void DetectSpellcasting()
	{
		if(! _control.GetPositiveAxis("Cast"))
			return;

		if(Time.time < _lastChange + _stateChangeLockout)
			return;

		_lastChange = Time.time;
		_movement.Suspend();
		_spellManager.PrepareSpell();
		controlState = SaraControlState.Casting;
	}

	private void DetectSpellPlacement()
	{
		if(! _control.GetPositiveAxis("Cast"))
			return;

		if(Time.time < _lastChange + _stateChangeLockout)
			return;
		
		_lastChange = Time.time;
		_movement.Resume();
		_spellManager.CastSpell();
		controlState = SaraControlState.Moving;
	}

	private void DetectCanceledCasting()
	{
		if(! _control.GetPositiveAxis("Cancel Cast"))
			return;

		if(Time.time < _lastChange + _stateChangeLockout)
			return;

		_lastChange = Time.time;
		_movement.Resume();
		_spellManager.CancelSpell();
		controlState = SaraControlState.Moving;
	}

	#endregion Methods
}
