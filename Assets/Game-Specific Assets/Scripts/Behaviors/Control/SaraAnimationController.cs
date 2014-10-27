using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SaraAnimationController : SidescrollingAnimationController
{
	#region Variables / Properties

	public List<SaraAnimationState> animationStates;

	private bool _overrideAnimation = false;
	private SaraPlayerControl _control;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();

		_control = GetComponent<SaraPlayerControl>();
	}

	#endregion Engine Hooks

	#region Hooks

	protected override void SelectCurrentAnimation()
	{
		if(_overrideAnimation)
			return;

		SaraAnimationState sequence = animationStates.FirstOrDefault(a => a.ControlState == _control.controlState);
		if(sequence == default(SaraAnimationState))
			return;

		_currentAnimation = sequence.Animation;
	}

	#endregion Hooks

	#region Methods

	public void OverrideAnimation(string animation)
	{
		_overrideAnimation = true;

		SaraAnimationState sequence = animationStates.FirstOrDefault(a => a.ControlState.ToString() == animation);
		if(sequence == default(SaraAnimationState))
			return;
		
		_currentAnimation = sequence.Animation;
	}

	public void StopAnimationOverride()
	{
		_overrideAnimation = false;
	}

	#endregion Methods
}
