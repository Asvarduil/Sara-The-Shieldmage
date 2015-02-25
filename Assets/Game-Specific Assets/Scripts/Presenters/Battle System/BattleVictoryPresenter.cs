using UnityEngine;
using System.Collections;

public class BattleVictoryPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    private BattleReferee _referee;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();

        _referee = BattleReferee.Instance;
    }

    public void ContinueToLoot()
    {
        PresentGUI(false);
        _referee.RollForLoot();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
