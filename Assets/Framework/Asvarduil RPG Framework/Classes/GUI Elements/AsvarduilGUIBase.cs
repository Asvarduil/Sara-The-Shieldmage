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
	public bool IsRelative;
	
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
		return IsRelative
			? new Rect(Position.x * Screen.width, Position.y * Screen.height,
			           dims.x * Screen.width,     dims.y * Screen.height)
			: new Rect(Position.x, Position.y, dims.x, dims.y);
	}
	
	#endregion Inherited Methods
	
	#region Abstract Methods
	
	#endregion Abstract Methods
}
