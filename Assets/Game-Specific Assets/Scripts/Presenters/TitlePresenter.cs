using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TitlePresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public SceneState NewGameSceneState;
    public AudioClip NewGameSoundEffect;

    private Fader _fader;

    private PartyManager _party;
    private InventoryManager _inventory;
    private TransitionManager _transition;

    private AbilityDatabase _abilityDB;
    private ItemDatabase _itemDB;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();

        _itemDB = ItemDatabase.Instance;
        _abilityDB = AbilityDatabase.Instance;

        _party = PartyManager.Instance;
        _inventory = InventoryManager.Instance;
        _transition = TransitionManager.Instance;
    }

    public void NewGame()
    {
        _maestro.PlayOneShot(NewGameSoundEffect);
        _transition.PrepareSceneChange(NewGameSceneState, false);
        _transition.ChangeScenes();
    }

    public void ExitGame()
    {
        StartCoroutine(QuitGame());
    }

    #endregion Hooks

    #region Methods

    private IEnumerator QuitGame()
    {
        _fader = FindObjectOfType<Fader>();
        _fader.FadeOut();

        _maestro.FadeOut();

        while (!_fader.ScreenHidden)
            yield return 0;

        Application.Quit();
    }

    #endregion Methods
}
