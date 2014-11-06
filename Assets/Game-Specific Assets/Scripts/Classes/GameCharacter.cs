using System;

[Serializable]
public class GameCharacter
{
	#region Variables / Properties

	public string Name;
	public PlayerCharacter CharacterKey;
	
	public bool IsUsable;
	public int PartyIndex = 0;
	
	public HealthSystem Health;
	public ManaSystem Mana;

	#endregion Variables / Properties
}
