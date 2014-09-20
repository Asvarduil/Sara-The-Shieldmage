using UnityEngine;
using System;

/// <summary>
/// Asvarduil Project custom image methods
/// </summary>
[Serializable]
public class AsvarduilImage : TweenableElement, IDrawable
{
	#region Public Fields
	
	/// <summary>
	/// The image to be shown.
	/// </summary>
	public Texture2D Image;
	
	#endregion Public Fields
	
	#region Constructor
	
	public AsvarduilImage(Vector2 pos,
	                      Vector2 targetPos,
	                      Color tint,
	                      Color targetTint,
	                      Texture2D image,
	                      float tweenRate,
		                  bool isRelative = false) 
		: base(pos, targetPos, tint, targetTint, tweenRate, isRelative)
	{
		Image = image;
	}
	
	#endregion Constructor
	
	#region Public Methods
	
	/// <summary>
	/// Draws the given image on the GUI.
	/// </summary>
	public void DrawMe()
	{
		if(Image == null)
		{
			return;
		}
		
		GUI.depth = Layer;
		GUI.color = Tint;
		Rect imageRect = GetElementRect(new Vector2(Image.width, Image.height));
		
		GUI.DrawTexture(imageRect, Image);
	}
	
	public override Rect GetElementRect(Vector2 dims)
	{
		return IsRelative
			? new Rect(Position.x * Screen.width, Position.y * Screen.height, dims.x, dims.y)
			: new Rect(Position.x, Position.y, dims.x, dims.y);
	}
	
	#endregion Public Methods
}
