using UnityEngine;
using System;
using System.Collections.Generic;

public class HealthHudPresenter : ResourceBitPresenter
{
	#region Methods

	public override void Initialize(GameObject playerCharacter)
	{
		HealthController _player = playerCharacter.GetComponent<HealthController>();
		UpdateImage(_player.Health.HP, _player.Health.MaxHP);
	}

	#endregion Methods
}
