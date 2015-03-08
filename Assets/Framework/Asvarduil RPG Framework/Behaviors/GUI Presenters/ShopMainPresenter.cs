using UnityEngine.UI;

public class ShopMainPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text CurrencyLabel;

    private ShopController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _controller = ShopController.Instance;
    }

    public void UpdateCurrencyLabel()
    {
        int funds = _controller.Currency.Quantity;
        CurrencyLabel.text = string.Format("{0} {1}", funds, _controller.Currency.Name);
    }

    public void BuyItems()
    {
        _controller.PresentGrid(true);
    }

    public void SellItems()
    {
        _controller.PresentGrid(false);
    }

    public void StopShopping()
    {
        _controller.StopShopping();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
