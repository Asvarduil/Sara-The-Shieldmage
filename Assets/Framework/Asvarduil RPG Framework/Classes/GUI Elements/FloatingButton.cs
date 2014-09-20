using UnityEngine;
using System;

[Serializable]
public class FloatingButton : FloatingGUIBase, IClickable
{
	#region Variables
	
	public Texture2D image;
	public string text;
	public Vector2 dimensions;
	
	public AsvarduilTooltip tooltip;
	
	#endregion Variables
	
	#region Methods
	
	public bool IsClicked()
	{	
		GUI.depth = layer;
		
		GUIContent content;
		if( ! image )
		{
			content = new GUIContent(text, tooltip.Description.Text);
		}
	    else if( string.IsNullOrEmpty(text))
		{
			content = new GUIContent(image, tooltip.Description.Text);
	    }
		else
		{
			content = new GUIContent(image + text, tooltip.Description.Text);
		}
		
		Rect rectangle = new Rect(position.x,position.y, dimensions.x,dimensions.y);
		bool value = GUI.Button( rectangle, content );
		
		// Draw the tooltip at the specified position.
		if( GUI.tooltip != "" 
			&& GUI.tooltip == tooltip.Description.Text )
		{
			tooltip.DrawMe();
		}
		
		return value;
	}
	
	#endregion Methods
}
