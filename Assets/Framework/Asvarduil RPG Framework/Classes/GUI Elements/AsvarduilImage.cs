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
		Vector2 offset = new Vector2(JustifyHorizontal(dims), JustifyVertical(dims));
		return new Rect(offset.x, offset.y, dims.x, dims.y);
	}
	
	protected override float JustifyHorizontal(Vector2 dims)
	{
		float result = 0.0f;
		
		if(IsRelative)
		{
			switch(HorizontalPositioning)
			{
				case AsvarduilHorizontalElementPositioning.Left:
					return Position.x * Screen.width;
					
				case AsvarduilHorizontalElementPositioning.Right:
					float proportionalOffset = Position.x * Screen.width;
					// NOTE: In Images, we never give 'relative' dimensions.
					return Screen.width - dims.x - proportionalOffset;
			}
		}
		else
		{
			switch(HorizontalPositioning)
			{
				case AsvarduilHorizontalElementPositioning.Left:
					return Position.x;
					
				case AsvarduilHorizontalElementPositioning.Right:
					return Screen.width - dims.x - Position.x;
			}
		}
		
		return result;
	}

	protected override float JustifyVertical(Vector2 dims)
	{
		float result = 0.0f;
		
		if(IsRelative)
		{
			switch(VerticalPositioning)
			{
				case AsvarduilVerticalElementPositioning.Top:
					return Position.y * Screen.height;
					
				case AsvarduilVerticalElementPositioning.Bottom:
					float proportionalOffset = Position.y * Screen.height;
					// NOTE: In images, we never give 'relative' dimensions.
					return Screen.height - dims.y - proportionalOffset;
			}
		}
		else
		{
			switch(VerticalPositioning)
			{
				case AsvarduilVerticalElementPositioning.Top:
					return Position.y;
					
				case AsvarduilVerticalElementPositioning.Bottom:
					return Screen.height - dims.y - Position.y;
			}
		}
		
		return result;
	}

	#endregion Public Methods
}
