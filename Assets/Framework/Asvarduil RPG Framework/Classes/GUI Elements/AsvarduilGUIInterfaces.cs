using UnityEngine;
using System;

#region Interfaces

/// <summary>
/// Interface for tweenable objects.
/// </summary>
public interface ITweenable
{
	void Tween();
}

/// <summary>
/// Interface for clickable objects.
/// </summary>
public interface IClickable
{
	bool IsClicked();
}

/// <summary>
/// Interface for things that can be drawn.
/// </summary>
public interface IDrawable
{
	void DrawMe();
}

#endregion Interfaces
