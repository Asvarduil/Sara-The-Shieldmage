using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class AsvarduilSpriteSystem : DebuggableBehavior
{
	#region Variables / Properties

	public bool AutomaticallyPlay = true;
	public string TextureKey = "_MainTex";
	public List<AsvarduilSpriteAnimation> Animations;

	private AsvarduilSpriteAnimation _currentAnimation;
	private int _flipDirection = 1;
	private float _lastFrameChange = 0.0f;
	private Material _workingMaterial;

	public int CurrentFrame = 0;

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

	public void Start()
	{
		_workingMaterial = renderer.material;

		if(Animations == null
		   || Animations.Count == 0
		   || Animations[0] == null)
			throw new Exception("An animation system must have at least one animation!");

		_currentAnimation = Animations[0];
	}

	public void Update()
	{
		if(! AutomaticallyPlay)
			return;

		if(! CanChangeFrames)
			return;

		IncrementFrame();
		DrawCurrentFrame();
	}

	public void OnDestroy()
	{
		_workingMaterial = null;
	}

	#endregion Engine Hooks

	#region Methods

	public void SetAnimation(string animation)
	{
		AsvarduilSpriteAnimation newAnimation = Animations.FirstOrDefault(a => a.Name == animation);
		if(newAnimation == default(AsvarduilSpriteAnimation))
		{
			DebugMessage("Could not find animation " + animation, LogLevel.LogicError);
		}

		_currentAnimation = newAnimation;
	}

	public void PlaySingleFrame(string animation)
	{
		if(string.IsNullOrEmpty(animation))
			throw new ArgumentException("Must specify an animation.");

		if(_currentAnimation == null
		   || _currentAnimation.Name != animation)
		{
			_currentAnimation = Animations.FirstOrDefault(a => a.Name == animation);
			ResetAnimation();
			DrawCurrentFrame();
			return;
		}

		if(CanChangeFrames)
		{
			IncrementFrame();
			DrawCurrentFrame();
		}
	}

	private void ResetAnimation()
	{
		CurrentFrame = 0;
		_flipDirection = 1;
	}

	private void DrawCurrentFrame()
	{
		_lastFrameChange = Time.time;

		Texture2D tex = _currentAnimation.Frames[CurrentFrame].Content;
		DebugMessage("_currentAnimation.Frames[" + CurrentFrame + "].Content is null? " + (tex == null ? "Null!" : "Not Null."));

		if(_workingMaterial == null)
			return;

		_workingMaterial.SetTexture(TextureKey, tex);
		DebugMessage("_workingMaterial is null? " + (_workingMaterial == null ? "Null!" : "Not Null."));

		renderer.material = _workingMaterial;
	}

	private void IncrementFrame()
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
					_flipDirection = 0;
				break;

			case AsvarduilAnimationType.PingPong:
				if(IsAnimationDone
			       || HasAnimationReset)
				{
					_flipDirection = -_flipDirection;
				}
				break;

			default:
				throw new Exception("Animation mode not supported: " + _currentAnimation.AnimationType);
		}
	}

	private void PushWorkingMaterialToRenderer()
	{
		renderer.material = _workingMaterial;
	}

	#endregion Methods
}
