using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class ItemDatabase : DatabaseBase<ItemDatabase> 
{
	#region Variables / Properties

	public List<InventoryItem> Items;

	#endregion Variables / Properties

	#region Data Management Methods

	public InventoryItem FindItemWithName(string name)
	{
        InventoryItem result = null;
        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItem current = Items[i];
            if (current.Name != name)
                continue;

            result = current;
            break;
        }

        return result;
	}

	#endregion Data Management Methods

	#region Data Access Methods

	protected override void MapBlob()
	{
		var parsed = JSON.Parse(RawBlob);
        Items = parsed["Items"].AsArray.UnfoldJsonArray<InventoryItem>();

        IsLoaded = Items.Count > 0;
	}

	#endregion Data Access Methods
}
