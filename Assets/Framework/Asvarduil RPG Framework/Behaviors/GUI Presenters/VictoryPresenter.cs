using UnityEngine;
using System.Collections;

public class VictoryPresenter : PresenterBase 
{
	#region Variables / Properties

    public AsvarduilBox Background;
    public AsvarduilLabel Text;
    public AsvarduilButton NextButton;

    private BattleReferee _referee;

	#endregion Variables / Properties
	
	#region Hooks

    public override void Start()
    {
        base.Start();

        _referee = BattleReferee.Instance;
    }
	
	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

        Background.TargetTint.a = opacity;
        Text.TargetTint.a = opacity;
        NextButton.TargetTint.a = opacity;
	}
	
	public override void DrawMe()
	{
        Background.DrawMe();
        Text.DrawMe();

        if(NextButton.IsClicked())
        {
            _maestro.PlayOneShot(ButtonSound);
            _referee.RollForLoot();
        }
	}
	
	public override void Tween()
	{
        Background.Tween();
        Text.Tween();
        NextButton.Tween();
	}
	
	#endregion Hooks
}
