using UnityEngine;
using System.Collections;

public class OneShotSoundEffect : DebuggableBehavior
{
    #region Variables / Properties

    public AudioClip SoundEffect;

    private Maestro _maestro;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _maestro = Maestro.Instance;
        _maestro.PlayOneShot(SoundEffect);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
