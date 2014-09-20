using UnityEngine;
using System.Collections;

public class ManaHudPresenter : ResourceBitPresenter
{
	#region Methods

	public override void Initialize(GameObject playerCharacter)
	{
		ManaController _player = playerCharacter.GetComponent<ManaController>();
		UpdateImage(_player.Mana.MP, _player.Mana.MaxMP);
	}

	#endregion Methods
}
