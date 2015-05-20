using System;
using System.Collections.Generic;

[Serializable]
public class NemesisObjective
{
    #region Variables / Properties

    public int ObjectiveId;
    public float DurationToCompletion;
    public string Description;
    public List<string> MisinformationDescriptions;
    public List<NemesisContingency> Outcomes;

    #endregion Variables / Properties

    #region Methods

    #endregion Methods
}
