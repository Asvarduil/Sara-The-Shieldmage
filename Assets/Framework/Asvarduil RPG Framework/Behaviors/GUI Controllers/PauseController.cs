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

    private AbilityDatabase _abilityDB;
	private MemberAbilityPresenter _magic;
    private ActiveQuestPresenter _quests;
	private PauseInterface _interface;
	private PauseMenuPresenter _presenter;
	private PartyInventoryPresenter _items;
	private SettingsPresenter _settings;
    private StatPresenter _stats;
	private EquippedItemsPresenter _equipment;
    private SelectMemberPresenter _selectMember;
    private SelectedItemPresenter _selectedItem;

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
        _abilityDB = AbilityDatabase.Instance;

        _interface = GetComponentInChildren<PauseInterface>();
        _presenter = GetComponentInChildren<PauseMenuPresenter>();
        _selectMember = GetComponentInChildren<SelectMemberPresenter>();
        _stats = GetComponentInChildren<StatPresenter>();
        _equipment = GetComponentInChildren<EquippedItemsPresenter>();
        _items = GetComponentInChildren<PartyInventoryPresenter>();
        _selectedItem = GetComponentInChildren<SelectedItemPresenter>();
        _magic = GetComponentInChildren<MemberAbilityPresenter>();
        _quests = GetComponentInChildren<ActiveQuestPresenter>();
		_settings = GetComponentInChildren<SettingsPresenter>();

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

        if (_items == null)
            DebugMessage("Could not find an Inventory Presenter in the children of " + gameObject.name + "!", LogLevel.Warning);

        if (_selectedItem == null)
            DebugMessage("Could not find a Selected Item Presenter in the children of " + gameObject.name + "!", LogLevel.Warning);

		if(_magic == null)
			DebugMessage("Could not find an Magic Presenter in the children of " + gameObject.name + "!", LogLevel.Warning);
	}

	#endregion Engine Hooks

	#region Methods

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

	public void OpenPauseMenu()
	{
		DebugMessage("Opening pause menu from game object: " + gameObject.name);

		CurrentInventoryPosition = _inventory.FindNextPositionOfAvailableItem(true, -1);
		PrepMemberStatPresenter();
		PrepMemberSelectPresenter();

		_presenter.PresentGUI(true);

		OpenEquipment();
	}

	public void PrepMemberStatPresenter()
	{
		_stats.ReloadCharacterStats(CurrentPartyMember);
	}

	public void PrepMemberSelectPresenter()
	{
		_selectMember.UpdateMemberName(CurrentPartyMember);
		_selectMember.PresentButtons(_party.Characters.Count > 1);
	}

	public void PrepEquipmentPresenter()
	{
		PlayableCharacter character = CurrentPartyMember;
		_stats.ReloadCharacterStats(character);
		_equipment.ReloadCharacterEquipment(character);
		
		bool hasWeapons = _inventory.ActiveInventory.HasItemsOfType(ItemType.Weapon);
		bool hasArmor = _inventory.ActiveInventory.HasItemsOfType(ItemType.Armor);
		bool hasAccessories = _inventory.ActiveInventory.HasItemsOfType(ItemType.Accessory);
		_equipment.SetEquipmentButtonVisibilities(hasWeapons, hasArmor, hasAccessories);
	}

	public void OpenItems()
	{
		DebugMessage("Opening inventory from game object: " + gameObject.name);
        PresentItemInterface();
		PrepItemsPresenter();
	}

	public void OpenFilteredItems(ItemType itemType)
	{
		DebugMessage("Opening inventory of " + itemType + "s from game object: " + gameObject.name);
        PresentItemInterface();
		PrepFilteredItemsPresenter(itemType);
	}

	public void PresentItemInterface()
	{
		_selectMember.PresentButtons(_party.Characters.Count > 1);
        _selectMember.PresentGUI(true);
		_stats.PresentGUI(true);
		_items.PresentGUI(true);
        _selectedItem.PresentGUI(false);
		
		_equipment.PresentGUI(false);
        _settings.PresentGUI(false);
        _quests.PresentGUI(false);
		_magic.PresentGUI(false);
	}

    public void SelectItem(InventoryItem item)
    {
        _selectedItem.PresentGUI(true);
        _selectedItem.LoadItem(item);
    }

	public void PrepItemsPresenter()
	{
		_items.LoadItems(_inventory.ActiveInventory.Items);
	}

	public void PrepFilteredItemsPresenter(ItemType itemType)
	{
		List<InventoryItem> filteredItems = new List<InventoryItem>();
		foreach(InventoryItem item in _inventory.ActiveInventory.Items)
		{
			if(item.ItemType == itemType)
				filteredItems.Add(item);
		}

		_items.LoadItems(filteredItems);
	}

	public void OpenMagic()
	{
		DebugMessage("Opening magic from game object: " + gameObject.name);
        List<Ability> abilities = _abilityDB.GetListByAbilityNames(CurrentPartyMember.AbilityNames);
        CurrentPartyMember.Abilities = abilities;

        _magic.LoadAbilities(abilities);
        _magic.LoadAbilityAtIndex(0);
        _magic.PresentGUI(true);

		// Hide the other presenters...
		_items.PresentGUI(false);
        _selectedItem.PresentGUI(false);
        _quests.PresentGUI(false);
        _settings.PresentGUI(false);
		_equipment.PresentGUI(false);
	}

	public void ClosePauseMenu()
	{
		DebugMessage("Closing pause menu from game object: " + gameObject.name);
        _interface.PreparePauseInterface();

		// Hide everything else...
		_items.PresentGUI(false);
        _selectedItem.PresentGUI(false);
		_magic.PresentGUI(false);
        _quests.PresentGUI(false);
		_stats.PresentGUI(false);
        _settings.PresentGUI(false);
		_equipment.PresentGUI(false);
		_presenter.PresentGUI(false);
        _selectMember.PresentGUI(false);
		_selectMember.PresentButtons(false);
	}

	public void OpenEquipment()
	{
		DebugMessage("Opening stats and equipment...");

		_equipment.PresentGUI(true);
		PrepEquipmentPresenter();

		_selectMember.PresentGUI(true);
		_stats.PresentGUI(true);
		_presenter.PresentGUI(true);
        _quests.PresentGUI(false);
		_items.PresentGUI(false);
        _selectedItem.PresentGUI(false);
		_magic.PresentGUI(false);
        _settings.PresentGUI(false);
	}

    public void OpenQuests()
    {
        DebugMessage("Opening quests menu from game object: " + gameObject.name);
        _quests.ShowPresenter();
        _quests.PresentGUI(true);

        _items.PresentGUI(false);
        _selectedItem.PresentGUI(false);
        _magic.PresentGUI(false);
        _stats.PresentGUI(false);
        _settings.PresentGUI(false);
        _equipment.PresentGUI(false);
        _presenter.PresentGUI(false);
        _selectMember.PresentGUI(false);
        _selectMember.PresentButtons(false);
    }

	public void OpenSettings()
	{
		DebugMessage("Opening settings menu from game object: " + gameObject.name);
        _settings.PresentGUI(true);

		// Hide everything else...
		_items.PresentGUI(false);
        _selectedItem.PresentGUI(false);
		_magic.PresentGUI(false);
		_stats.PresentGUI(false);
        _quests.PresentGUI(false);
		_equipment.PresentGUI(false);
        _presenter.PresentGUI(false);
		_selectMember.PresentGUI(false);
        _selectMember.PresentButtons(false);
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
		_stats.ReloadCharacterStats(character);
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
				_stats.ReloadCharacterStats(character);

				if(! item.IsAvailable)
				{
					_selectedItem.PresentGUI(false);
				}
				else
				{
					_selectedItem.LoadItem(item);
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

		_selectMember.UpdateMemberName(CurrentPartyMember);
        PrepMemberStatPresenter();
        _magic.LoadAbilities(CurrentPartyMember.Abilities);
        _magic.LoadAbilityAtIndex(0);
        PrepEquipmentPresenter();
	}

    public Ability GetAbilityAtIndexOnCurrentMember(int index)
    {
        try
        {
            Ability ability = CurrentPartyMember.Abilities[index];
            return ability;
        }
        catch
        {
            return null;
        }
    }

	#endregion Methods
}
