using UnityEngine;
using System.Collections;

public class TitleController : ManagerBase<TitleController>
{
    #region Variables / Properties

    private TitlePresenter _title;
    //private SettingsPresenter _settings;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _title = GetComponentInChildren<TitlePresenter>();
    }

    public void PresentTitleScreen()
    {
        _title.PresentElements();
    }

    public void PresentContinueScreen()
    {

    }

    public void PresentSettingsScreen()
    {

    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
