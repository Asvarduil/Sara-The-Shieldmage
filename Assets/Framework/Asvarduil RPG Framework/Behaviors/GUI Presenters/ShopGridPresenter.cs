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

    private int _pageId;
    private List<InventoryItem> _gridItems;
    private ShopController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _controller = ShopController.Instance;
    }

    public void NextPage()
    {
        _pageId++;
        PresentPagingSystem();
        LoadGrid();
    }

    public void LastPage()
    {
        if (_pageId == 0)
            return;

        _pageId--;
        PresentPagingSystem();
        LoadGrid();
    }

    public void LoadItems(List<InventoryItem> newItems)
    {
        _pageId = 0;
        _gridItems = newItems;

        bool showPaging = _gridItems.Count > GridButtons.Count;
        ActivateButton(LastButton, showPaging);
        ActivateButton(NextButton, showPaging);
        ActivateText(PageLabel, showPaging);

        PresentPagingSystem();
        LoadGrid();
    }

    public void ShowItem(int index)
    {
        _controller.PresentItem(_gridItems[index]);
    }

    #endregion Hooks

    #region Methods

    private void PresentPagingSystem()
    {
        if (_gridItems.Count <= GridButtons.Count)
            return;

        ActivateButton(LastButton, _pageId > 0);
        ActivateButton(NextButton, _pageId < (_gridItems.Count / GridButtons.Count));

        PageLabel.text = string.Format("Page {0}", _pageId + 1);
    }

    private void LoadGrid()
    {
        for (int i = 0; i < GridButtons.Count; i++)
        {
            Button current = GridButtons[i];

            int itemIndex = (_pageId * GridButtons.Count) + i;
            if (itemIndex > _gridItems.Count - 1)
            {
                DebugMessage("Hiding button #" + (i + 1));
                ActivateButton(current, false);
                continue;
            }

            InventoryItem item = _gridItems[itemIndex];
            Text buttonText = current.GetComponentInChildren<Text>();
            buttonText.text = item.Name;
            ActivateButton(current, true);
        }
    }

    #endregion Methods
}
