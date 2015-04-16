using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class ConversationPlayerEvents : DebuggableBehavior
{
    #region Variables / Properties

    private PartyManager _party;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _party = PartyManager.Instance;
    }

    public void FullyHealParty(List<string> args)
    {
        List<PlayableCharacter> partyMembers = _party.FindAvailableCharacters().ToList();

        for (int i = 0; i < partyMembers.Count; i++)
        {
            PlayableCharacter current = partyMembers[i];
            current.Health.Heal(current.Health.MaxHP);
        }
    }

    public void ReviveDeadPlayers(List<string> args)
    {
        List<PlayableCharacter> partyMembers = _party.FindAvailableCharacters().ToList();

        for(int i = 0; i < partyMembers.Count; i++)
        {
            PlayableCharacter current = partyMembers[i];
            if (current.Health.IsDead)
                current.Health.Heal(current.Health.MaxHP);
        }

        // TODO: Visual Effect!
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
