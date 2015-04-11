using System;
using System.Linq;
using UnityEngine;

public class PriestNPC : EntityTextInterface
{
    #region Variables / Properties

    public string deadPartyMembersThread = "Revive Party";
    public string genericChatterThread = "Chatter";

    private PartyManager _party;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        base.Start();
        _party = PartyManager.Instance;
    }

    public override void OnInteraction()
    {
        string threadName = _party.FindAvailableCharacters().Any(p => p.Health.IsDead)
            ? deadPartyMembersThread
            : genericChatterThread;

        TextThread thread = _entityText.GetThreadByName(threadName);
        if (thread == default(TextThread))
            throw new Exception("Could not find a thread on NPC " + gameObject.name + " named " + threadName);

        _controller.PresentTextThread(thread);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
