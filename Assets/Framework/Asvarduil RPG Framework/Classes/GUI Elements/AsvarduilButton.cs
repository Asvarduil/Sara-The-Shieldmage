using UnityEngine;
using System;

/// <summary>
/// Asvarduil Project custom button, with text, tweening,
/// and built-in tooltips.
/// </summary>
[Serializable]
public class AsvarduilButton : TweenableElement, IClickable, ICloneable
{
	#region Public Fields
	
	/// <summary>
	/// The dimensions of the button.
	/// </summary>
	public Vector2 Dimensions;
	
	/// <summary>
	/// The button's text.
	/// </summary>
	public string ButtonText;
	
	/// <summary>
	/// The button's tooltip.
	/// </summary>
	public AsvarduilTooltip Tooltip;
	
	#endregion Public Fields
	
	#region Constructor
	
	public AsvarduilButton(Vector2 pos,
		                   Vector2 targetPos,
		                   Vector2 dimensions,
		                   Color tint,
		                   Color targetTint,
		                   float tweenRate,
	                       string buttonText,
	                       AsvarduilTooltip tooltip) 
		: base(pos, targetPos, tint, targetTint, tweenRate)
	{
		Dimensions = dimensions;
		ButtonText = buttonText;
		
		Tooltip = tooltip;
	}
	
	#endregion Constructor
	
	#region Public Methods
	
	/// <summary>
	/// Determines whether the button has been clicked.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is clicked; otherwise, <c>false</c>.
	/// </returns>
	public bool IsClicked()
	{	
		if(ButtonText == null)
		{
			throw new ArgumentException("Buttons require text to be shown.");
		}
		
		if(! IsInteractable)
			return false;
		
		GUI.depth = Layer;
		GUI.color = Tint;
		Rect buttonRect = GetElementRect(Dimensions);
		
		bool result;
		if(Tooltip != null)
		{
			result = GUI.Button(buttonRect, new GUIContent(ButtonText, Tooltip.Description.Text));
			
			// Draw the tooltip.
			if(GUI.tooltip == Tooltip.Description.Text
			   && ! string.IsNullOrEmpty(GUI.tooltip))
			{
				Tooltip.DrawMe();
			}
		}
		else
		{
			result = GUI.Button(buttonRect, ButtonText);
		}
		
		return result;
	}

	public object Clone()
	{
		return new AsvarduilButton
		(
			Position, TargetPosition, Dimensions, 
			Tint, TargetTint, TweenRate, 
			ButtonText, Tooltip
		);
	}
	
	#endregion Public Methods
}

