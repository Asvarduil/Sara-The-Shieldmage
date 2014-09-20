using UnityEngine;
using System;

[Serializable]
public class AsvarduilStaticImage : AsvarduilGUICore, IDrawable
{
	#region Variables
	
	public Vector2 Dimensions;
	public Texture2D Image;
	
	#endregion Variables
	
	#region Constructor
	
	public AsvarduilStaticImage(Vector2 position, Vector2 dimensions, Color tint, Texture2D image, bool isRelative)
		: base(position, tint, isRelative)
	{
		Dimensions = dimensions;
		Image = image;
	}
	
	#endregion Constructor
	
	#region Base Class Overrides
	
	/// <summary>
	/// Draws the image on the GUI.
	/// </summary>
	public void DrawMe()
	{
		if(Image == null)
			return;
		
		GUI.depth = Layer;
		GUI.color = Tint;
		
		Rect imageRect = GetElementRect(Dimensions);
		GUI.DrawTexture(imageRect, Image);
	}
	
	#endregion Base Class Overrides
}
