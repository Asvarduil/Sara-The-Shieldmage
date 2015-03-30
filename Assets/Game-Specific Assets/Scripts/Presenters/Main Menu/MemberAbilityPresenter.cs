using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MemberAbilityPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public List<Button> AbilityButtons;
    public Text AbilityNameLabel;
    public Text AbilityDescriptionLabel;

    private PauseController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();

        _controller = PauseController.Instance;
    }

    public void LoadAbilities(List<Ability> abilities)
    {
        DebugMessage(string.Format("Loading {0} abilities into {1} buttons.", abilities.Count, AbilityButtons.Count));

        for (int i = 0; i < AbilityButtons.Count; i++)
        {
            Ability current = (i < abilities.Count)
                ? abilities[i]
                : null;

            MapAbilityToAbilityButton(current, AbilityButtons[i]);
        }
    }

    public void LoadAbilityAtIndex(int index)
    {
        Ability ability = _controller.GetAbilityAtIndexOnCurrentMember(index);

        if (ability != null
            && ability.Available)
        {
            DebugMessage("Loaded ability #" + (index + 1) + ", " + ability.Name);

            AbilityNameLabel.text = ability.Name + " (" + ability.AtbCost + " ATB)";
            AbilityDescriptionLabel.text = ability.Description;
        }
        else
        {
            DebugMessage("Ability #" + (index + 1) + " is not available or does not exist.");

            AbilityNameLabel.text = "Unknown Ability";
            AbilityDescriptionLabel.text = "Find the quest to learn this ability, and complete it!";
        }
    }

    #endregion Hooks

    #region Methods

    private void MapAbilityToAbilityButton(Ability ability, Button button)
    {
        Text childText = button.GetComponentInChildren<Text>();
        childText.text = GetAbilityButtonText(ability);
    }

    private string GetAbilityButtonText(Ability ability)
    {
        if (ability == null
            || !ability.Available)
            return "???";

        return ability.Name;
    }

    #endregion Methods
}
