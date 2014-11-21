using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class SpellManager : ManagerBase<SpellManager>, ISuspendable
{
	#region Variables / Properties

	public bool isTargetingEnabled = true;
	public bool isPlacingSpell = false;
	public AudioClip OutOfManaEffect;
	
	public List<Spell> Spells;

	public string ChangeSpellAxis;

	private int _currentSpell = 0;
	private Maestro _maestro;
	private ManaController _mana;
	private GameObject _effectGhost;

	private float _lastSpellToggle;
	private float _spellToggleLockout = 0.25f;

	private ControlManager _controls;
	private CharacterManager _characters;
	private SpellCastingPresenter _spellInterface;
	private SpellTargetingPresenter _targetPresenter;

	public List<Spell> CharacterSpells
	{
		get
		{
			return Spells.Where(s => s.Character == _characters.ActiveCharacter).ToList();
		}
	}

	public Spell ActiveSpell
	{
		get 
		{ 
			return CharacterSpells[_currentSpell]; 
		}
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Awake()
	{
		_maestro = Maestro.Instance;
		_controls = ControlManager.Instance;
		_characters = CharacterManager.Instance;

		// Acquire mana system, so that we can tell spells to consume mana charges.
		GameObject playerCharacter = GameObject.FindGameObjectWithTag("Player");
		_mana = playerCharacter.GetComponent<ManaController>();

		_spellInterface = GetComponentInChildren<SpellCastingPresenter>();
		_targetPresenter = GetComponentInChildren<SpellTargetingPresenter>();
	}

	public void Start()
	{
		_spellInterface.UpdateInterface(ActiveSpell);
		_spellInterface.SetVisibility(true);
	}

	public void Update()
	{
		DetectChangedSpells();
		TargetSpell();
	}

	#endregion Engine Hooks

	#region Methods

	public void DetectChangedSpells()
	{
		float spellChange = _controls.GetAxis(ChangeSpellAxis);
		if (spellChange == 0)
			return;

		if (Time.time < _lastSpellToggle + _spellToggleLockout)
			return;

		_lastSpellToggle = Time.time;

		// Supports a positive or negative axis!
		if (spellChange > 0) 
		{
			_currentSpell++;
			if(_currentSpell == CharacterSpells.Count)
				_currentSpell = 0;
		} 
		else 
		{
			_currentSpell--;
			if(_currentSpell == -1)
				_currentSpell = CharacterSpells.Count - 1;
		}

		DebugMessage("Swapped to spell: " + ActiveSpell.Name);
		_spellInterface.UpdateInterface(ActiveSpell);
	}

	public void TargetSpell()
	{
		if(! ActiveSpell.IsTargeted)
			return;
		
		if(! isTargetingEnabled)
			return;
		
		bool canCast = _mana.Mana.MP >= ActiveSpell.ManaCost;
		_targetPresenter.UpdateTargetingCursor(canCast);
	}

	public void Suspend()
	{
		isTargetingEnabled = false;
		_spellInterface.SetVisibility(false);
		CancelSpell();
	}

	public void Resume()
	{
		isTargetingEnabled = true;
		_spellInterface.SetVisibility(true);
	}

	public void PrepareSpell()
	{
		if(! isTargetingEnabled)
			return;

		if(! ActiveSpell.IsTargeted)
		{
			DebugMessage("The current spell is not targeted.  Doing a relative placement instead.");
			return;
		}

		isPlacingSpell = true;
		_targetPresenter.SetVisibility(true);
	}

	public void CancelSpell()
	{
		isPlacingSpell = false;
		_targetPresenter.SetVisibility(false);
	}

	public void CastSpell()
	{
		if(! isTargetingEnabled)
			return;

		Vector3 position = _targetPresenter.SpellPosition;
		_targetPresenter.SetVisibility(false);

		if(_mana.Mana.MP < ActiveSpell.ManaCost)
		{
			if(OutOfManaEffect != null)
				_maestro.PlayOneShot(OutOfManaEffect);

			return;
		}

		_mana.Lose(ActiveSpell.ManaCost);
		GameObject.Instantiate(ActiveSpell.Effect, position, Quaternion.identity);
	}

	#endregion Methods
}
