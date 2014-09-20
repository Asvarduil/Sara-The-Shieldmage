using UnityEngine;

public class Fader : MonoBehaviour 
{
	#region Constants
	
	private const float _OpaqueEnough = 0.99f;
	
	#endregion Constants
	
	#region Variables / Properties
	
	public Texture2D FadeImage;
	public float FadeRate = 0.1f;
	public Color Tint = new Color(0,0,0, 1);
	public Color TargetTint = new Color(0,0,0, 0);
	
	public bool ScreenHidden
	{
		get { return Tint.a >= _OpaqueEnough; }
	}
	
	public bool ScreenShown
	{
		get { return Tint.a < _OpaqueEnough; }
	}
	
	#endregion Variables / Properties
	
	#region Engine Hooks

    void OnGUI()
	{
		GUI.color = Tint;
		GUI.depth = -1;
		GUI.DrawTexture(new Rect(0,0, Screen.width,Screen.height), FadeImage);
	}
	
	void FixedUpdate() 
	{
		Tint = Color.Lerp(Tint, TargetTint, FadeRate * Time.deltaTime);
	}
	
	#endregion Engine Hooks
	
	#region Methods
	
	public void FadeOut()
	{
		Tint = new Color(0,0,0, 0);
		TargetTint = new Color(0,0,0, 1);
	}
	
	public void FadeIn()
	{
		Tint = new Color(0,0,0, 1);
		TargetTint = new Color(0,0,0, 0);
	}
	
	#endregion Methods
}
