using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class Enemy : CombatEntity
{
	#region Variables / Properties

	public List<ItemDrop> Drops;

	#endregion Variables / Properties

	#region Methods

	public IEnumerable<InventoryItem> RollForLoot()
	{
		List<InventoryItem> loot = new List<InventoryItem>();
		for(int i = 0; i < Drops.Count; i++)
		{
			if(Drops[i].RollForThisDrop())
				loot.Add(Drops[i].Item);
		}

		return loot;
	}

	#endregion Methods
}
