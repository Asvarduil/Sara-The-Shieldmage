using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectMemberPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text MemberNameLabel;
    public Button LastButton;
    public Button NextButton;

    private PauseController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _controller = PauseController.Instance;
    }

    public void PresentButtons(bool isNecessary)
    {
        ActivateButton(LastButton, isNecessary);
        ActivateButton(NextButton, isNecessary);
    }

    public void NextMember()
    {
        _controller.ChangeMember(1);
    }

    public void LastMember()
    {
        _controller.ChangeMember(-1);
    }

    public void UpdateMemberName(PlayableCharacter character)
    {
        MemberNameLabel.text = character.Name;
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
