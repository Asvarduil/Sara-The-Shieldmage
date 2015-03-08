using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class PartyInventory
{
	#region Variables / Properties
	
	public List<InventoryItem> Items;

	#endregion Variables / Properties

	#region Methods

	public void GainItem(InventoryItem item, int quantity = 1)
	{
		if(HasItem(item.Name))
		{
			InventoryItem heldItem = FindItem(item.Name);
			heldItem.Quantity += quantity;
		}
		else
		{
			Items.Add(item);
			item.Quantity = quantity;
		}
	}

	public void LoseItem(InventoryItem item, int quantity = 1)
	{
		if(! HasItem(item.Name))
			throw new ArgumentException("Cannot lose item " + item.Name + " as there are already none!");

		item.Quantity -= quantity;
	}

	public bool HasItem(string name)
	{
		return Items.Any(i => i.Name == name);
	}

	public bool HasItemsOfType(ItemType type)
	{
		return Items.Any(i => i.ItemType == type);
	}

	public InventoryItem FindItem(string name)
	{
		return Items.FirstOrDefault(i => i.Name == name);
	}
	
	public IEnumerable<InventoryItem> FindItemByType(ItemType type)
	{
		return Items.Where(i => i.ItemType == type);
	}

	public int CountOfItemsByType(ItemType type)
	{
		return Items.Count(i => i.ItemType == type);
	}

	public InventoryItem FindNextItemByType(ItemType type, int searchFrom)
	{
		int index = FindPositionOfNextItemOfType(type, searchFrom);
		if(index == -1)
			return null;

		return Items[index];
	}

	public int FindPositionOfNextItemOfType(ItemType type, int searchFrom)
	{
		// Search forward...
		int position = FindPositionOfNextAvailableItemByType(type, searchFrom, Items.Count);
		if(position == -1)
		{
			// It wasn't found.  Search from start to origin.
			position = FindPositionOfNextAvailableItemByType(type, 0, searchFrom);
		}

		return position;
	}

	public int FindPositionOfNextAvailableItemByType(ItemType type, int searchFrom, int searchTo)
	{
		int position = -1;
		for(int i = searchFrom; i < searchTo; i++)
		{
			if(i < 0 || i >= Items.Count)
				break;
			
			if(Items[i].Quantity > 0
			   && Items[i].ItemType == type)
			{
				position = i;
				break;
			}
		}

		return position;
	}

	public int FindPositionOfNextAvailableItem(bool searchForward, int searchSource)
	{
		int result = -1;

		if(searchForward)
		{
			result = FindPositionOfNextAvailableItemInRange(searchSource + 1, Items.Count - 1, 1);
			if(result != -1)
				return result;
		}
		else
		{
			result = FindPositionOfNextAvailableItemInRange(searchSource - 1, 0, -1);
			if(result != -1)
				return result;
		}

		return -1;
	}

	public int FindPositionOfNextAvailableItemInRange(int searchFrom, int searchTo, int searchDirection)
	{
		for(int i = searchFrom; i <= searchTo; i += searchDirection)
		{
			if(i < 0 || i >= Items.Count)
				break;

			InventoryItem item = Items[i];
			if(item.IsAvailable)
				return i;
		}

		return -1;
	}

	public int FindPositionOfItemByTypeInPositionRange(ItemType type, int searchFrom, int searchTo)
	{
		for(int i = searchFrom; i <= searchTo; i++)
		{
			if(i < 0 || i >= Items.Count)
				break;

			InventoryItem item = Items[i];
			if(item.ItemType == type
			   && item.IsAvailable)
				return i;
		}
		
		return -1;
	}

    public List<InventoryItem> HeldItems()
    {
        List<InventoryItem> results = new List<InventoryItem>();

        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItem current = Items[i];
            if (current.Quantity == 0)
                continue;

            results.Add(current);
        }

        return results;
    }

	#endregion Methods
}
