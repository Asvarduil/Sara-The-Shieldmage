using UnityEngine;
using System;

/// <summary>
/// Asvarduil Project custom button that shows
/// an image instead of text.
/// </summary>
[Serializable]
public class AsvarduilImageButton : TweenableElement, IClickable
{
	#region Public Fields
	
	/// <summary>
	/// The image shown on the button.
	/// </summary>
	public Texture2D Image;
	
	/// <summary>
	/// The dimensions of the button.
	/// </summary>
	public Vector2 Dimensions;
	
	/// <summary>
	/// The button's tooltip.
	/// </summary>
	public AsvarduilTooltip Tooltip;
	
	#endregion Public Fields
	
	#region Constructor
	
	public AsvarduilImageButton(Vector2 pos,
		                        Vector2 targetPos,
		                        Vector2 dimensions,
		                        Color tint,
		                        Color targetTint,
		                        float tweenRate,
		                        Texture2D image,
	                            AsvarduilTooltip tooltip,
		                        bool isRelative = false) 
		: base(pos, targetPos, tint, targetTint, tweenRate, isRelative)
	{
		Image = image;
		Dimensions = dimensions;
		
		Tooltip = tooltip;
	}
	
	#endregion Constructor
	
	#region Public Methods
	
	/// <summary>
	/// Determines whether the button is clicked.
	/// </summary>
	/// <returns><c>true</c> if this instance is clicked; otherwise, <c>false</c>.</returns>
	public bool IsClicked()
	{	
		if(Image == null)
			return false;
		
		if(! IsInteractable)
			return false;
		
		GUI.depth = Layer;
		GUI.color = Tint;
		Rect buttonRect = GetElementRect(Dimensions);
		
		bool result = GUI.Button (buttonRect, new GUIContent(Image, Tooltip.Description.Text));
		
				// Draw the tooltip.
		if(GUI.tooltip == Tooltip.Description.Text &&
		   ! string.IsNullOrEmpty(GUI.tooltip))
		{
			Tooltip.DrawMe();
		}
		
		return result;
	}
	
	#endregion Public Methods
}
