using UnityEngine;
using System.Collections;

public class ManaController : MonoBehaviour 
{
	#region Variables / Properties
	
	public ManaSystem Mana;
	public float RegenerationInterval = 5;
	public float RegenerationAmount = 1;
	public GameObject OutOfManaEffect;

	private float _lastRegeneration;
	private PlayerHudController _playerHud;
	private ParticleEmitter _manaGainEffect;

	public bool IsFull
	{
		get { return Mana.MP >= Mana.MaxMP; }
	}
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	public void Start()
	{
		_playerHud = PlayerHudController.Instance;

		// Requires a Legacy Particle Emitter, because Shuriken can't do a 'black hole' effect.
		_manaGainEffect = transform.FindChild("Mana Gain Effect").GetComponent<ParticleEmitter>();
	}

	public void Update()
	{
		CheckRegeneration();
	}
	
	#endregion Engine Hooks
	
	#region Methods

	private void CheckRegeneration()
	{
		if(Time.time < _lastRegeneration + RegenerationInterval)
			return;

		if(Mana.MP >= Mana.MaxMP)
			return;
		
		Gain(1);
		_lastRegeneration = Time.time;
	}
	
	public void Lose(int amount)
	{
		if(amount > Mana.MP)
		{
			GameObject.Instantiate(OutOfManaEffect, transform.position, transform.rotation);
			return;
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
	
	#endregion Methods
}
