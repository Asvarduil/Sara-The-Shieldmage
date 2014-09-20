using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class TreasureManager : ManagerBase<TreasureManager>
{
	#region Variables / Properties

	public List<string> ObtainedTreasures = new List<string>();

	#endregion Variables / Properties

	#region Methods

	public void MarkTreasureAsObtained(string treasureName)
	{
		ObtainedTreasures.Add(treasureName);
	}

	public bool HasObtainedTreasure(string treasureName)
	{
		return ObtainedTreasures.Any(t => t == treasureName);
	}

	#endregion Methods
}
