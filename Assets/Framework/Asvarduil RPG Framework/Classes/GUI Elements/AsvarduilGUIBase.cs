using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all Asvarduil Project GUI elements.
/// </summary>
public abstract class AsvarduilGUICore
{
	#region Public Fields
	
	/// <summary>
	/// Relative GUI layer that the element will be drawn on (bigger is farther 'back').
	/// </summary>
	public int Layer = 0;
	
	/// <summary>
	/// Is the GUI element's position/dimensions relative to the screen's dimensions?
	/// (e.g. Position = (0.5,0.5) relative causes the element's top left corner
	///                  to be the middle of the screen.)
	/// </summary>
	public bool IsRelative = true;

	/// <summary>
	/// How is the GUI element placed, relative to the left/right of the screen?
	/// </summary>
	public AsvarduilHorizontalElementPositioning HorizontalPositioning;

	/// <summary>
	/// How is the GUI element placed, relative to the top/bottom of the screen?
	/// </summary>
	public AsvarduilVerticalElementPositioning VerticalPositioning;
	
	/// <summary>
	/// The GUI element's current position.
	/// </summary>
	public Vector2 Position;
	
	/// <summary>
	/// The GUI element's current color.
	/// </summary>
	public Color Tint;
	
	#endregion Public Fields
	
	#region Constructor
	
	public AsvarduilGUICore(Vector2 pos, Color tint, bool isRelative = false)
	{
		Position = pos;
		Tint = tint;
		IsRelative = isRelative;
	}
	
	#endregion Constructor
	
	#region Inherited Methods
	
	/// <summary>
	/// Gets the element's rectangled based on if this element is relative
	/// or fixed.
	/// </summary>
	/// <returns>The element's rectangle.</returns>
	/// <param name='dims'>2-dimensional vector of the element's dimensions</param>
	public virtual Rect GetElementRect(Vector2 dims)
	{
		Vector2 offset = new Vector2(JustifyHorizontal(dims), JustifyVertical(dims));
		Vector2 scale = new Vector2(ScaleHorizontal(dims), ScaleVertical(dims));
		return new Rect(offset.x, offset.y, scale.x, scale.y);
	}

	/// <summary>
	/// Based on the Horizontal Positioning setting, return where the
	/// GUI element should be placed, in whole pixels.
	/// </summary>
	/// <returns>The X pixel.</returns>
	protected virtual float JustifyHorizontal(Vector2 dims)
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
					float proportionalWidth = dims.x * Screen.width;
					return Screen.width - proportionalWidth - proportionalOffset;
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

	/// <summary>
	/// Based on the Vertical Positioning setting, return where the
	/// GUI element should be placed, in whole pixels.
	/// </summary>
	/// <returns>The Y pixel.</returns>
	protected virtual float JustifyVertical(Vector2 dims)
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
					float proportionalHeight = dims.y * Screen.height;
					return Screen.height - proportionalHeight - proportionalOffset;
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

	protected virtual float ScaleHorizontal(Vector2 dims)
	{
		return IsRelative
			? dims.x * Screen.width
			: dims.x;
	}

	protected virtual float ScaleVertical(Vector2 dims)
	{
		return IsRelative
			? dims.y * Screen.height
			: dims.y;
	}
	
	#endregion Inherited Methods
	
	#region Abstract Methods
	
	#endregion Abstract Methods	
}
