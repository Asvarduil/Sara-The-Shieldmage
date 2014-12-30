using UnityEngine;
using System;
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
        GameObject.Instantiate(DeathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void ProvideFeedback(BattleEntityFeedbackType type, string value)
    {
        switch(type)
        {
            case BattleEntityFeedbackType.Animation:
                PlayAnimation(value);
                break;

            case BattleEntityFeedbackType.Flytext:
                EmitFlytext(value);
                break;

            default:
                throw new InvalidOperationException("Unexpected battle entity feedback type: " + type);
        }
    }

    private void PlayAnimation(string animation)
    {
        _sprite.SetAnimation(animation);
    }

    private void EmitFlytext(string text)
    {

    }

	#endregion Methods
}
