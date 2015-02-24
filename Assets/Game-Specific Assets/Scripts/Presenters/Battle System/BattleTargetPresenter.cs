using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleTargetPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public List<Button> TargetButtons;

    private BattleReferee _referee;
    private List<CombatEntity> _targets;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();

        _referee = BattleReferee.Instance;
    }

    public void Prompt(List<CombatEntity> targets, string labelName)
    {
        PresentGUI(true);

        _targets = targets;
        LoadTargets();
    }

    public void ApplyTarget(int index)
    {
        PresentGUI(false);

        var target = _targets[index];
        _referee.ApplyTargetToPlayerCommand(target);
    }

    #endregion Hooks

    #region Methods

    private void LoadTargets()
    {
        // Load all targets...
        for(int i = 0; i < _targets.Count; i++)
        {
            Button current = TargetButtons[i];

            Text childText = current.GetComponentInChildren<Text>();
            childText.text = _targets[i].Name;

            ActivateButton(current, true);
        }

        // Hide all unloaded buttons.
        for (int i = TargetButtons.Count - 1; i > _targets.Count - 1; i--)
        {
            Button current = TargetButtons[i];
            ActivateButton(current, false);
        }
    }

    #endregion Methods
}
