using UnityEngine;
using UnityEngine.UI;

public class SelectedItemPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text ItemName;
    public Text ItemDescription;
    public Button UseItemButton;
    public Button EquipItemButton;

    private InventoryItem _item;
    private PauseController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();

        _controller = PauseController.Instance;
    }

    public void LoadItem(InventoryItem item)
    {
        _item = item;
        ItemName.text = item.Name;
        ItemDescription.text = item.Description;

        switch(item.ItemType)
        {
            case ItemType.Consumable:
                ActivateButton(UseItemButton, true);
                ActivateButton(EquipItemButton, false);
                break;

            case ItemType.KeyItem:
                ActivateButton(UseItemButton, false);
                ActivateButton(EquipItemButton, false);
                break;

            case ItemType.Weapon:
            case ItemType.Armor:
            case ItemType.Accessory:
                ActivateButton(UseItemButton, false);
                ActivateButton(EquipItemButton, true);
                break;

            default:
                ActivateButton(UseItemButton, false);
                ActivateButton(EquipItemButton, false);
                break;
        }
    }

    public void UseItem()
    {
        _controller.UseItem(_item);
    }

    public void EquipItem()
    {
        _controller.EquipItem(_item);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
