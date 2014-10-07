using UnityEngine;
using System.Collections;

public abstract class SidescrollingAnimationController : DebuggableBehavior
{
	#region Variables / Properties

	protected string _currentAnimation;
	protected SidescrollingMovement _movement;
	protected AsvarduilSpriteSystem _sprite;

	#endregion Variables / Properties

	#region Engine Hooks

	public virtual void Start()
	{
		_movement = GetComponent<SidescrollingMovement>();
		_sprite = GetComponentInChildren<AsvarduilSpriteSystem>();
	}

	#endregion Engine Hooks

	#region Methods

	protected abstract void SelectCurrentAnimation();

	public void Animate()
	{
		SelectCurrentAnimation();
		_sprite.PlaySingleFrame(_currentAnimation);
	}

	#endregion Methods
}
