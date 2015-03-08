using System.Collections.Generic;
using UnityEngine;

public class ShopController : ManagerBase<ShopController>
{
    #region Variables / Properties

    public string CurrencyName = "Saphirium";

    private ShopMainPresenter _main;
    private ShopGridPresenter _grid;
    private ShopItemPresenter _item;

    private bool _buyMode = false;
    private List<InventoryItem> _houseInventory;

    private ControlManager _controls;
    private InventoryManager _inventory;

    private InventoryItem _currency;
    public InventoryItem Currency
    {
        get { return _currency ?? (_currency = _inventory.ActiveInventory.FindItem(CurrencyName)); }
    }

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
        _main.UpdateCurrencyLabel();

        _grid.PresentGUI(false);
        _item.PresentGUI(false);
    }

    public void PresentGrid(bool isBuying)
    {
        _grid.PresentGUI(true);

        _buyMode = isBuying;
        List<InventoryItem> inventory;
        if (isBuying)
        {
            inventory = _houseInventory;
        }
        else
        {
            var items = _inventory.ActiveInventory.HeldItems();
            inventory = FilterKeyItemsOut(items);
        }

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
        Currency.Quantity -= price;
        _inventory.ActiveInventory.GainItem(item, quantity);

        _item.PresentGUI(false);
        PresentGrid(_buyMode);
        _main.UpdateCurrencyLabel();
    }

    public void PerformSale(InventoryItem item, int quantity, int price)
    {
        Currency.Quantity += price;
        _inventory.ActiveInventory.LoseItem(item, quantity);

        _item.PresentGUI(false);
        PresentGrid(_buyMode);
        _main.UpdateCurrencyLabel();
    }

    #endregion Hooks

    #region Methods

    public List<InventoryItem> FilterKeyItemsOut(List<InventoryItem> items)
    {
        List<InventoryItem> results = new List<InventoryItem>();

        for(int i = 0; i < items.Count; i++)
        {
            InventoryItem current = items[i];
            if (current.ItemType == ItemType.KeyItem)
                continue;

            results.Add(current);
        }

        return results;
    }

    #endregion Methods
}
