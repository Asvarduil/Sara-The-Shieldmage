using UnityEngine;
using System.Collections;

public class HealthController : DebuggableBehavior
{
	#region Variables / Properties

	public bool ReloadSceneOnDeath = false;
	public HealthSystem Health;
	public int GainParticleCount = 5;
	public int DamageParticleCount = 25;
	public AudioClip HealSoundEffect;
	public AudioClip DamageSoundEffect;

	public GameObject DeathEffect;

	private Maestro _maestro;
	private ParticleSystem _healEffect;
	private ParticleSystem _damageEffect;
	private PlayerHudController _playerHud;

	public bool IsFull
	{
		get { return Health.HP >= Health.MaxHP; }
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_maestro = Maestro.Instance;
		_playerHud = PlayerHudController.Instance;

		Transform healEffectChild = transform.FindChild("Heal Effect");
		if(healEffectChild != null)
			_healEffect = healEffectChild.GetComponent<ParticleSystem>();

		Transform damageEffectChild = transform.FindChild("Damage Effect");
		if(damageEffectChild != null)
			_damageEffect = damageEffectChild.GetComponent<ParticleSystem>();
	}

	#endregion Engine Hooks

	#region Methods

	public void TakeDamage(int amount)
	{
		if(_damageEffect != null)
			_damageEffect.Emit(DamageParticleCount);

		if(DamageSoundEffect != null)
			_maestro.PlayOneShot(DamageSoundEffect);

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
			_healEffect.Emit(GainParticleCount);

		if(HealSoundEffect != null)
			_maestro.PlayOneShot(HealSoundEffect);

		Health.Heal(amount);
		_playerHud.UpdateHealthWidget(Health.HP, Health.MaxHP);
	}

	public void IncrementMax()
	{
		Health.RaiseMaxHP(1);
		_playerHud.UpdateHealthWidget(Health.HP, Health.MaxHP);
	}

	#endregion Methods
}
