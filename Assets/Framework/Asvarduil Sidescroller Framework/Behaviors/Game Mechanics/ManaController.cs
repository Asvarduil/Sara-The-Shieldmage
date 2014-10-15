using UnityEngine;
using System.Collections;

public class ManaController : MonoBehaviour 
{
	#region Variables / Properties

	public bool RegenerateMana = true;

	public ManaSystem Mana;
	public float RegenerationInterval = 5;
	public float RegenerationAmount = 1;

	private float _lastRegeneration;
	private Maestro _maestro;
	private PlayerHudController _playerHud;
	private ParticleSystem _manaGainEffect;

	public bool IsFull
	{
		get { return Mana.MP >= Mana.MaxMP; }
	}
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	public void Start()
	{
		_playerHud = PlayerHudController.Instance;

		_maestro = Maestro.Instance;
		// Requires a Legacy Particle Emitter, because Shuriken can't do a 'black hole' effect.
		_manaGainEffect = transform.FindChild("Mana Gain Effect").GetComponent<ParticleSystem>();
	}

	public void Update()
	{
		CheckRegeneration();
	}
	
	#endregion Engine Hooks
	
	#region Methods

	private void CheckRegeneration()
	{
		if(! RegenerateMana)
			return;

		if(Time.time < _lastRegeneration + RegenerationInterval)
			return;

		if(Mana.MP >= Mana.MaxMP)
			return;
		
		Gain(1);
		_lastRegeneration = Time.time;
	}
	
	public void Lose(int amount)
	{
		// If Mana is full, start the regeneration timer immediately,
		// to prevent the Mana from gaining a charge immediately after
		// a cast or loss.
		if(Mana.MP == Mana.MaxMP)
		{
			_lastRegeneration = Time.time;
		}

		Mana.Lose(amount);
		_playerHud.UpdateManaWidget(Mana.MP, Mana.MaxMP);
	}
	
	public void Gain(int amount)
	{
		if(_manaGainEffect != null)
			_manaGainEffect.Emit(25);

		Mana.Gain(amount);
		_playerHud.UpdateManaWidget(Mana.MP, Mana.MaxMP);
	}

	public void IncrementMax()
	{
		Mana.RaiseMaxMP(1);
		_playerHud.UpdateManaWidget(Mana.MP, Mana.MaxMP);
	}
	
	#endregion Methods
}
