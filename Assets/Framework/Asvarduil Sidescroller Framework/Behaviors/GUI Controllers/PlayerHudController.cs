using UnityEngine;
using System.Collections;

public class PlayerHudController : ManagerBase<PlayerHudController>
{
	#region Variables / Properties
	
	private HealthHudPresenter _healthPresenter;
	private ManaHudPresenter _manaPresenter;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_healthPresenter = GetComponentInChildren<HealthHudPresenter>();
		_manaPresenter = GetComponentInChildren<ManaHudPresenter>();

		GameObject _playerCharacter = FindObjectOfType<SidescrollingPlayerControl>().gameObject;
		_healthPresenter.Initialize(_playerCharacter);
		_manaPresenter.Initialize(_playerCharacter);
	}

	#endregion Engine Hooks

	#region Methods

	public void Show()
	{
		_healthPresenter.SetVisibility(true);
		_manaPresenter.SetVisibility(true);
	}

	public void Hide()
	{
		_healthPresenter.SetVisibility(false);
		_manaPresenter.SetVisibility(false);
	}

	public void UpdateHealthWidget(int hp, int maxHP)
	{
		_healthPresenter.UpdateImage(hp, maxHP);
	}

	public void UpdateManaWidget(int mp, int maxMP)
	{
		_manaPresenter.UpdateImage(mp, maxMP);
	}

	#endregion Methods
}
