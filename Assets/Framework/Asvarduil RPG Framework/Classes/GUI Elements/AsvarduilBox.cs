using UnityEngine;
using System;

[Serializable]
public class AsvarduilBox : TweenableElement, IDrawable
{
	#region Variables / Properties

	public Vector2 Dimensions;

	#endregion Variables / Properties

	#region Constructors

	public AsvarduilBox
	(
		Vector2 pos,     Vector2 targetPos,
		Color tint,      Color targetTint,
		float tweenRate, bool isRelative
	) 
	: base
	(
		pos,       targetPos, 
	    tint,      targetTint, 
	    tweenRate, isRelative
	)
	{
		// Constructor body
	}

	#endregion Constructors

	#region Methods

	public void DrawMe()
	{
		Rect rect = GetElementRect(Dimensions);
		GUI.color = Tint;

		GUI.Box(rect, "");
	}

	#endregion Methods
}
