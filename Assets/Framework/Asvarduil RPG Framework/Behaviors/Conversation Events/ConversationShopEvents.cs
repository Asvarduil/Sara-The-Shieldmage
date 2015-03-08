using UnityEngine;
using System.Collections.Generic;

public class ConversationShopEvents : DebuggableBehavior
{
    #region Variables / Properties

    private ShopController _shop;
    private ItemDatabase _itemDB;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _shop = ShopController.Instance;
        _itemDB = ItemDatabase.Instance;
    }

    public void OpenShop(List<string> args)
    {
        var shopInventory = new List<InventoryItem>();
        for(int i = 0; i < args.Count; i++)
        {
            string current = args[i];

            InventoryItem item = _itemDB.FindItemWithName(current);
            if (item == null)
            {
                DebugMessage("No item named " + current + " exists in the Item Database.");
                continue;
            }

            shopInventory.Add(item.Clone() as InventoryItem);
        }

        _shop.PresentShop(shopInventory);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
