using UnityEngine;
using System.Collections.Generic;

public class SpellManager : ManagerBase<SpellManager>
{
	#region Variables / Properties

	public bool isPlacingSpell = false;
	public AudioClip OutOfManaEffect;
	
	public List<Spell> Spells;

	private int _currentSpell = 0;
	private Maestro _maestro;
	private ManaController _mana;
	private GameObject _effectGhost;

	// TODO: Implement spell selection presenter...
	//private SpellSelectionPresenter _selectPresenter;
	private SpellTargetingPresenter _targetPresenter;

	public Spell ActiveSpell
	{
		get { return Spells[_currentSpell]; }
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_maestro = Maestro.Instance;

		// Acquire mana system, so that we can tell spells to consume mana charges.
		GameObject playerCharacter = FindObjectOfType<SidescrollingPlayerControl>().gameObject;
		_mana = playerCharacter.GetComponent<ManaController>();

		//_selectPresenter = GetComponentInChildren<SpellSelectionPresenter>();
		_targetPresenter = GetComponentInChildren<SpellTargetingPresenter>();
	}

	#endregion Engine Hooks

	#region Methods

	public void PrepareSpell()
	{
		isPlacingSpell = true;
		// TODO: Ref ghost effect of current spell.
		_targetPresenter.SetEffectGhost(ActiveSpell.GhostEffect);
		_targetPresenter.SetVisibility(true);
	}

	public void CancelSpell()
	{
		isPlacingSpell = false;
		_targetPresenter.SetVisibility(false);
	}

	public void CastSpell()
	{
		Vector3 position = _targetPresenter.SpellPosition;
		_targetPresenter.SetVisibility(false);

		if(_mana.Mana.MP < ActiveSpell.ManaCost)
		{
			if(OutOfManaEffect != null)
				_maestro.PlayOneShot(OutOfManaEffect);

			return;
		}

		_mana.Lose(ActiveSpell.ManaCost);
		// TODO: Instantiate the current spell at the cursor location.
		GameObject.Instantiate(ActiveSpell.Effect, position, Quaternion.identity);
	}

	#endregion Methods
}
