using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopGridPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Button LastButton;
    public Button NextButton;
    public Text PageLabel;

    public List<Button> GridButtons;

    private List<InventoryItem> _gridItems;
    private ShopController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _controller = ShopController.Instance;
    }

    public void LoadItems(List<InventoryItem> newItems)
    {
        _gridItems = newItems;

        // TODO: Determine whether or not to show/hide
        //       the Paging System.

        // TODO: Update buttons

        // TODO: Hide unused buttons
    }

    public void ShowItem(int index)
    {
        _controller.PresentItem(_gridItems[index]);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
