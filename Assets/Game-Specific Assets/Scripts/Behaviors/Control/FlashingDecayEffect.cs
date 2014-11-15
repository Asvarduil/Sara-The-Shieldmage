using UnityEngine;
using System.Collections;

public class FlashingDecayEffect : DebuggableBehavior 
{
	#region Variables / Properties

	public string ShaderTintFieldName = "_TintColor";
	public int NumberOfFlashes = 3;
	public float FlashInterval = 1.0f;
	public float InitialAlpha = 1.0f;
	public float FadeRate = 0.25f;
	public AudioClip FlashSoundEffect;
	public GameObject ExpirationEffect;

	private int _remainingFlashes;
	private float _nextFlash;
	private float _currentAlphaStrength;
	private float _spawnTime;
	private float _expireTime;

	private Maestro _maestro;

	private float EffectDuration
	{
		get { return FlashInterval * NumberOfFlashes; }
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_maestro = Maestro.Instance;

		_spawnTime = Time.time;
		_expireTime = _spawnTime + EffectDuration;
		_remainingFlashes = NumberOfFlashes;
		_nextFlash = _spawnTime + FlashInterval;
	}

	public void Update()
	{
		FlashFade();
		CheckExpiration();
	}

	#endregion Engine Hooks

	#region Methods

	private void FlashFade()
	{
		Color target = renderer.material.GetColor(ShaderTintFieldName);

		if(Time.time >= _nextFlash)
		{
			if(FlashSoundEffect != null)
				_maestro.PlayOneShot(FlashSoundEffect);

			_nextFlash = Time.time + FlashInterval;
			_remainingFlashes--;
			target.a = InitialAlpha;
		}
		else
		{
			target.a = (target.a - ((FadeRate * target.a) / EffectDuration));
		}

		renderer.material.SetColor(ShaderTintFieldName, target);
	}

	private void CheckExpiration()
	{
		if(Time.time <= _expireTime)
			return;
		
		if(ExpirationEffect != null)
			GameObject.Instantiate(ExpirationEffect, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
	}

	#endregion Methods
}
