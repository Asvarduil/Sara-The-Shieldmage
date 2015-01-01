using System;
using UnityEngine;

[Serializable]
public class HealthSystem
{
	#region Variables / Properties

	public int HP;
	public int MaxHP;
    public int RegenAmount;
    public float RegenRate;

    public Action OnDamageTaken;

	public bool IsDead
	{
		get { return HP == 0; }
	}

    public float NextRegenTick
    {
        get { return _lastRegenTick + RegenRate; }
    }

    private float _lastRegenTick;

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

            case "regen amount":
                return RegenAmount;

			default:
				return -1;
		}
	}

	public void Heal(int amount)
	{
		HP += amount;
		if(HP >= MaxHP)
		{
			HP = MaxHP;
		}
	}

    public void Regenerate()
    {
        if (RegenAmount == 0)
            return;

        if (Time.time <= NextRegenTick)
            return;

        _lastRegenTick = Time.time;
        Heal(RegenAmount);
    }

	public void TakeDamage(int amount)
	{
		HP -= amount;
		if(HP <= 0)
		{
			HP = 0;
		}

        // Counter-effects!
        if (HP > 0)
            OnDamageTaken();
	}

	public void RaiseMaxHP(int amount)
	{
		HP += amount;
		MaxHP += amount;
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
                RaiseMaxHP(effect);
                break;

            case "regen amount":
                RegenAmount += effect;
                break;

            default:
                throw new Exception("Unexpected Health System stat name: " + statName);
        }
    }

	#endregion Methods
}
