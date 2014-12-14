using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour 
{
	#region Variables / Properties

	public string textureProperty = "_MainTex";
	public Vector2 ScrollDirection = new Vector2(0.1f, 0.1f);

	private Vector2 _textureOffset;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Update()
	{
		_textureOffset += ScrollDirection * Time.deltaTime;
		renderer.material.SetTextureOffset(textureProperty, _textureOffset);
	}

	#endregion Engine Hooks
}
