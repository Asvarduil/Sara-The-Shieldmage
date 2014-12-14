using UnityEngine;
using System;

[Serializable]
public abstract class FloatingGUIBase
{
	#region Variables
	
	public Vector2 offset;
	public int layer = 0;
	
	protected Vector2 position;
	
	#endregion Variables
	
	#region Methods
	
	public void CalculatePosition(Vector3 worldCoords)
	{
		position = Camera.main.WorldToScreenPoint( worldCoords );
		
		position.x += offset.x;
		position.y = ( Screen.height - position.y ) + offset.y;
	}
	
	#endregion Methods
}
