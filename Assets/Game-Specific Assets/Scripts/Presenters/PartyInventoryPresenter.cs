using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartyInventoryPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Button LastPageButton;
    public Button NextPageButton;
    public Text PageLabel;

    public int ItemsPerPage = 6;
    public List<Button> ItemButtons;

    private PauseController _controller;

    private int _pageId = 0;
    private List<InventoryItem> _items;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();

        _controller = PauseController.Instance;
    }

    public void LoadItems(List<InventoryItem> items)
    {
        bool enablePaging = items.Count > ItemsPerPage;
        ActivateButton(LastPageButton, enablePaging);
        ActivateText(PageLabel, enablePaging);
        ActivateButton(NextPageButton, enablePaging);

        _pageId = 0;
        _items = items;
        UpdatePageLabelText();
        RefreshGrid();
    }

    private void RefreshGrid()
    {
        for (int i = 0; i < ItemsPerPage; i++)
        {
            Button current = ItemButtons[i];

            int itemIndex = (_pageId * ItemsPerPage ) + i;
            if (itemIndex > _items.Count - 1)
            {
                DebugMessage("Hiding button #" + (i + 1));
                ActivateButton(current, false);
                continue;
            }

            InventoryItem item = _items[itemIndex];
            Text buttonText = current.GetComponentInChildren<Text>();
            buttonText.text = item.Name + " x" + item.Quantity;
            ActivateButton(current, true);
        }
    }

    public void LastPage()
    {
        if (_pageId == 0)
            return;

        _pageId--;
        UpdatePageLabelText();
        RefreshGrid();
    }

    public void NextPage()
    {
        if (_pageId * ItemsPerPage > _items.Count)
            return;

        _pageId++;
        UpdatePageLabelText();
        RefreshGrid();
    }

    private void UpdatePageLabelText()
    {
        PageLabel.text = "Page " + (_pageId + 1);
    }

    public void SelectItem(int buttonIndex)
    {
        int itemIndex = (_pageId * ItemsPerPage) + buttonIndex;
        InventoryItem item = _items[itemIndex];

        _controller.SelectItem(item);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
