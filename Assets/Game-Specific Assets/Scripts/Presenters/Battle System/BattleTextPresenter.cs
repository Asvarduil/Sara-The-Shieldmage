using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleTextPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public float Duration;
    public Text MessageLabel;

    private float _shownAt;

    #endregion Variables / Properties

    #region Hooks

    public void Update()
    {
        if (Time.time < _shownAt + Duration)
            return;

        PresentGUI(false);
        _shownAt = Time.time;
    }

    public void ShowMessage(string messageText)
    {
        if (string.IsNullOrEmpty(messageText))
            return;

        MessageLabel.text = messageText;
        PresentGUI(true);
        _shownAt = Time.time;
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
