using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class LootPresenter : PresenterBase
{
	#region Variables / Properties

    public AsvarduilBox Background;
    public AsvarduilLabel Text;
    public AsvarduilButton NextButton;

    private BattleReferee _referee;
	
	#endregion Variables / Properties
	
	#region Hooks

    public void ShowLoot(List<InventoryItem> items)
    {
        StringBuilder builder = new StringBuilder("Got ");
        for (int i = 0; i < items.Count; i++)
        {
            if (i > 0 && i == items.Count - 1)
                builder.Append("and ");

            InventoryItem item = items[i];
            builder.Append(item.Quantity);
            builder.Append(" ");
            builder.Append(item.Name);

            if (i < items.Count - 1)
                builder.Append(", ");
        }

        Text.Text = builder.ToString();

        SetVisibility(true);
    }

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

        if (NextButton.IsClicked())
        {
            _maestro.PlayOneShot(ButtonSound);
            _referee.ReturnToOriginalScene();
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
