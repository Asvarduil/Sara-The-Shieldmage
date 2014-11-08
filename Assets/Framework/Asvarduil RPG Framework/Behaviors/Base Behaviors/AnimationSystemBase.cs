using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using CustomAnimation = AsvarduilAnimation<AsvarduilFrame<UnityEngine.Object>>;

public abstract class AnimationSystemBase<T> : DebuggableBehavior where T : CustomAnimation
{
	#region Variables / Properties

	public bool AutomaticallyPlay = true;
	public List<T> Animations;

	private T _currentAnimation;
	private int _flipDirection = 1;
	private float _lastFrameChange = 0.0f;

	public int CurrentFrame;

	private int FlipBoundary
	{
		get { return _flipDirection > 0 ? _currentAnimation.Frames.Count - 1 : 0; }
	}
	
	public bool IsAnimationDone
	{
		get { return _flipDirection > 0 && CurrentFrame > FlipBoundary; }
	}
	
	public bool HasAnimationReset
	{
		get { return _flipDirection < 0 && CurrentFrame < FlipBoundary; }
	}
	
	private bool CanChangeFrames
	{
		get { return Time.time >= _lastFrameChange + _currentAnimation.Frames[CurrentFrame].AnimationDelay; }
	}
	
	#endregion Variables / Properties
	
	#region Engine Hooks

	public virtual void Start()
	{
		if(Animations == null
		   || Animations.Count == 0
		   || Animations[0] == null)
			throw new Exception("An animation system must have at least one animation!");

        _currentAnimation = Animations[0];
	}

	public virtual void Update()
	{
		if(! AutomaticallyPlay)
			return;
		
		if(! CanChangeFrames)
			return;
		
		IncrementFrame();
	}
	
	#endregion Engine Hooks
	
	#region Methods

	public abstract void PlaySingleFrame(string animation);

	public void SetAnimation(string animation)
	{
		T newAnimation = Animations.FirstOrDefault(a => a.Name == animation);
		if(newAnimation == default(T))
		{
			DebugMessage("Could not find animation " + animation, LogLevel.LogicError);
		}
		
		_currentAnimation = newAnimation;
	}

	protected void ResetAnimation()
	{
		CurrentFrame = 0;
		_flipDirection = 1;
	}

	protected void IncrementFrame()
	{
		CurrentFrame += _flipDirection;
		
		switch(_currentAnimation.AnimationType)
		{
			case AsvarduilAnimationType.Loop:
				if(IsAnimationDone)
					CurrentFrame = 0;
				break;
				
			case AsvarduilAnimationType.OneShot:
				if(IsAnimationDone)
				{
					_flipDirection = 0;
					CurrentFrame = _currentAnimation.Frames.Count - 1;
				}
				break;
				
			case AsvarduilAnimationType.PingPong:
				if(IsAnimationDone
				   || HasAnimationReset)
				{
					_flipDirection = -_flipDirection;
					
					if(_flipDirection > 0)
						CurrentFrame = 0;
					else
						CurrentFrame = _currentAnimation.Frames.Count - 1;
				}
				break;
				
			default:
				throw new Exception("Animation mode not supported: " + _currentAnimation.AnimationType);
		}
	}
	
	#endregion Methods
}
