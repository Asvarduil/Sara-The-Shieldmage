using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum BattleEntityFeedbackType
{
    Flytext,
    Animation
}

public class BattleEntity : DebuggableBehavior
{
	#region Variables / Properties

    public string CharacterName;
    public AIBase AI;
    public GameObject DeathEffect;
    public string DeathAnimation;

    private AsvarduilSpriteSystem _sprite;

	#endregion Variables / Properties

	#region Hooks

	public void Start()
	{
        _sprite = GetComponentInChildren<AsvarduilSpriteSystem>();
	}

	#endregion Hooks

	#region Methods

    public void DoDeathSequence()
    {
        if (DeathEffect != null)
            GameObject.Instantiate(DeathEffect, transform.position, Quaternion.identity);

        if (string.IsNullOrEmpty(DeathAnimation))
            gameObject.SetActive(false);
        else
            _sprite.SetAnimation(DeathAnimation);
    }

    public void PlayAnimation(string animation)
    {
        _sprite.SetAnimation(animation);
    }

    private void EmitFlytext(string text)
    {

    }

	#endregion Methods
}
