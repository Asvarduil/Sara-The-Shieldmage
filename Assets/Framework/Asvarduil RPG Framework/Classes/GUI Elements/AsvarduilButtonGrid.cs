using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AsvarduilButtonGrid : TweenableElement, IClickable 
{
	#region Variables / Properties

	public int Rows;
	public int Columns;
	public Vector2 Padding;
	public AsvarduilButton PrototypeButton;
	public List<INamed> DataElements = new List<INamed>();

	private List<AsvarduilButton> _gridButtons = new List<AsvarduilButton>();

	public bool HasButtons
	{
		get 
		{ 
			return _gridButtons != null
				   && _gridButtons.Count > 0;
		}
	}

	public int ButtonCount
	{
		get { return _gridButtons.Count; }
	}

	public INamed SelectedObject { get; private set; }

	#endregion Variables / Properties

	#region Constructor

	public AsvarduilButtonGrid(
		Vector2 pos, Vector2 targetPos,
		Color tint, Color targetTint,
		float tweenRate, bool isRelative,
		List<INamed> dataElements
	)
		: base(pos, targetPos, tint, targetTint, tweenRate, isRelative)
	{
		DataElements = dataElements;
		RefreshButtons();
	}

	#endregion Constructor

	#region Implementations of Interfaces

	public bool IsClicked()
	{
		if(! HasButtons)
			return false;

		for(int i = 0; i < _gridButtons.Count; i++)
		{
			AsvarduilButton button = _gridButtons[i];
			if(button.IsClicked())
			{
				SelectedObject = DataElements[i];
				return true;
			}
		}

		return false;
	}
	
	public override void Tween()
	{
		if(! HasButtons)
			return;

		for(int i = 0; i < _gridButtons.Count; i++)
		{
			AsvarduilButton button = _gridButtons[i];
			button.TargetTint = TargetTint;

			button.Tween();
		}
	}

	#endregion Implementations of Interfaces

	#region Methods

	public void Refresh(List<INamed> newData)
	{
		DataElements = new List<INamed>();
		for(int i = 0; i < newData.Count; i++)
		{
			INamed data = newData[i];
			if(! data.IsAvailable)
				continue;

			DataElements.Add(data);
		}

		RefreshButtons();
	}

	public void RefreshButtons()
	{
		// For every data element, clone the template button.
		// take the Presentable Name property, and make it that
		//   button's text.
		_gridButtons = new List<AsvarduilButton>();
		for(int i = 0; i < DataElements.Count; i++)
		{
			INamed element = DataElements[i];

			AsvarduilButton newButton = (AsvarduilButton) PrototypeButton.Clone();
			newButton.ButtonText = element.PresentableName;

			_gridButtons.Add(newButton);
		}

		// Then, go through the list and position each button.
		// Whenever the column count is exceeded, start a new row.
		int row = 0;
		int column = 0;
		for(int i = 0; i < _gridButtons.Count; i++)
		{
			AsvarduilButton button = _gridButtons[i];

			button.IsRelative = IsRelative;

			float xPosition = Position.x + (column * button.Dimensions.x) + Padding.x;
			float yPosition = Position.y + (row * button.Dimensions.y) + Padding.y;

			button.Position.x = xPosition;
			button.TargetPosition.x = xPosition;

			button.Position.y = yPosition;
			button.TargetPosition.y = yPosition;

			column++;
			if(column >= Columns)
			{
				column = 0;
				row++;
			}
		}
	}

	#endregion Methods
}
