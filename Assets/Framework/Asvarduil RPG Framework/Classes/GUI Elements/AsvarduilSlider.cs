using UnityEngine;
using System;

[Serializable]
public class AsvarduilSlider : TweenableElement
{
	#region Variables / Properties
	
	public bool IsHorizontal = true;
	public float Value;
	public float MinValue;
	public float MaxValue;
	public float Step;
	public Vector2 Dimensions;
	public AsvarduilLabel Label;
	
	#endregion Variables / Properties
	
	#region Constructor
	
	public AsvarduilSlider(Vector2 pos,
	                       Vector2 targetPos,
	                       Color tint,
	                       Color targetTint,
	                       float tweenRate,
	                       Vector2 dimensions,
	                       float value,
	                       float step,
		                   bool isRelative = false) 
		: base(pos, targetPos, tint, targetTint, tweenRate, isRelative)
	{
		Value = value;
		Step = step;
		Dimensions = dimensions;
	}
	
	#endregion Constructor
	
	#region Methods
	
	public override void Tween()
	{
		Label.Tween();
		base.Tween();
	}
	
	public float IsMoved()
	{
		if(!IsInteractable)
			return Value;
		
		Label.DrawMe();
		
		GUI.depth = Layer;
		GUI.color = Tint;
		Rect sliderRect = GetElementRect(Dimensions);
		
		if(IsHorizontal)
		{
			Value = GUI.HorizontalSlider(sliderRect, Value, MinValue, MaxValue);
			return Value;
		}

		Value = GUI.VerticalSlider(sliderRect, Value, MaxValue, MinValue);
		return Value;
	}
	
	#endregion Methods
}
