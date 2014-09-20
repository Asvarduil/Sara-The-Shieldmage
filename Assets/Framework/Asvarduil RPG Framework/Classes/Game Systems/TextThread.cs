using System;
using System.Collections.Generic;

[Serializable]
public class TextThread
{
	#region Variables / Properties

	public string Name;
	public bool IsDefaultThread = false;
	public SequenceRange SequenceRange;
	public List<TextContent> TextContent;

	#endregion Variables / Properties

	#region Methods

	#endregion Methods
}
