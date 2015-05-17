using System;
using System.Collections.Generic;

using UnityEngine;

public class FactionManager : ManagerBase<FactionManager>
{
    #region Variables / Properties

    public List<FactionReputation> FactionRelationships;
    public List<FactionRelationship> RelationshipStates;

    #endregion Variables / Properties

    #region Hooks

    public FactionRelationship GetFactionRelationshipByFactionName(string factionName)
    {
        FactionReputation faction = GetFactionByName(factionName);

        FactionRelationship relationship = null;
        for(int i = 0; i < RelationshipStates.Count; i++)
        {
            FactionRelationship current = RelationshipStates[i];
            if (current.FulfillsRelationship(faction.ReputationPoints))
            {
                relationship = current;
                break;
            }
        }
            
        return relationship;
    }

    public void DiscoverFaction(string factionName)
    {
        FactionReputation faction = GetFactionByName(factionName);
        faction.IsDiscovered = true;
    }

    public void RaiseFactionReputation(string factionName, int amount)
    {
        FactionReputation faction = GetFactionByName(factionName);
        faction.GainReputation(amount);
    }

    public void LowerFactionReputation(string factionName, int amount)
    {
        FactionReputation faction = GetFactionByName(factionName);
        faction.LoseReputation(amount);
    }

    #endregion Hooks

    #region Methods

    private FactionReputation GetFactionByName(string factionName)
    {
        FactionReputation result = null;
        for (int i = 0; i < FactionRelationships.Count; i++)
        {
            FactionReputation current = FactionRelationships[i];
            if (current.Name == factionName)
            {
                result = current;
                break;
            }
        }

        return result;
    }

    #endregion Methods
}
