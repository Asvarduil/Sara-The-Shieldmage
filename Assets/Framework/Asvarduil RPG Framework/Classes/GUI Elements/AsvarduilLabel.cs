using UnityEngine;
using System;

/// <summary>
/// Asvarduil Project custom label.
/// </summary>
[Serializable]
public class AsvarduilLabel : TweenableElement, IDrawable
{
	#region Public Fields
	
	/// <summary>
	/// The dimensions of the label.
	/// </summary>
	public Vector2 Dimensions;
	
	/// <summary>
	/// The text shown by the label.
	/// </summary>
	public string Text;
	
	#endregion Public Fields
	
	#region Constructor
	
	public AsvarduilLabel(Vector2 pos,
	                      Vector2 targetPos,
	                      Vector2 dimensions,
		                  string text,
	                      Color tint,
	                      Color targetTint,
	                      float tweenRate,
		                  bool isRelative = false) 
		: base(pos, targetPos, tint, targetTint, tweenRate, isRelative)
	{
		Dimensions = dimensions;
		Text = text;
	}
	
	#endregion Constructor
	
	#region Public Methods
	
	/// <summary>
	/// Draws the label on the GUI.
	/// </summary>
	public void DrawMe()
	{
		if(string.IsNullOrEmpty(Text))
		{
			return;
		}
		
		GUI.depth = Layer;
		GUI.color = Tint;
		Rect imageRect = GetElementRect(Dimensions);
		
		GUI.Label(imageRect, Text);
	}
	
	#endregion Public Methods
}
