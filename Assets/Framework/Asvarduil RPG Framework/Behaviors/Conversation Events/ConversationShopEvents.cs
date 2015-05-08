using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class ConversationShopEvents : ConversationEventBase
{
    #region Variables / Properties

    private ShopController _shop;
    private ItemDatabase _itemDB;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _shop = ShopController.Instance;
        _itemDB = ItemDatabase.Instance;

        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("OpenShop", OpenShop);
    }

    public IEnumerator OpenShop(List<string> args)
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

        yield return null;
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
