using UnityEngine;
using System.Collections;

public class SpellCastingController : ManagerBase<SpellCastingController>
{
	#region Variables / Properties

	private SpellManager _manager;
	private SpellCastingPresenter _presenter;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_manager = SpellManager.Instance;
		_presenter = GetComponentInChildren<SpellCastingPresenter>();

		_presenter.SetVisibility(true);
		UpdatePresenter(_manager.ActiveSpell);
	}

	#endregion Engine Hooks

	#region Methods

	public void UpdatePresenter(Spell spell)
	{
		_presenter.UpdateInterface(spell);
	}

	#endregion Methods
}
