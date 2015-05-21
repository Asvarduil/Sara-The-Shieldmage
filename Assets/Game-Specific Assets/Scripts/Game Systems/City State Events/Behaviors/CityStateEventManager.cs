using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CityStateEventManager : ManagerBase<CityStateEventManager>
{
    #region Variables / Properties

    public List<CityStateEvent> CityStateEvents;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public List<CityStateEvent> GetEventsForCityStateWithName(string cityStateName, string eventName)
    {
        // TODO: Ditto.
        List<CityStateEvent> events = CityStateEvents.Where(e => e.CityStateName == cityStateName && e.EventKey == eventName).ToList();
        return events;
    }

    #endregion Methods
}
