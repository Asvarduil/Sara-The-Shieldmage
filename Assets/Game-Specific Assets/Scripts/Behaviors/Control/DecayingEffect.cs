using UnityEngine;
using System.Collections;

public class DecayingEffect : MonoBehaviour 
{
	#region Variables / Properties

	public float EffectDuration = 5.0f;
	public string ShaderTintFieldName = "_TintColor";
	public float MaterialFadeRate = 0.05f;
	public float MinimumFadeAmount = 0.1f;
	public GameObject ExpirationEffect;

	private float _spawnTime;
	private float _expireTime;
	private Material _objectMaterial;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_spawnTime = Time.time;
		_expireTime = _spawnTime + EffectDuration;

		_objectMaterial = renderer.materials[0];
	}

	public void Update()
	{
		FadeObjectAlpha();
		CheckExpiration();
	}

	#endregion Engine Hooks

	#region Methods

	public void FadeObjectAlpha()
	{
		Color target = renderer.material.GetColor(ShaderTintFieldName);
		target.a = (target.a - ((MaterialFadeRate * target.a) / EffectDuration));

		renderer.material.SetColor(ShaderTintFieldName, target);
	}

	public void CheckExpiration()
	{
		if(Time.time <= _expireTime)
			return;

		if(ExpirationEffect != null)
			GameObject.Instantiate(ExpirationEffect, transform.position, Quaternion.identity);
		
		Destroy(gameObject);
	}

	#endregion Methods
}
