using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopItemPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public string CurrencyName = "Saphirium";

    public Text ItemName;
    public Text ItemDescription;
    public Text QuantityPriceLabel;
    public Button IncreaseQuantityButton;
    public Button DecreaseQuantityButton;
    public Button BuyButton;
    public Button SellButton;

    private bool _buyMode = false;
    private int _quantity = 1;
    private int _price = 0;

    private InventoryItem _item;

    private InventoryItem _currency;

    private ShopController _controller;
    private InventoryManager _inventory;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _controller = ShopController.Instance;
        _inventory = InventoryManager.Instance;

        _currency = _inventory.ActiveInventory.FindItem(CurrencyName);
    }

    public void LoadItem(InventoryItem item, bool buyMode)
    {
        _buyMode = buyMode;
        _quantity = 1;
        _item = item;

        UpdateBuySellButton();
        UpdateItemLabels();
        UpdateQuantityPriceLabel();
    }

    public void IncreaseQuantity(int step)
    {
        _quantity += step;

        UpdateQuantityPriceLabel();
        CheckIncreaseButtonAvailability();
    }

    public void DecreaseQuantity(int step)
    {
        _quantity -= step;

        UpdateQuantityPriceLabel();
        CheckDecreaseButtonAvailability();
    }

    public void BuyItem()
    {
        _controller.PerformPurchase(_item, _quantity, _price);
    }

    public void SellItem()
    {
        _controller.PerformSale(_item, _quantity, _price);
    }

    #endregion Hooks

    #region Methods

    private void UpdateBuySellButton()
    {
        if (_buyMode)
        {
            ActivateButton(SellButton, false);
            ActivateButton(BuyButton, true);
            return;
        }
        
        ActivateButton(SellButton, true);
        ActivateButton(BuyButton, false);
    }

    private void UpdateItemLabels()
    {
        ItemName.text = _item.Name;
        ItemDescription.text = _item.Description;
    }

    private void UpdateQuantityPriceLabel()
    {
        _price = _item.Value * _quantity;

        string sign = _buyMode ? "-" : "+";
        QuantityPriceLabel.text = string.Format("x{0} ({1} {2})", _quantity, sign, _price);
    }

    private void CheckIncreaseButtonAvailability()
    {
        bool showButton = false;
        if (_buyMode)
            showButton = _price < _currency.Quantity;
        else
            showButton = _quantity < _item.Quantity;

        ActivateButton(IncreaseQuantityButton, showButton);
    }

    private void CheckDecreaseButtonAvailability()
    {
        bool showButton = _quantity < _item.Quantity;
        ActivateButton(DecreaseQuantityButton, showButton);
    }

    private void CheckBuyButtonAvailability()
    {
        if (!_buyMode)
        {
            ActivateButton(BuyButton, false);
            return;
        }

        bool showButton = _price <= _currency.Quantity;
        ActivateButton(BuyButton, showButton);
    }

    private void CheckSellButtonAvailability()
    {
        bool showButton = _buyMode ? false : true;
        ActivateButton(SellButton, showButton);
    }

    #endregion Methods
}
