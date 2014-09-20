using UnityEngine;
using System;

[Serializable]
public class FloatingTexture : FloatingGUIBase, IDrawable
{
	#region Variables
	
	public Texture2D image;
	
	#endregion Variables
	
	#region Methods
	
	public void DrawMe()
	{
		if(! image) { return; }
		
		GUI.depth = layer;
		Vector2 dimensions = new Vector2(image.width, image.height);
		
		GUI.DrawTexture( new Rect(position.x, position.y, dimensions.x, dimensions.y), image);
	}
	
	#endregion Methods
}
