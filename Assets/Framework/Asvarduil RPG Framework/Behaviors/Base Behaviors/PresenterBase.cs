using UnityEngine;
using System.Collections;

public abstract class PresenterBase : DebuggableBehavior, ITweenable, IDrawable
{
	#region Variables / Properties

	public GUISkin Skin;
	public AudioClip ButtonSound;
	public int DefaultResolutionHeight;
	public int LabelFontSize;
	public int ButtonFontSize;

	protected Maestro _maestro;

	#endregion Variables / Properties

	#region Engine Hooks

	public virtual void Start()
	{
		_maestro = Maestro.Instance;
	}

	public virtual void OnGUI()
	{
		AdaptGuiSkinToDisplay();
		DrawMe();
	}
	
	public virtual void Update()
	{
		Tween();
	}

	#endregion Engine Hooks

	#region Methods

	public abstract void SetVisibility(bool isVisible);

	public abstract void DrawMe();

	public abstract void Tween();

	protected float DetermineOpacity(bool isVisible)
	{
		return isVisible ? 1.0f : 0.0f;
	}

	protected void AdaptGuiSkinToDisplay()
	{
		GUI.skin = Skin;

		GUI.skin.label.fontSize = Screen.height / DefaultResolutionHeight  * LabelFontSize;
		GUI.skin.button.fontSize = Screen.height / DefaultResolutionHeight  * ButtonFontSize;
	}

	#endregion Methods
}
