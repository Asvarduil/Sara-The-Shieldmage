using UnityEngine;
using System.Collections;

public class SceneChangeTrigger : DebuggableBehavior
{
    #region Variables / Properties

    public string AllowedTag = "Player";
    public SceneState TargetState;

    private TransitionManager _transition;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _transition = TransitionManager.Instance;
    }

    public void OnTriggerEnter(Collider who)
    {
        if (who.tag != AllowedTag)
            return;

        _transition.PrepareSceneChange(TargetState);
        _transition.ChangeScenes();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
