using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class EquipmentPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilBox EquipmentBox;

	public AsvarduilButton ChangeWeapon;
	public AsvarduilImage WeaponIcon;
	public AsvarduilLabel WeaponLabel;

	public AsvarduilButton ChangeArmor;
	public AsvarduilImage ArmorIcon;
	public AsvarduilLabel ArmorLabel;

	public AsvarduilButton ChangeAccessory;
	public AsvarduilImage AccessoryIcon;
	public AsvarduilLabel AccessoryLabel;

	private PauseController _controller;
	private PartyManager _partyManager;

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

		EquipmentBox.TargetTint.a = opacity;
		
		ArmorIcon.TargetTint.a = opacity;
		WeaponIcon.TargetTint.a = opacity;
		AccessoryIcon.TargetTint.a = opacity;
		ArmorLabel.TargetTint.a = opacity;
		WeaponLabel.TargetTint.a = opacity;
		AccessoryLabel.TargetTint.a = opacity;
		ChangeWeapon.TargetTint.a = opacity;
		ChangeArmor.TargetTint.a = opacity;
		ChangeAccessory.TargetTint.a = opacity;
	}
	
	public override void DrawMe()
	{
		EquipmentBox.DrawMe();
		
		ArmorIcon.DrawMe();
		WeaponIcon.DrawMe();
		AccessoryIcon.DrawMe();
		ArmorLabel.DrawMe();
		WeaponLabel.DrawMe();
		AccessoryLabel.DrawMe();

		if(ChangeWeapon.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenFilteredItems(ItemType.Weapon);
		}

		if(ChangeArmor.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenFilteredItems(ItemType.Armor);
		}

		if(ChangeAccessory.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.OpenFilteredItems(ItemType.Accessory);
		}
	}
	
	public override void Tween()
	{
		EquipmentBox.Tween();

		ArmorIcon.Tween();
		WeaponIcon.Tween();
		AccessoryIcon.Tween();
		ArmorLabel.Tween();
		WeaponLabel.Tween();
		AccessoryLabel.Tween();

		ChangeWeapon.Tween();
		ChangeArmor.Tween();
		ChangeAccessory.Tween();
	}

	public void SetEquipmentButtonVisibilities(bool weaponsVisible, bool armorVisible, bool accessoryVisible)
	{
		ChangeWeapon.TargetTint.a = DetermineOpacity(weaponsVisible);
		ChangeArmor.TargetTint.a = DetermineOpacity(armorVisible);
		ChangeAccessory.TargetTint.a = DetermineOpacity(accessoryVisible);
	}

	public void ReloadCharacterEquipment(PlayableCharacter character)
	{
		ReloadWeaponStats(character.Weapon);
		ReloadArmorStats(character.Armor);
		ReloadAccessoryStats(character.Accessory);
	}

	public void ReloadWeaponStats(InventoryItem weapon)
	{
		WeaponLabel.Text = weapon.EquipmentBonusText;
	}

	public void ReloadArmorStats(InventoryItem armor)
	{
		ArmorLabel.Text = armor.EquipmentBonusText;
	}

	public void ReloadAccessoryStats(InventoryItem accessory)
	{
		AccessoryLabel.Text = accessory.EquipmentBonusText;
	}

	#endregion Methods
}
