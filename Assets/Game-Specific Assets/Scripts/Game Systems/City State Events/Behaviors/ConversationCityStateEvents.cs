using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationCityStateEvents : ConversationEventBase
{
    #region Variables / Properties

    public CityStateEventManager _cityStates;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _cityStates = CityStateEventManager.Instance;
        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("AddCityStateEvent", AddCityStateEvent);
        _controller.RegisterEventHook("UpdateCityStateEvent", UpdateCityStateEvent);
        _controller.RegisterEventHook("RemoveCityStateEvent", RemoveCityStateEvent);
    }

    #endregion Hooks

    #region Methods

    public IEnumerator AddCityStateEvent(List<string> args)
    {
        // TODO: Implement
        yield return null;
    }

    public IEnumerator UpdateCityStateEvent(List<string> args)
    {
        // TODO: Implement
        yield return null;
    }

    public IEnumerator RemoveCityStateEvent(List<string> args)
    {
        // TODO: Implement
        yield return null;
    }

    #endregion Methods
}
