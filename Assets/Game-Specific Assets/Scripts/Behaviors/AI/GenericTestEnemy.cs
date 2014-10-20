using System;
using System.Collections.Generic;

using UnityEngine;

public class GenericTestEnemy : DebuggableBehavior
{
	#region Enumerations

	public enum AIState
	{
		Idle,
		Active
	}

	#endregion Enumerations

	#region Variables / Properties

	public AIState State;

	private Dictionary<Func<bool>, Action> States;

	#endregion Variables / Properties

	#region Hooks

	public void Start()
	{
		States = new Dictionary<Func<bool>, Action>();
		States.Add(CanSeePlayer, LoafAbout);
	}

	public void Update()
	{
		foreach(KeyValuePair<Func<bool>, Action> state in States)
		{
			if(! state.Key())
				continue;

			state.Value();
			break;
		}
	}

	#endregion Hooks

	#region Methods

	#endregion Methods

	#region Conditions

	private bool CanSeePlayer()
	{
		return false;
	}

	#endregion Conditions

	#region Mannerisms

	private void LoafAbout()
	{
		// Snap into a Slim Tim! (TM)
	}

	#endregion Mannerisms
}
