using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationItemEvents : ConversationEventBase
{
	#region Variables / Properties

	private ItemDatabase _itemDatabase;
	private InventoryManager _inventory;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		_itemDatabase = ItemDatabase.Instance;
		_inventory = InventoryManager.Instance;
        base.Start();
	}

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("GainItem", GainItem);
    }

	#endregion Engine Hooks

	#region Messages

	public IEnumerator GainItem(List<string> args)
	{
		if(args.IsNullOrEmpty())
			throw new ArgumentNullException("GainItem requires a list containing the item to gain, and the quantity.");

		string itemName = args[0];
		int quantity = Convert.ToInt32(args[1]);

		InventoryItem item = _itemDatabase.FindItemWithName(itemName);
		if(item != default(InventoryItem))
			_inventory.GainItem(item, quantity);

		DebugMessage("Could not find item " + itemName + " in the inventory database.", LogLevel.LogicError);
        yield return 0;
	}

	#endregion Messages
}
