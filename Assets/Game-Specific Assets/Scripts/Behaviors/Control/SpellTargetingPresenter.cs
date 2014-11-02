using UnityEngine;
using System;
using System.Collections.Generic;

public class SpellTargetingPresenter : PresenterBase
{
	#region Variables / Properties

	public bool isPlacingSpell = false;
	public AsvarduilCursorAnimation DefaultCursor;
	public AsvarduilCursorAnimation AvailableCursor;
	public AsvarduilCursorAnimation UnavailableCursor;

	private AsvarduilCursorAnimation _currentCursor;

	public float PlacementDepth = 6.5f;
	public int CurrentFrame = 0;
	private int _flipDirection = 1;
	private float _lastFrameChange = 0.0f;

	private int FlipBoundary
	{
		get { return _flipDirection > 0 ? _currentCursor.Frames.Count - 1 : 0; }
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
		get 
		{ 
			return Time.time >= _lastFrameChange + _currentCursor.Frames[CurrentFrame].AnimationDelay; 
		}
	}

	public Vector3 SpellPosition
	{
		get 
		{
			Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, PlacementDepth);
			Vector3 worldSpace = Camera.main.ScreenToWorldPoint(mousePosition);

			return worldSpace;
		}
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();
		SetCursorToDefault();
	}

	#endregion Engine Hooks

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		isPlacingSpell = isVisible;

		if(! isPlacingSpell)
			SetCursorToDefault();
	}

	public override void Tween()
	{
		IncrementFrame();
	}

	public override void DrawMe()
	{
		if(! CanChangeFrames)
			return;

		DrawCurrentFrame();
	}

	#endregion Hooks

	#region Methods

	public void SetCursorToDefault()
	{
		_currentCursor = DefaultCursor;
		CurrentFrame = 0;
	}

	public void UpdateTargetingCursor(bool canCastSpell)
	{
		if(! isPlacingSpell)
			return;

		AsvarduilCursorAnimation newCursorAnimation = canCastSpell ? AvailableCursor : UnavailableCursor;
		if(_currentCursor.Name == newCursorAnimation.Name)
			return;

		_currentCursor = newCursorAnimation;
		CurrentFrame = 0;
	}

	private void DrawCurrentFrame()
	{
		_lastFrameChange = Time.time;

		AsvarduilCursorFrame frame = _currentCursor.Frames[CurrentFrame];
		Texture2D tex = frame.Content;

		Cursor.SetCursor(tex, frame.HotSpot, CursorMode.Auto);
	}
	
	private void IncrementFrame()
	{
		CurrentFrame += _flipDirection;

		switch(_currentCursor.AnimationType)
		{
			case AsvarduilAnimationType.None:
				CurrentFrame = 0;
				break;

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
				throw new Exception("Animation mode not supported: " + _currentCursor.AnimationType);
		}
	}

	#endregion Methods
}
