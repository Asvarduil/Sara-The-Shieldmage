using UnityEngine;
using UnityEngine.UI;

public class BattleCharacterStatPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Image Thumbnail;
    public Gauge HealthGauge;
    public Gauge ATBGauge;

    public int MaxHealthGaugeSize = 100;
    public int MaxATBGaugeSize = 100;

    public CombatEntity Character;

    private const float gaugeTheta = 0.01f;

    #endregion Variables / Properties

    #region Hooks

    public void BindCharacter(CombatEntity character)
    {
        Character = character;
        // TODO: Associate character thumbnail...
    }

    public void UpdateATB()
    {
        int ATB = Character.GetStatByName("ATB").Value;
        int MaxATB = Character.GetStatByName("Max ATB").Value;

        ATBGauge.RecalculateGaugeSize(ATB, MaxATB);
    }

    public void UpdateHealth()
    {
        int HP = Character.Health.HP;
        int MaxHP = Character.Health.MaxHP;

        HealthGauge.RecalculateGaugeSize(HP, MaxHP);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
