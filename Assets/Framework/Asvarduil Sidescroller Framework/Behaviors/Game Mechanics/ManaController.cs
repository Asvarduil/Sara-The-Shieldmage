using UnityEngine;
using System.Collections;

public class ManaController : MonoBehaviour 
{
	#region Variables / Properties
	
	public ManaSystem Mana;
	public GameObject OutOfManaEffect;
	
	private PlayerHudController _playerHud;
	
	#endregion Variables / Properties
	
	#region Engine Hooks
	
	public void Start()
	{
		_playerHud = PlayerHudController.Instance;
	}
	
	#endregion Engine Hooks
	
	#region Methods
	
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
		Mana.Gain(amount);
		_playerHud.UpdateManaWidget(Mana.MP, Mana.MaxMP);
	}
	
	#endregion Methods
}
