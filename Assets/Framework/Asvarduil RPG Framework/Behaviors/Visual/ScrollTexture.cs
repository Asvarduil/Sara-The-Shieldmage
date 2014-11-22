using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour 
{
	#region Variables / Properties

	public Vector2 ScrollDirection = new Vector2(0.1f, 0.1f);

	private Material _workingMaterial;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_workingMaterial = renderer.material;
	}

	public void Update()
	{
		_workingMaterial.mainTextureOffset += (ScrollDirection * Time.deltaTime);
		renderer.material = _workingMaterial;
	}

	public void OnDestroy()
	{
		_workingMaterial = null;
	}

	public void OnLevelLoaded()
	{
		_workingMaterial = null;
	}

	#endregion Engine Hooks
}
