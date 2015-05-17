using System;

[Serializable]
public class FactionRelationship
{
    #region Variables / Properties

    public string RelationshipDescriptor;
    public bool CanRaise;
    public bool CanLower;
    public int RaiseThreshold;
    public int LowerThreshold;

    public bool FulfillsRelationship(int factionPoints)
    {
        if (!CanLower)
            return factionPoints < RaiseThreshold;

        if (!CanRaise)
            return factionPoints > LowerThreshold;

        return factionPoints > LowerThreshold && factionPoints < RaiseThreshold;
    }

    #endregion Variables / Properties
}
