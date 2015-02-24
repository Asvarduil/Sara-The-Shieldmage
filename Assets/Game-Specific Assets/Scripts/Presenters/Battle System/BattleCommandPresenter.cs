using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleCommandPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public AudioClip PromptChime;
    public List<Button> CommandButtons;

    private PlayableCharacter _character;
    private Ability _selectedAbility;
    private List<Ability> _abilities;

    private BattleReferee _referee;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _referee = BattleReferee.Instance;
    }

    public void Prompt(PlayableCharacter character)
    {
        _maestro.PlayOneShot(PromptChime);
        _character = character;

        PresentGUI(true);
        LoadAbilities();
    }

    public void SelectAbility(int index)
    {
        _selectedAbility = _abilities[index];

        switch (_selectedAbility.TargetType)
        {
            case AbilityTargetType.Self:
                ApplyTarget(_character);
                break;

            case AbilityTargetType.TargetEnemy:
            case AbilityTargetType.TargetAlly:
                _referee.PromptForTarget(_selectedAbility.TargetType);
                // TODO: Request target from the Controller.
                break;

            case AbilityTargetType.AllAlly:
            case AbilityTargetType.AllEnemy:
            case AbilityTargetType.All:
                _referee.UseAbilityOnAllTargets(_selectedAbility, _character);
                break;

            default:
                DebugMessage("Target Type: " + _selectedAbility.TargetType + " not implemented.");
                return;
        }
    }

    public void ApplyTarget(CombatEntity target)
    {
        _referee.UseAbility(_selectedAbility, _character, target);
        PresentGUI(false);
    }

    #endregion Hooks

    #region Methods

    private void LoadAbilities()
    {
        _abilities = _character.AvailableAbilities;

        // Load available moves...
        for(int i = 0; i < _abilities.Count; i++)
        {
            Button current = CommandButtons[i];

            Text childText = current.GetComponentInChildren<Text>();
            childText.text = _abilities[i].Name;

            ActivateButton(current, true);
        }

        // Hide unassigned buttons.
        for (int i = CommandButtons.Count - 1; i > _abilities.Count - 1; i--)
        {
            Button current = CommandButtons[i];
            ActivateButton(current, false);
        }
    }

    #endregion Methods
}
