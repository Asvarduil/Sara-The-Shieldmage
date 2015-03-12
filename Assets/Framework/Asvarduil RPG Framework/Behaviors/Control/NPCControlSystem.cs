using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCControlSystem : DebuggableBehavior
{
    #region Variables / Properties

    public NPCControlState ActionState;
    public List<NPCAnimationState> AnimationStates;

    private AsvarduilSpriteSystem _sprite;
    private PedestrianMovement _movement;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _movement = GetComponent<PedestrianMovement>();
        _sprite = GetComponentInChildren<AsvarduilSpriteSystem>();
    }

    public void Update()
    {
        MoveCharacter();
        UpdateAnimation();
    }

    #endregion Hooks

    #region Methods

    private void MoveCharacter()
    {
        // TODO: Implement
    }

    private void UpdateAnimation()
    {
        string animation = GetAnimationForCurrentState();
        _sprite.SetAnimation(animation);
    }

    private string GetAnimationForCurrentState()
    {
        string result = string.Empty;
        bool foundAnimation = false;
        for(int i = 0; i < AnimationStates.Count; i++)
        {
            NPCAnimationState current = AnimationStates[i];
            if(ActionState == current.State)
            {
                result = current.Animation;
                foundAnimation = true;
                break;
            }

            continue;
        }

        if (!foundAnimation)
        {
            string exceptionMessage = string.Format("NPC State [{0}] does not exist in the animation/state list.",
                                                    ActionState);

            throw new InvalidOperationException(exceptionMessage);
        }

        return result;
    }

    #endregion Methods
}
