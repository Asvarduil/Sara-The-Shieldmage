using System;

[Serializable]
public class ManaSystem
{
	#region Variables / Properties
	
	public int MP;
	public int MaxMP;
	public int EffectiveMaxMP;
	
	public bool IsOutOfMana
	{
		get { return MP == 0; }
	}
	
	#endregion Variables / Properties
	
	#region Methods
	
	public int GetStatByName(string name)
	{
		switch(name.ToLower())
		{
			case "mp":
				return MP;
				
			case "max mp":
				return MaxMP;
				
			case "effective max mp":
				return EffectiveMaxMP;
				
			default:
				return -1;
		}
	}
	
	public void Gain(int amount)
	{
		MP += amount;
		if(MP >= EffectiveMaxMP)
		{
			MP = EffectiveMaxMP;
		}
	}
	
	public void Lose(int amount)
	{
		MP -= amount;
		if(MP <= 0)
		{
			MP = 0;
		}
	}

	public void RaiseMaxMP(int amount)
	{
		MP += amount;
		MaxMP += amount;
		EffectiveMaxMP += amount;
	}
	
	public void RaiseEffectiveMaxMP(int amount)
	{
		EffectiveMaxMP += amount;
		if(EffectiveMaxMP >= MaxMP)
		{
			EffectiveMaxMP = MaxMP;
		}
	}
	
	public void LowerEffectiveMaxMP(int amount)
	{
		EffectiveMaxMP -= amount;
		if(EffectiveMaxMP < 1)
		{
			EffectiveMaxMP = 1;
		}
	}
	
	public bool ApplyItemEffect(ItemEffect effect)
	{
		int mpChangeIndicator = GetStatByName(effect.TargetStat);
		if(mpChangeIndicator == -1)
			return false;
		
		int mpChange = (int)(effect.FixedEffect * effect.ScalingEffect);
		string statName = effect.TargetStat.ToLower();
		
		switch(statName)
		{
			case "mp":
				if(mpChange > 0)
					Gain(mpChange);
				else
					Lose(mpChange);
				break;
				
			case "max mp":
				MaxMP += mpChange;
				break;
				
			case "effective max mp":
				if(mpChange > 0)
					RaiseEffectiveMaxMP(mpChange);
				else
					LowerEffectiveMaxMP(mpChange);
				break;
				
			default:
				throw new Exception("Unexpected Health System stat name: " + statName);
		}
		
		return true;
	}
	
	public bool RemoveItemEffect(ItemEffect effect)
	{
		int mpChangeIndicator = GetStatByName(effect.TargetStat);
		if(mpChangeIndicator == -1)
			return false;
		
		int mpChange = (int)(effect.FixedEffect * effect.ScalingEffect);
		string statName = effect.TargetStat.ToLower();
		
		switch(statName)
		{
			case "mp":
				MP -= mpChangeIndicator;
				break;
				
			case "max mp":
				MaxMP -= mpChange;
				break;
				
			case "effective max mp":
				EffectiveMaxMP -= mpChange;
				break;
				
			default:
				throw new Exception("Unexpected Health System stat name: " + statName);
		}
		
		return true;
	}
	
	#endregion Methods
}
