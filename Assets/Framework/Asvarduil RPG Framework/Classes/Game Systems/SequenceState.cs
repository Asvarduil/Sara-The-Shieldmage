using System;

[Serializable]
public class SequenceState
{
	#region Variables / Properties

	public string Name;
	public int Counter;

	#endregion Variables / Properties

	#region Methods

	public override string ToString()
	{
		return string.Format("[{0} {1}]", Name, Counter);
	}

	#endregion Methods
}
