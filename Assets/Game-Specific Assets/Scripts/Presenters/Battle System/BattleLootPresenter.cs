using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

public class BattleLootPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text LootLabel;

    private BattleReferee _referee;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _referee = BattleReferee.Instance;
    }

    public void ShowLoot(List<InventoryItem> items)
    {
        LootLabel.text = GenerateLootString(items);
        PresentGUI(true);
    }

    public void EndBattle()
    {
        _referee.ReturnToOriginalScene();
    }

    #endregion Hooks

    #region Methods

    private string GenerateLootString(List<InventoryItem> items)
    {
        StringBuilder builder = new StringBuilder("Got ");
        for (int i = 0; i < items.Count; i++)
        {
            if (i > 0 && i == items.Count - 1)
                builder.Append("and ");

            InventoryItem item = items[i];
            builder.Append(item.Quantity);
            builder.Append(" ");
            builder.Append(item.Name);

            if (i < items.Count - 1)
                builder.Append(", ");
        }

        return builder.ToString();
    }

    #endregion Methods
}
