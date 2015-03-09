using UnityEngine;
using System.Collections;

public class DoorInterface : DebuggableBehavior
{
    #region Variables / Properties

    public string AffectedTag = "Player";
    public string InteractText = "Talk";

    public SceneState TargetState;

    private TransitionManager _transition;
    private DialogueController _controller;

    #endregion Variables / Properties

    #region Hooks

    public virtual void Start()
    {
        _transition = TransitionManager.Instance;
        _controller = DialogueController.Instance;
    }

    public virtual void OnInteraction()
    {
        _transition.PrepareSceneChange(TargetState);
        _transition.ChangeScenes();
    }

    public void OnTriggerEnter(Collider who)
    {
        if (who.tag != AffectedTag)
            return;

        _controller.PrepareInteraction(InteractText, () => OnInteraction());
    }

    public void OnTriggerExit(Collider who)
    {
        if (who.tag != AffectedTag)
            return;

        _controller.ClearInteraction();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
