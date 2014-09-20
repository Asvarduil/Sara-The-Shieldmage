using UnityEngine;
using System;

[Serializable]
public class AsvarduilCheckbox : TweenableElement
{
	#region Variables / Properties
	
	public bool Value = false;
	public string Text;
	public Vector2 Dimensions;
	public AsvarduilTooltip Tooltip;
	
	#endregion Variables / Properties
	
	#region Constructor
	
	public AsvarduilCheckbox(Vector2 pos,
		                     Vector2 targetPos,
		                     Color tint,
		                     Color targetTint,
		                     float tweenRate,
		                     string checkboxText,
		                     bool value,
		                     Vector2 dimensions,
		                     AsvarduilTooltip tooltip,
		                     bool isRelative = false) 
		: base(pos, targetPos, tint, targetTint, tweenRate, isRelative)
	{
		Text = checkboxText;
		Value = value;
		Dimensions = dimensions;
		Tooltip = tooltip;
	}
		
	
	#endregion Constructor
	
	#region Methods
	
	public bool IsClicked()
	{
		if(string.IsNullOrEmpty(Text))
		{
			throw new ArgumentException("Checkboxes require text to be shown.");
		}
		
		if(! IsInteractable)
			return false;
		
		GUI.depth = Layer;
		GUI.color = Tint;
		
		Rect checkRect = GetElementRect(Dimensions);
		
		if(Tooltip != null)
		{
			Value = GUI.Toggle(checkRect, Value, new GUIContent(Text, Tooltip.Description.Text));
			
			// Draw the tooltip.
			if(GUI.tooltip == Tooltip.Description.Text
			   && ! string.IsNullOrEmpty(GUI.tooltip))
			{
				Tooltip.DrawMe();
			}
		}
		else
		{
			Value = GUI.Toggle(checkRect, Value, Text);
		}
		
		return true;
	}
	
	#endregion Methods
}
