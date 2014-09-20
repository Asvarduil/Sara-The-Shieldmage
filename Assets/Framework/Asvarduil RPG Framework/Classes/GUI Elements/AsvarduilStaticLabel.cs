using UnityEngine;
using System;

[Serializable]
public class AsvarduilStaticLabel : AsvarduilGUICore, IDrawable
{
	#region Variables / Properties
	
	public Vector2 Dimensions;
	public string Text;
	
	#endregion Variables / Properties
	
	#region Constructor
	
	public AsvarduilStaticLabel(Vector2 pos, Vector2 dims, Color tint, string text)
		: base(pos, tint)
	{
		Dimensions = dims;
		Text = text;
	}
	
	#endregion Constructor
	
	#region Methods
	
	/// <summary>
	/// Draws the static Asvarduil label.
	/// </summary>
	public void DrawMe()
	{
		if(string.IsNullOrEmpty(Text))
			return;
		
		GUI.depth = Layer;
		GUI.color = Tint;
		
		Rect labelRect = new Rect(Position.x, Position.y, Dimensions.x, Dimensions.y);
		GUI.Label(labelRect, Text);
	}
	
	#endregion Methods
}
