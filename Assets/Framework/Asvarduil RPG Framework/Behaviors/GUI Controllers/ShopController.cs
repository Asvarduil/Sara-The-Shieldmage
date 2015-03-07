using System.Collections.Generic;
using UnityEngine;

public class ShopController : ManagerBase<ShopController>
{
    #region Variables / Properties

    private ShopMainPresenter _main;
    private ShopGridPresenter _grid;
    private ShopItemPresenter _item;

    private bool _buyMode = false;
    private List<InventoryItem> _houseInventory;

    private ControlManager _controls;
    private InventoryManager _inventory;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _controls = ControlManager.Instance;
        _inventory = InventoryManager.Instance;

        _main = GetComponentInChildren<ShopMainPresenter>();
        _grid = GetComponentInChildren<ShopGridPresenter>();
        _item = GetComponentInChildren<ShopItemPresenter>();
    }

    public void StopShopping()
    {
        _controls.RadiateResumeCommand();

        _main.PresentGUI(false);
        _grid.PresentGUI(false);
        _item.PresentGUI(false);
    }

    public void PresentShop(List<InventoryItem> inventory)
    {
        _controls.RadiateSuspendCommand();

        _houseInventory = inventory;

        _main.PresentGUI(true);
        _grid.PresentGUI(false);
        _item.PresentGUI(false);
    }

    public void PresentGrid(bool isBuying)
    {
        _grid.PresentGUI(true);

        _buyMode = isBuying;
        List<InventoryItem> inventory = isBuying ? _houseInventory : _inventory.ActiveInventory.Items;

        _grid.LoadItems(inventory);

        _item.PresentGUI(false);
    }

    public void PresentItem(InventoryItem item)
    {
        _item.PresentGUI(true);
        _item.LoadItem(item, _buyMode);
    }

    public void PerformPurchase(InventoryItem item, int quantity, int price)
    {
        // TODO: Remove [price] Saphirium
        // TODO: Add [quantity] of [item]

        _item.PresentGUI(false);
    }

    public void PerformSale(InventoryItem item, int quantity, int price)
    {
        // TODO: Add [price] Saphirium
        // TODO: Remove [quantity] of [item]

        _item.PresentGUI(false);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
