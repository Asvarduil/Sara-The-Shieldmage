using System;
using UnityEngine;

[Serializable]
public class Spell
{
	#region Variables / Properties

	public string Name;
	public Texture2D Thumbnail;
	public PlayerCharacter Character;

	public bool IsTargeted = false;
	public bool CanCast = false;
	public int ManaCost = 1;

	public GameObject Effect;

	#endregion Variables / Properties
}
