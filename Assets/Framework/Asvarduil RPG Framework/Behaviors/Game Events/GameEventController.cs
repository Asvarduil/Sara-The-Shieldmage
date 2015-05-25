using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventController : ManagerBase<GameEventController>
{
    #region Variables / Properties

    private List<GameEventHook> _eventFunctions;

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _eventFunctions = new List<GameEventHook>();
    }

    #endregion Hooks

    #region Methods

    public void RegisterEventHook(string eventName, Func<List<string>, IEnumerator> eventFunction)
    {
        GameEventHook newHook = new GameEventHook
        {
            Name = eventName,
            Function = eventFunction
        };

        _eventFunctions.Add(newHook);
        DebugMessage("Registered event '" + eventName + "'.");
    }

    // Immediately runs all game events simultaneously; the first and second events run at the same time.
    public void RunGameEventGroup(List<GameEvent> gameEvents)
    {
        DebugMessage("Controller is executing a game event group with " + gameEvents.Count + " members...");

        for (int i = 0; i < gameEvents.Count; i++)
        {
            GameEvent gameEvent = gameEvents[i];
            StartCoroutine(ExecuteGameEvent(gameEvent));
        }
    }

    // Runs each game event sequentially; the second event doesn't run until the first is done.
    public IEnumerator ExecuteGameEventGroup(List<GameEvent> gameEvents)
    {
        DebugMessage("Controller is executing a game event group with " + gameEvents.Count + " members...");

        for (int i = 0; i < gameEvents.Count; i++)
        {
            GameEvent gameEvent = gameEvents[i];
            yield return StartCoroutine(ExecuteGameEvent(gameEvent));
        }
    }

    public IEnumerator ExecuteGameEvent(GameEvent gameEvent)
    {
        DebugMessage("Controller is executing game event: " + gameEvent.Event + "...");

        string eventName = gameEvent.Event;
        List<string> eventArgs = gameEvent.EventArgs;

        // Find the first coroutine on the child behaviors with a name that matches the event name.
        GameEventHook coroutine = _eventFunctions.FirstOrDefault(f => f.Name == eventName);
        if (coroutine == default(GameEventHook))
        {
            Debug.LogError("Could not find an event named " + eventName + " in the registered event list.");
            yield break;
        }

        DebugMessage(eventName + " is registered!  Doing it.");
        yield return StartCoroutine(coroutine.Function(eventArgs));
    }

    #endregion Methods
}
