using UnityEngine;
using System;

public class SpellTargetingPresenter : PresenterBase, IDisposable
{
	#region Variables / Properties

	public bool isPlacingSpell = false;
	public float effectDepth = 6.5f;
	public GameObject effectGhost;
	
	private GameObject _ghostInstance;

	public Vector3 SpellPosition
	{
		get 
		{
			if(_ghostInstance == null)
				return Vector3.zero;

			return _ghostInstance.transform.position;
		}
	}

	#endregion Variables / Properties

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		isPlacingSpell = isVisible;

		if(isPlacingSpell)
		{
			DebugMessage("A spell is being placed...");
			_ghostInstance = (GameObject) GameObject.Instantiate(effectGhost, transform.position, transform.rotation);
		}
		else
		{
			DebugMessage("The caster is no longer positioning a spell.");
			GameObject.Destroy(_ghostInstance);
			_ghostInstance = null;
		}
	}

	public override void Tween()
	{
	}

	public override void DrawMe()
	{
	}

	public void Dispose()
	{
		_ghostInstance = null;
	}

	#endregion Hooks

	#region Methods

	public void SetEffectGhost(GameObject ghostObject)
	{
		effectGhost = ghostObject;
	}

	#endregion Methods
}
