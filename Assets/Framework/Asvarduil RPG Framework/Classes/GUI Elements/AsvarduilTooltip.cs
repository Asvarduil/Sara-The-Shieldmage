using UnityEngine;
using System;

/// <summary>
/// Asvarduil Project custom tooltip.
/// </summary>
[Serializable]
public class AsvarduilTooltip : IDrawable
{
	#region Public Fields
	
	public AsvarduilStaticImage Background;
	public AsvarduilStaticLabel Description;
	
	#endregion Public Fields
	
	#region Constructor
	
	public AsvarduilTooltip(AsvarduilStaticImage bg, AsvarduilStaticLabel desc)
	{
		Background = bg;
		Description = desc;
	}
	
	#endregion Constructor
	
	#region Public Methods
	
	/// <summary>
	/// Draws the tooltip on the GUI.
	/// </summary>
	public void DrawMe()
	{
		Background.DrawMe();
		Description.DrawMe();
	}
	
	#endregion Public Methods
}
