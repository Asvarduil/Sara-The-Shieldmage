using UnityEngine;
using System.Collections;

public class ExpiringObject : DebuggableBehavior
{
	#region Variables / Properties

	public float LifeSpan;
	public GameObject ExpireEffect;

	private float _expireTime;

	#endregion Variables / Properties

	#region Hooks

	public void Start()
	{
		_expireTime = Time.time + LifeSpan;
	}

	public void Update()
	{
		if (Time.time < _expireTime)
			return;

		if (ExpireEffect != null)
			GameObject.Instantiate(ExpireEffect, transform.position, transform.rotation);

		Destroy(gameObject);
	}

	#endregion Hooks

	#region Methods

	#endregion Methods
}
