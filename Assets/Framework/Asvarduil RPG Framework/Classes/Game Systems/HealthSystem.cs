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

    public bool ApplyAbilityEffect(AbilityEffect effect, int amount)
    {
        int hpChangeIndicator = GetStatByName(effect.TargetStat);
        if (hpChangeIndicator == -1)
            return false;

        string statName = effect.TargetStat.ToLower();
        ChangeHealthSystemStat(statName, amount);

        return true;
    }

	public bool ApplyItemEffect(ItemEffect effect)
	{
		int hpChangeIndicator = GetStatByName(effect.TargetStat);
		if(hpChangeIndicator == -1)
			return false;

        // TODO: This is wrong.  Fix it.
		int amount = (int)(effect.FixedEffect * effect.ScalingEffect);
		string statName = effect.TargetStat.ToLower();
        ChangeHealthSystemStat(statName, amount);

		return true;
	}

	public bool RemoveItemEffect(ItemEffect effect)
	{
		int hpChangeIndicator = GetStatByName(effect.TargetStat);
		if(hpChangeIndicator == -1)
			return false;
		
        // TODO: This is wrong.  Fix it.
		int amount = (int)(effect.FixedEffect * effect.ScalingEffect) * -1;
		string statName = effect.TargetStat.ToLower();
        ChangeHealthSystemStat(statName, amount);
		
		return true;
	}

    private void ChangeHealthSystemStat(string statName, int effect)
    {
        switch (statName)
        {
            case "hp":
                if (effect > 0)
                    Heal(effect);
                else
                    TakeDamage(Math.Abs(effect));
                break;

            case "max hp":
                MaxHP += effect;
                break;

            case "effective max hp":
                if (effect > 0)
                    RaiseEffectiveMaxHP(effect);
                else
                    LowerEffectiveMaxHP(Math.Abs(effect));
                break;

            default:
                throw new Exception("Unexpected Health System stat name: " + statName);
        }
    }

	#endregion Methods
}
