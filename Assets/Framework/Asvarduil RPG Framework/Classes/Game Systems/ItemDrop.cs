using UnityEngine;
using System;

using Random = UnityEngine.Random;

[Serializable]
public class ItemDrop
{
	public InventoryItem Item;
	public int DropRate;

	public bool RollForThisDrop()
	{
		int roll = Random.Range(0, 100);
		return roll < DropRate;
	}
}
