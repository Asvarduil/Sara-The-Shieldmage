using UnityEngine;
using System.Collections;

public class LootPresenter : PresenterBase
{
	#region Variables / Properties
	
	#endregion Variables / Properties
	
	#region Methods
	
	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);
	}
	
	public override void DrawMe()
	{
	}
	
	public override void Tween()
	{
	}
	
	#endregion Methods
}
