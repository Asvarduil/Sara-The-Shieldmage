using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text HealthLabel;
    public Text AttackLabel;
    public Text MagicLabel;
    public Text MaxATBLabel;
    public Text ATBSpeedLabel;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void ReloadCharacterStats(PlayableCharacter character)
    {
        int hp = character.GetModifiedStatValue("HP");
        int maxHP = character.GetModifiedStatValue("Max HP");
        HealthLabel.text = string.Format("{0}/{1}", hp, maxHP);

        MagicLabel.text = character.GetModifiedStatValue("Magic").ToString();
        AttackLabel.text = character.GetModifiedStatValue("Attack").ToString();
        MaxATBLabel.text = character.GetModifiedStatValue("Max ATB").ToString();
        ATBSpeedLabel.text = character.GetModifiedStatValue("ATB Speed").ToString();
    }

    #endregion Methods
}
