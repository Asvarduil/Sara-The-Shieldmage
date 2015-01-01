using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class PauseController : ManagerBase<PauseController>
{
	#region Variables / Properties

	public int CurrentPartyId = 0;
	public int CurrentPartyMemberIndex = 0;
	public int CurrentInventoryPosition = 0;
	public int CurrentMagicPosition = 0;
	public int LastWeaponPosition = 0;
	public int LastArmorPosition = 0;
	public int LastAccessoryPosition = 0;

    private bool _pauseShown = false;
    private float _lastToggle;
    private float _toggleLockout = 0.5f;

	private MagicPresenter _magic;
	private PauseInterface _interface;
	private PausePresenter _presenter;
	private InventoryPresenter _items;
	private SettingsPresenter _settings;
	private MemberStatPresenter _member;
	private EquipmentPresenter _equipment;
	private MemberSelectPresenter _memberSelect;

	private PartyManager _party;
	private ControlManager _controls;
	private InventoryManager _inventory;

	private PlayableCharacter CurrentPartyMember
	{
		get { return _party.FindCharacterInPartyByPosition(CurrentPartyMemberIndex, CurrentPartyId); }
	}

	private InventoryItem CurrentInventoryItem
	{
		get { return _inventory.ActiveInventory.Items[CurrentInventoryPosition]; }
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_items = GetComponentInChildren<InventoryPresenter>();
		_interface = GetComponentInChildren<PauseInterface>();
		_presenter = GetComponentInChildren<PausePresenter>();
		_settings = GetComponentInChildren<SettingsPresenter>();
		_member = GetComponentInChildren<MemberStatPresenter>();
		_equipment = GetComponentInChildren<EquipmentPresenter>();
		_magic = GetComponentInChildren<MagicPresenter>();
		_memberSelect = GetComponentInChildren<MemberSelectPresenter>();

		_party = PartyManager.Instance;
		_controls = ControlManager.Instance;
		_inventory = InventoryManager.Instance;

		if(_interface == null)
			DebugMessage("Could not find a Pause Interface in the children of " + gameObject.name + "!", LogLevel.Warning);

		if(_presenter == null)
			DebugMessage("Could not find a Pause Presenter in the children of " + gameObject.name + "!", LogLevel.Warning);

		if(_settings == null)
			DebugMessage("Could not find a Settings Presenter in the children of " + gameObject.name + "!", LogLevel.Warning);

		if(_equipment == null)
			DebugMessage("Could not find an Equipment Presenter in the children of " + gameObject.name + "!", LogLevel.Warning);

		if(_magic == null)
			DebugMessage("Could not find an Magic Presenter in the children of " + gameObject.name + "!", LogLevel.Warning);
	}

	#endregion Engine Hooks

	#region Methods

    public void TogglePause()
    {
        if (Time.time < _lastToggle + _toggleLockout)
            return;

        _lastToggle = Time.time;

        if(! _pauseShown)
        {
            Pause();
            _pauseShown = true;
        }
        else
        {
            Unpause();
            _pauseShown = false;
        }
    }

	public void Pause()
	{
		OpenPauseMenu();
		_controls.RadiateSuspendCommand();
	}

	public void Unpause()
	{
		ClosePauseMenu();
		_controls.RadiateResumeCommand();
	}

	public void PresentInterface(bool isVisible)
	{
		_interface.SetVisibility(isVisible);
	}

	public void OpenPauseMenu()
	{
		DebugMessage("Opening pause menu from game object: " + gameObject.name);

		CurrentInventoryPosition = _inventory.FindNextPositionOfAvailableItem(true, -1);
		PrepMemberStatPresenter();
		PrepMemberSelectPresenter();

		_interface.SetVisibility(false);
		_presenter.SetVisibility(true);

		OpenEquipment();
	}

	public void PrepMemberStatPresenter()
	{
		_member.ReloadCharacterStats(CurrentPartyMember);
	}

	public void PrepMemberSelectPresenter()
	{
		_memberSelect.UpdateMemberName(CurrentPartyMember);
		_memberSelect.SetButtonVisibility(_party.Characters.Count > 1);
	}

	public void PrepEquipmentPresenter()
	{
		PlayableCharacter character = CurrentPartyMember;
		_member.ReloadCharacterStats(character);
		_equipment.ReloadCharacterEquipment(character);
		
		bool hasWeapons = _inventory.ActiveInventory.HasItemsOfType(ItemType.Weapon);
		bool hasArmor = _inventory.ActiveInventory.HasItemsOfType(ItemType.Armor);
		bool hasAccessories = _inventory.ActiveInventory.HasItemsOfType(ItemType.Accessory);
		_equipment.SetEquipmentButtonVisibilities(hasWeapons, hasArmor, hasAccessories);
	}

	public void OpenItems()
	{
		DebugMessage("Opening inventory from game object: " + gameObject.name);
		PrepItemsPresenter();
		PresentItemInterface();
	}

	public void OpenFilteredItems(ItemType itemType)
	{
		DebugMessage("Opening inventory of " + itemType + "s from game object: " + gameObject.name);
		PrepFilteredItemsPresenter(itemType);
		PresentItemInterface();
	}

	public void PresentItemInterface()
	{
		_memberSelect.SetButtonVisibility(_party.Characters.Count > 1);
		_memberSelect.SetVisibility(true);
		_member.SetVisibility(true);
		_items.SetVisibility(true);
		
		_equipment.SetVisibility(false);
		_settings.SetVisibility(false);
		_magic.SetVisibility(false);
	}

	public void PrepItemsPresenter()
	{
		List<INamed> namedObjects = new List<INamed>();
		foreach(INamed item in _inventory.ActiveInventory.Items)
		{
			namedObjects.Add((INamed) item);
		}

		DebugMessage("Prepped " + namedObjects.Count + " items for the inventory presenter...");
		_items.LoadItems(namedObjects);
		DebugMessage("Loaded " + _items.ItemGrid.DataElements.Count + " items.");
		DebugMessage("The Inventory Presenter's skill grid has " + _items.ItemGrid.ButtonCount + " buttons.");
	}

	public void PrepFilteredItemsPresenter(ItemType itemType)
	{
		List<INamed> namedObjects = new List<INamed>();
		foreach(InventoryItem item in _inventory.ActiveInventory.Items)
		{
			if(item.ItemType == itemType)
				namedObjects.Add((INamed) item);
		}

		DebugMessage("Prepped " + namedObjects.Count + " items for the inventory presenter...");
		_items.LoadItems(namedObjects);
		DebugMessage("Loaded " + _items.ItemGrid.DataElements.Count + " items.");
		DebugMessage("The Inventory Presenter's skill grid has " + _items.ItemGrid.ButtonCount + " buttons.");
	}

	public void OpenMagic()
	{
		DebugMessage("Opening magic from game object: " + gameObject.name);
		PrepMagicPresenter();
		_magic.SetVisibility(true);

		// Hide the other presenters...
		_items.SetVisibility(false);
		_equipment.SetVisibility(false);
		_settings.SetVisibility(false);
	}

	private void PrepMagicPresenter()
	{
		List<INamed> namedSkills = new List<INamed>();
		foreach(Ability ability in CurrentPartyMember.Abilities)
		{
			namedSkills.Add((INamed) ability);
		}

		DebugMessage("Prepped " + namedSkills.Count + " skills for the Ability Presenter!");
		_magic.LoadSkills(namedSkills);
		DebugMessage("Loaded " + _magic.Skills.DataElements.Count + " abilities.");
		DebugMessage("The Ability Presenter's skill grid has " + _magic.Skills.ButtonCount + " buttons.");
	}

	public void ClosePauseMenu()
	{
		DebugMessage("Closing pause menu from game object: " + gameObject.name);
		_interface.SetVisibility(true);

		// Hide everything else...
		_items.SetVisibility(false);
		_magic.SetVisibility(false);
		_member.SetVisibility(false);
		_settings.SetVisibility(false);
		_equipment.SetVisibility(false);
		_presenter.SetVisibility(false);
		_memberSelect.SetVisibility(false);
		_memberSelect.SetButtonVisibility(_party.Characters.Count > 1);
	}

	public void OpenEquipment()
	{
		DebugMessage("Opening stats and equipment...");

		_equipment.SetVisibility(true);
		PrepEquipmentPresenter();

		_memberSelect.SetVisibility(true);
		_member.SetVisibility(true);
		_presenter.SetVisibility(true);

		_items.SetVisibility(false);
		_magic.SetVisibility(false);
		_settings.SetVisibility(false);
	}

	public void OpenSettings()
	{
		DebugMessage("Opening settings menu from game object: " + gameObject.name);
		_settings.SetVisibility(true);

		// Hide everything else...
		_items.SetVisibility(false);
		_magic.SetVisibility(false);
		_member.SetVisibility(false);
		_equipment.SetVisibility(false);
		_interface.SetVisibility(false);
		_memberSelect.SetVisibility(false);
		_presenter.SetVisibility(false);
	}

	public void EquipItem(InventoryItem item)
	{
		PlayableCharacter character = CurrentPartyMember;

		switch(item.ItemType)
		{
			case ItemType.Weapon:
				DebugMessage(string.Format("Swapping {0}'s {1} for a {2}", character.Name, character.Weapon.Name, item.Name));
				InventoryItem equippedWeapon = character.Weapon;
				_inventory.GainItem(equippedWeapon.Clone() as InventoryItem);
				character.UnequipItem(ItemType.Weapon);
				
				character.EquipItem(ItemType.Weapon, item);
				_equipment.ReloadAccessoryStats(character.Weapon);
				break;

			case ItemType.Armor:
				DebugMessage(string.Format("Swapping {0}'s {1} for a {2}", character.Name, character.Armor.Name, item.Name));
				InventoryItem equippedArmor = character.Armor;
				_inventory.GainItem(equippedArmor.Clone() as InventoryItem);
				character.UnequipItem(ItemType.Armor);
				
				character.EquipItem(ItemType.Armor, item);
				_equipment.ReloadAccessoryStats(character.Armor);
				break;

			case ItemType.Accessory:
				DebugMessage(string.Format("Swapping {0}'s {1} for a {2}", character.Name, character.Accessory.Name, item.Name));
				InventoryItem equippedAccessory = character.Accessory;
				_inventory.GainItem(equippedAccessory.Clone() as InventoryItem);
				character.UnequipItem(ItemType.Accessory);
				
				character.EquipItem(ItemType.Accessory, item);
				_equipment.ReloadAccessoryStats(character.Accessory);
				break;

			default:
				throw new Exception("Unexpected item type: " + item.ItemType);
		}

		_inventory.LoseItem(item);
		_member.ReloadCharacterStats(character);
		OpenEquipment();
	}

	public void UseItem(InventoryItem item)
	{
		PlayableCharacter character = CurrentPartyMember;

		switch(item.ItemType)
		{
			case ItemType.Consumable:
				character.ApplyItem(item);
				_inventory.LoseItem(item);
				_member.ReloadCharacterStats(character);

				if(! item.IsAvailable)
				{
					_items.HideCommandBar();
				}
				else
				{
					_items.LoadItemInCommandBar(item);
				}	

				PrepItemsPresenter();
				return;

			default:
				throw new Exception("Unexpected item type: " + item.ItemType);
		}
	}

	public void ChangeMember(int changeDirection)
	{
		CurrentPartyMemberIndex += changeDirection;
		if(CurrentPartyMemberIndex < 0)
		{
			CurrentPartyMemberIndex = _party.Characters.Count - 1;
		}

		if(CurrentPartyMemberIndex >= _party.Characters.Count)
		{
			CurrentPartyMemberIndex = 0;
		}

		_memberSelect.UpdateMemberName(CurrentPartyMember);
	}

	#endregion Methods
}
