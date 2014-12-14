using System;

[Serializable]
public class HealthSystem
{
	#region Variables / Properties

	public int HP;
	public int MaxHP;
	public int EffectiveMaxHP;

	public bool IsDead
	{
		get { return HP == 0; }
	}

	#endregion Variables / Properties

	#region Methods

	public int GetStatByName(string name)
	{
		switch(name.ToLower())
		{
			case "hp":
				return HP;
				
			case "max hp":
				return MaxHP;
				
			case "effective max hp":
				return EffectiveMaxHP;

			default:
				return -1;
		}
	}

	public void Heal(int amount)
	{
		HP += amount;
		if(HP >= EffectiveMaxHP)
		{
			HP = EffectiveMaxHP;
		}
	}

	public void TakeDamage(int amount)
	{
		HP -= amount;
		if(HP <= 0)
		{
			HP = 0;
		}
	}

	public void RaiseMaxHP(int amount)
	{
		HP += amount;
		MaxHP += amount;
		EffectiveMaxHP += amount;
	}

	public void RaiseEffectiveMaxHP(int amount)
	{
		EffectiveMaxHP += amount;
		if(EffectiveMaxHP >= MaxHP)
		{
			EffectiveMaxHP = MaxHP;
		}
	}

	public void LowerEffectiveMaxHP(int amount)
	{
		EffectiveMaxHP -= amount;
		if(EffectiveMaxHP < 1)
		{
			EffectiveMaxHP = 1;
		}
	}

	public bool ApplyItemEffect(ItemEffect effect)
	{
		int hpChangeIndicator = GetStatByName(effect.TargetStat);
		if(hpChangeIndicator == -1)
			return false;

		int hpChange = (int)(effect.FixedEffect * effect.ScalingEffect);
		string statName = effect.TargetStat.ToLower();
		
		switch(statName)
		{
			case "hp":
				if(hpChange > 0)
					Heal(hpChange);
				else
					TakeDamage(hpChange);
				break;
				
			case "max hp":
				MaxHP += hpChange;
				break;
				
			case "effective max hp":
				if(hpChange > 0)
					RaiseEffectiveMaxHP(hpChange);
				else
					LowerEffectiveMaxHP(hpChange);
				break;
				
			default:
				throw new Exception("Unexpected Health System stat name: " + statName);
		}

		return true;
	}

	public bool RemoveItemEffect(ItemEffect effect)
	{
		int hpChangeIndicator = GetStatByName(effect.TargetStat);
		if(hpChangeIndicator == -1)
			return false;
		
		int hpChange = (int)(effect.FixedEffect * effect.ScalingEffect);
		string statName = effect.TargetStat.ToLower();
		
		switch(statName)
		{
			case "hp":
				HP -= hpChangeIndicator;
				break;
				
			case "max hp":
				MaxHP -= hpChange;
				break;
				
			case "effective max hp":
				EffectiveMaxHP -= hpChange;
				break;
				
			default:
				throw new Exception("Unexpected Health System stat name: " + statName);
		}
		
		return true;
	}

	#endregion Methods
}
