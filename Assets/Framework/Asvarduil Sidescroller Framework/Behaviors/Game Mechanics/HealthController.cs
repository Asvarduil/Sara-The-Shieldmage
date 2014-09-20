using UnityEngine;
using System.Collections;

public class HealthController : DebuggableBehavior
{
	#region Variables / Properties

	public HealthSystem Health;
	public GameObject DamageEffect;
	public GameObject DeathEffect;

	private PlayerHudController _playerHud;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_playerHud = PlayerHudController.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public void TakeDamage(int amount)
	{
		GameObject.Instantiate(DamageEffect, transform.position, transform.rotation);
		Health.TakeDamage(amount);
		_playerHud.UpdateHealthWidget(Health.HP, Health.MaxHP);

		if(Health.IsDead)
		{
			GameObject.Instantiate(DeathEffect, transform.position, transform.rotation);
			TransitionManager.Instance.ChangeScenes();
			gameObject.SetActive(false);
		}
	}

	public void Heal(int amount)
	{
		Health.Heal(amount);
		_playerHud.UpdateHealthWidget(Health.HP, Health.MaxHP);
	}

	#endregion Methods
}
