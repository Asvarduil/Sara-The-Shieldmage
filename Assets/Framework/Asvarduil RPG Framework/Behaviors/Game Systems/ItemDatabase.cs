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
		var items = parsed["Items"].AsArray;

		Items = new List<InventoryItem>();
		foreach(var item in items.Childs)
		{
			InventoryItem newItem = new InventoryItem();

			newItem.Name = item["Name"];
			newItem.Description = item["Description"];
			newItem.Value = item["Value"].AsInt;

			string itemType = item["ItemType"];
			newItem.ItemType = (ItemType) Enum.Parse(typeof(ItemType), itemType);
			newItem.Quantity = 0;

			var equipEffects = item["EquipmentEffects"].AsArray;
			newItem.EquipmentEffects = new List<ItemEffect>();

			foreach(var effect in equipEffects.Childs)
			{
				ItemEffect newEquipEffect = new ItemEffect();

				newEquipEffect.TargetStat = effect["TargetStat"];
				newEquipEffect.FixedEffect = effect["FixedEffect"].AsInt;
				newEquipEffect.ScalingEffect = effect["ScalingEffect"].AsFloat;
				newEquipEffect.EffectDuration = effect["EffectDuration"].AsFloat;

				newItem.EquipmentEffects.Add(newEquipEffect);
			}

			var consumeEffects = item["ConsumeEffects"].AsArray;
			newItem.ConsumeEffects = new List<ItemEffect>();

			foreach(var effect in consumeEffects.Childs)
			{
				ItemEffect newUseEffect = new ItemEffect();
				
				newUseEffect.TargetStat = effect["TargetStat"];
				newUseEffect.FixedEffect = effect["FixedEffect"].AsInt;
				newUseEffect.ScalingEffect = effect["ScalingEffect"].AsFloat;
				newUseEffect.EffectDuration = effect["EffectDuration"].AsFloat;
				
				newItem.ConsumeEffects.Add(newUseEffect);
			}

			Items.Add(newItem);
		}

        IsLoaded = Items.Count > 0;
	}

	#endregion Data Access Methods
}
