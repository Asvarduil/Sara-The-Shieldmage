using UnityEngine;
using System.Collections;

public class BattlePresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilBox EnemyListPane;
	public AsvarduilBox PlayerListPane;

	#endregion Variables / Properties

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		EnemyListPane.TargetTint.a = opacity;
		PlayerListPane.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		GUI.skin = Skin;

		EnemyListPane.DrawMe();
		PlayerListPane.DrawMe();
	}

	public override void Tween()
	{
		EnemyListPane.Tween();
		PlayerListPane.Tween();
	}

	#endregion Hooks

	#region Methods

	#endregion Methods
}
