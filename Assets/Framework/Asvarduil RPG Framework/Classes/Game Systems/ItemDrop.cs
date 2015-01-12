using UnityEngine;
using System;

using Random = UnityEngine.Random;

[Serializable]
public class ItemDrop : ICloneable
{
	public InventoryItem Item;
	public int DropRate;

    public int BaseAmount;
    public int MinimumBonus;
    public int MaximumBonus;

    public object Clone()
    {
        var clone = new ItemDrop
        {
            Item = this.Item,
            DropRate = this.DropRate,
            BaseAmount = this.BaseAmount,
            MinimumBonus = this.MinimumBonus,
            MaximumBonus = this.MaximumBonus
        };

        return clone;
    }

	public bool RollForThisDrop()
	{
		int roll = Random.Range(0, 100);
		return roll < DropRate;
	}

    public int RollForQuantity()
    {
        int quantity = BaseAmount + Random.Range(MinimumBonus, MaximumBonus);
        return quantity;
    }
}
