using UnityEngine;
using System;

using Random = UnityEngine.Random;

[Serializable]
public class ItemDrop : ICloneable
{
	public InventoryItem Item;
	public int DropRate;

    public object Clone()
    {
        var clone = new ItemDrop
        {
            Item = this.Item,
            DropRate = this.DropRate
        };

        return clone;
    }

	public bool RollForThisDrop()
	{
		int roll = Random.Range(0, 100);
		return roll < DropRate;
	}
}
