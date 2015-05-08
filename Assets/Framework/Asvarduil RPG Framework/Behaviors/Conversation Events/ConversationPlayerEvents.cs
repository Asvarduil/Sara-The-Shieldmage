using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ConversationPlayerEvents : ConversationEventBase
{
    #region Variables / Properties

    private PartyManager _party;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _party = PartyManager.Instance;
        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("FullyHealParty", FullyHealParty);
        _controller.RegisterEventHook("ReviveDeadPlayers", ReviveDeadPlayers);
    }

    public IEnumerator FullyHealParty(List<string> args)
    {
        List<PlayableCharacter> partyMembers = _party.FindAvailableCharacters().ToList();

        for (int i = 0; i < partyMembers.Count; i++)
        {
            PlayableCharacter current = partyMembers[i];
            current.Health.Heal(current.Health.MaxHP);
        }

        yield return null;
    }

    public IEnumerator ReviveDeadPlayers(List<string> args)
    {
        List<PlayableCharacter> partyMembers = _party.FindAvailableCharacters().ToList();

        for(int i = 0; i < partyMembers.Count; i++)
        {
            PlayableCharacter current = partyMembers[i];
            if (current.Health.IsDead)
                current.Health.Heal(current.Health.MaxHP);
        }

        // TODO: Visual Effect!

        yield return null;
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
