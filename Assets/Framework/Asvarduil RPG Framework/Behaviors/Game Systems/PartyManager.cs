using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class PartyManager : ManagerBase<PartyManager>
{
	#region Variables / Properties

	public List<PlayableCharacter> Characters;

	#endregion Variables / Properties

	#region Methods

	public PlayableCharacter FindCharacterInPartyByPosition(int index, int partyId = 0)
	{
		List<PlayableCharacter> partyCharacters = FindCharactersInParty(partyId).ToList();
		return partyCharacters[index];
	}

	public PlayableCharacter FindCharacterByName(string name)
	{
		return Characters.FirstOrDefault(c => c.Name == name);
	}

	public IEnumerable<PlayableCharacter> FindCharactersInParty(int partyId)
	{
		return Characters.Where(c => c.PartyIndex == partyId);
	}

	public IEnumerable<PlayableCharacter> FindAvailableCharacters()
	{
		return Characters.Where(c => c.IsUsable == true);
	}

	#endregion Methods
}
