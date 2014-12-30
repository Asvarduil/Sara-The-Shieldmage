using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class Enemy : CombatEntity
{
	#region Variables / Properties

    public Func<List<Ability>, Ability> DetermineAction
    {
        get 
        {
            if (BattlePiece == null)
                throw new InvalidOperationException("There is no BattleEntity script on the Enemy's prefab.");

            if (BattlePiece.AI == null)
                throw new InvalidOperationException("The AI object did not get initialized on the Battle Piece.");

            return (abilities) => BattlePiece.AI.DetermineAction(abilities); 
        }
    }

    public Func<List<CombatEntity>, CombatEntity> DetermineTarget
    {
        get 
        {
            if (BattlePiece == null)
                throw new InvalidOperationException("There is no BattleEntity script on the Enemy's prefab.");

            if (BattlePiece.AI == null)
                throw new InvalidOperationException("The AI object did not get initialized on the Battle Piece.");

            return (target) => BattlePiece.AI.DetermineTarget(target); 
        }
    }

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
