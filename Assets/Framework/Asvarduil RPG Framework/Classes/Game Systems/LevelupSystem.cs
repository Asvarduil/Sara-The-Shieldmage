using System;
using System.Collections.Generic;

[Serializable]
public class LevelupSystem 
{
	#region Variables / Properties

	public string Name;
	public int Level;
	public int XP;
	public List<int> LevelUpAmountTable;

	public int XpDifferenceToNextLevel
	{
		get{ return RawXpToNextLevel - XP; }
	}

	public int RawXpToNextLevel
	{
		get{ return LevelUpAmountTable[Level - 1]; }
	}

	#endregion Variables / Properties

	#region Methods

	public bool GainXP(int amount)
	{
		XP += amount;
		if(XP > RawXpToNextLevel)
		{
			Level++;
			return true;
		}

		return false;
	}

	#endregion Methods
}
