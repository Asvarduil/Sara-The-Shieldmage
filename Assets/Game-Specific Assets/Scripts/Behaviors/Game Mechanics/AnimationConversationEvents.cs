using UnityEngine;
using System;
using System.Collections.Generic;

public class AnimationConversationEvents : DebuggableBehavior
{
	#region Variables / Properties
	
	private SaraAnimationController _animation;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_animation = GameObject.FindGameObjectWithTag("Player").GetComponent<SaraAnimationController>();
	}

	#endregion Engine Hooks

	#region Methods

	public void PerformAnimation(List<string> args)
	{
		string overrideAnimation = args[0];
		_animation.OverrideAnimation(overrideAnimation);
	}

	public void ResumeOriginalAnimation(List<string> args)
	{
		_animation.StopAnimationOverride();
	}

	#endregion Methods
}
