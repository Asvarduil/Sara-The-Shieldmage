using UnityEngine;
using System;

/// <summary>
/// Asvarduil Project custom message box.
/// </summary>
[Serializable]
public class AsvarduilMessageBox : IClickable, ITweenable
{
	#region Public Fields

	public AsvarduilImage background;
	public AsvarduilLabel message;
	public AsvarduilButton okButton;
	
	#endregion Public Fields
	
	#region Public Methods
	
	/// <summary>
	/// Determines whether the message box is clicked.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is clicked; otherwise, <c>false</c>.
	/// </returns>
	public bool IsClicked()
	{
		background.DrawMe();
		message.DrawMe();
		
		return okButton.IsClicked();
	}
	
	/// <summary>
	/// Tweens the message box.
	/// </summary>
	public void Tween()
	{
		background.Tween();
		message.Tween();
		okButton.Tween();
	}
	
	#endregion Public Methods
}
