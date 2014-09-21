using UnityEngine;
using System.Collections;

public class HealthController : DebuggableBehavior
{
	#region Variables / Properties

	public bool ReloadSceneOnDeath = false;
	public HealthSystem Health;

	public GameObject DeathEffect;

	private PlayerHudController _playerHud;
	private ParticleSystem _healEffect;
	private ParticleSystem _damageEffect;

	public bool IsFull
	{
		get { return Health.HP >= Health.MaxHP; }
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_playerHud = PlayerHudController.Instance;
		_healEffect = transform.FindChild("Heal Effect").GetComponent<ParticleSystem>();
		_damageEffect = transform.FindChild("Damage Effect").GetComponent<ParticleSystem>();
	}

	#endregion Engine Hooks

	#region Methods

	public void TakeDamage(int amount)
	{
		if(_damageEffect != null)
			_damageEffect.Emit(25);

		Health.TakeDamage(amount);
		_playerHud.UpdateHealthWidget(Health.HP, Health.MaxHP);

		if(Health.IsDead)
		{
			if(DeathEffect != null)
				GameObject.Instantiate(DeathEffect, transform.position, transform.rotation);

			gameObject.SetActive(false);

			if(ReloadSceneOnDeath)
				DeathReloadManager.Instance.ReloadLevel();
		}
	}

	public void Heal(int amount)
	{
		if(_healEffect != null)
			_healEffect.Emit(25);

		Health.Heal(amount);
		_playerHud.UpdateHealthWidget(Health.HP, Health.MaxHP);
	}

	#endregion Methods
}
