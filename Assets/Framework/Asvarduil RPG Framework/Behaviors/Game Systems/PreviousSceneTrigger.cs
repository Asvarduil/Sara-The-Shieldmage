using UnityEngine;
using System.Collections;

public class PreviousSceneTrigger : DebuggableBehavior
{
    #region Variables / Properties

    public string AllowedTag = "Player";

    private Fader _fader;
    private Maestro _maestro;
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

        _transition.ChangeScenes(true);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
