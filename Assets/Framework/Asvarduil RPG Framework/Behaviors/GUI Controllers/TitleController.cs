using UnityEngine;
using UnityEngine.UI;

public class TitleController : ManagerBase<TitleController>
{
    #region Variables / Properties

    private TitlePresenter _title;
    private ContinuePresenter _continue;
    private SettingsPresenter _settings;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _title = GetComponentInChildren<TitlePresenter>();
        _continue = GetComponentInChildren<ContinuePresenter>();
        _settings = GetComponentInChildren<SettingsPresenter>();

        PresentTitleScreen();
    }

    public void PresentTitleScreen()
    {
        _title.PresentGUI(true);

        _continue.PresentGUI(false);
        _settings.PresentGUI(false);
    }

    public void PresentContinueScreen()
    {
        _continue.PresentGUI(true);

        _title.PresentGUI(false);
        _settings.PresentGUI(false);
    }

    public void PresentSettingsScreen()
    {
        _settings.PresentGUI(true);

        _title.PresentGUI(false);
        _continue.PresentGUI(false);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
