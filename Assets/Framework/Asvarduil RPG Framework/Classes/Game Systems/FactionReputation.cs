using System;
using System.Collections.Generic;

[Serializable]
public class FactionReputation
{
    #region Variables / Properties

    public string Name;
    public string Description;
    public bool IsDiscovered = false;
    public int ReputationPoints = 0;

    #endregion Variables / Properties

    #region Methods

    public void GainReputation(int amount)
    {
        ReputationPoints += amount;
    }

    public void LoseReputation(int amount)
    {
        ReputationPoints -= amount;
    }

    #endregion Methods
}
