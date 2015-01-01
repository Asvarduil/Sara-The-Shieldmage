using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class InventoryManager : ManagerBase<InventoryManager> 
{
	#region Variables / Properties

	public int ActiveInventoryId;

	// TODO: Create a domain object for a Party Inventory.
	public List<PartyInventory> PartyInventories;

	public PartyInventory ActiveInventory
	{
		get { return PartyInventories[ActiveInventoryId]; }
	}

	#endregion Variables / Properties

	#region Methods

	public void GainItem(InventoryItem item, int amount = 1)
	{
		ActiveInventory.GainItem(item, amount);

		int newAmount = ActiveInventory.FindItem(item.Name).Quantity;
		DebugMessage(string.Format("Added a {0} to the active inventory. (Quantity: {1})", item.Name, newAmount));
	}

	public void LoseItem(InventoryItem item, int amount = 1)
	{
		ActiveInventory.LoseItem(item, amount);

		int newAmount = ActiveInventory.FindItem(item.Name).Quantity;
		DebugMessage(string.Format("Removed a {0} from the active inventory. (Quantity: {1})", item.Name, newAmount));
	} 
 
	public InventoryItem FindNextItemOfType(ItemType type, int searchPosition = 0)
	{
		InventoryItem foundItem = ActiveInventory.FindNextItemByType(type, searchPosition);
		if(foundItem != null)
			DebugMessage("Found " + foundItem.Name + "!");
		else
			DebugMessage("The player has no items that are " + type);

		return foundItem;
	}

	public int FindPositionOfNextItemOfType(ItemType type, int searchPosition = 0)
	{
		int foundPosition = ActiveInventory.FindPositionOfNextItemOfType(type, searchPosition);
		DebugMessage("Found another available " + type + " at position #" + foundPosition);

		return foundPosition;
	}

	public int FindNextPositionOfAvailableItem(bool searchForward, int searchPosition = 0)
	{
		// First try: Search from our position to an endpoint depending on the search direction.
		int foundPosition = ActiveInventory.FindPositionOfNextAvailableItem(searchForward, searchPosition);
        //DebugMessage("Found an available item at position #" 
        //             + foundPosition + "/" + (ActiveInventory.Items.Count - 1)
        //             + " when searching " + (searchForward ? "forward" : "backward") 
        //             + " from position " + searchPosition);

		if(foundPosition == -1)
		{
			// ...So we didn't find it.  Try searching from an end point toward our original position,
			// depending on the direction of our search.
			if(searchForward)
				foundPosition = ActiveInventory.FindPositionOfNextAvailableItemInRange(0, searchPosition - 1, 1);
			else
				foundPosition = ActiveInventory.FindPositionOfNextAvailableItemInRange(ActiveInventory.Items.Count - 1, searchPosition + 1, -1);

            //DebugMessage("[Retry] Found an available item at position #" 
            //             + foundPosition + "/" + ActiveInventory.Items.Count
            //             + " when searching " + (searchForward ? "forward" : "backward") 
            //             + " from position " + searchPosition);

			if(foundPosition == -1)
				throw new Exception("Could not find an item at all.");
		}

		return foundPosition;
	}

	#endregion Methods
}
