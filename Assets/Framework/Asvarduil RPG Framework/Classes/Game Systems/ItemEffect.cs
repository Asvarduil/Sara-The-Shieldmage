using System;
using SimpleJSON;

[Serializable]
public class ItemEffect : IJsonSavable
{
	#region Variables / Properties

	public string TargetStat;
	public int FixedEffect;
	public float ScalingEffect;
	public float EffectDuration;

	#endregion Variables / Properties

    #region Methods

    public void ImportState(JSONClass state)
    {
        TargetStat = state["TargetStat"];
        FixedEffect = state["FixedEffect"].AsInt;
        ScalingEffect = state["ScalingEffect"].AsFloat;
        EffectDuration = state["EffectDuration"].AsFloat;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["TargetStat"] = new JSONData(TargetStat);
        state["FixedEffect"] = new JSONData(FixedEffect);
        state["ScalingEffect"] = new JSONData(ScalingEffect);
        state["EffectDuration"] = new JSONData(EffectDuration);

        return state;
    }

    #endregion Methods
}
