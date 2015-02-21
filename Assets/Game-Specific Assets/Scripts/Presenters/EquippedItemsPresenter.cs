using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquippedItemsPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Button ChangeWeapon;
    public Button ChangeArmor;
    public Button ChangeAccessory;

    public Text WeaponLabel;
    public Text ArmorLabel;
    public Text AccessoryLabel;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void SetEquipmentButtonVisibilities(bool weaponsVisible, bool armorVisible, bool accessoryVisible)
    {
        ActivateButton(ChangeWeapon, weaponsVisible);
        ActivateButton(ChangeArmor, armorVisible);
        ActivateButton(ChangeAccessory, accessoryVisible);
    }

    public void ReloadCharacterEquipment(PlayableCharacter character)
    {
        ReloadWeaponStats(character.Weapon);
        ReloadArmorStats(character.Armor);
        ReloadAccessoryStats(character.Accessory);
    }

    public void ReloadWeaponStats(InventoryItem weapon)
    {
        WeaponLabel.text = weapon.EquipmentBonusText;
    }

    public void ReloadArmorStats(InventoryItem armor)
    {
        ArmorLabel.text = armor.EquipmentBonusText;
    }

    public void ReloadAccessoryStats(InventoryItem accessory)
    {
        AccessoryLabel.text = accessory.EquipmentBonusText;
    }

    #endregion Methods
}
