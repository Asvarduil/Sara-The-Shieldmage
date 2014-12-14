using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class EnemyDatabase : ManagerBase<EnemyDatabase>
{
	#region Variables / Properties

	public List<Enemy> Enemies;

	#endregion Variables / Properties

	#region Hooks

	#endregion Hooks

	#region Methods

	public Enemy FindEnemyByName(string enemyName)
	{
		Enemy result = null;
		for (int i = 0; i < Enemies.Count; i++) 
		{
			Enemy current = Enemies[i];
			if(current.Name != enemyName)
				continue;

			result = current;
			break;
		}

		return result;
	}

	#endregion Methods
}
