using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class InventoryPresenter : PresenterBase 
{
	#region Variables / Properties

	public AsvarduilBox InventoryBox;
	public AsvarduilButtonGrid ItemGrid;

	public AsvarduilBox CommandBar;
	public AsvarduilButton UseItemButton;
	public AsvarduilButton EquipItemButton;
	public AsvarduilLabel ItemInfoLabel;

	private PauseController _controller;
	private InventoryItem _currentItem;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();
		_controller = PauseController.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);
		InventoryBox.TargetTint.a = opacity;
		ItemGrid.TargetTint.a = opacity;

		HideCommandBar();
	}

	public override void DrawMe()
	{
		InventoryBox.DrawMe();

		if(ItemGrid.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_currentItem = (InventoryItem) ItemGrid.SelectedObject;
			LoadItemInCommandBar(_currentItem);
		}

		CommandBar.DrawMe();
		ItemInfoLabel.DrawMe();

		if(EquipItemButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.EquipItem(_currentItem);
		}

		if(UseItemButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.UseItem(_currentItem);
		}
	}

	public override void Tween()
	{
		InventoryBox.Tween();
		ItemGrid.Tween();

		CommandBar.Tween();
		ItemInfoLabel.Tween();
		UseItemButton.Tween();
		EquipItemButton.Tween();
	}

	public void HideCommandBar()
	{
		CommandBar.TargetTint.a = 0.0f;
		ItemInfoLabel.TargetTint.a = 0.0f;
		UseItemButton.TargetTint.a = 0.0f;
		EquipItemButton.TargetTint.a = 0.0f;
	}

	public void LoadItemInCommandBar(InventoryItem item)
	{
		ItemInfoLabel.Text = item.InfoText;

		CommandBar.TargetTint.a = 1.0f;
		ItemInfoLabel.TargetTint.a = 1.0f;
		UseItemButton.TargetTint.a = 0.0f;
		EquipItemButton.TargetTint.a = 0.0f;

		switch(item.ItemType)
		{
			case ItemType.Consumable:
				UseItemButton.TargetTint.a = 1.0f;
				break;

			case ItemType.Weapon:
			case ItemType.Armor:
			case ItemType.Accessory:
				EquipItemButton.TargetTint.a = 1.0f;
				break;
		}
	}

	public void LoadItems(List<INamed> items)
	{
		ItemGrid.Refresh(items);
	}

	#endregion Methods
}
