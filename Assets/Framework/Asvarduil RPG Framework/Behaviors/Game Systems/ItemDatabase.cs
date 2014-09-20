using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class ItemDatabase : ManagerBase<ItemDatabase> 
{
	#region Variables / Properties

	public bool TryDownloadingBlob = false;

	public TextAsset LocalBlob;
	public string RemoteBlobUrl;
	public string RawBlob;

	public List<InventoryItem> Items;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		LoadItemsFromJson();
	}

	#endregion Engine Hooks

	#region Business Object Retrieval Methods

	public InventoryItem FindItemWithName(string name)
	{
		if(string.IsNullOrEmpty(name))
			throw new ArgumentNullException("Must specify an item name to find.");

		return Items.FirstOrDefault(i => i.Name == name);
	}

	#endregion Business Object Retrieval Methods

	#region Data Access Methods

	public void LoadItemsFromJson()
	{
		if(TryDownloadingBlob)
			StartCoroutine(DownloadBlob());

		if(string.IsNullOrEmpty(RawBlob))
		{
			RawBlob = FetchLocalBlob();
			
			if(string.IsNullOrEmpty(RawBlob))
				return;
			else
				MapBlob();
		}
		else
		{
			MapBlob();
		}
	}

	private IEnumerator DownloadBlob()
	{
		if(! TryDownloadingBlob)
			yield break;
		
		WWW dataInterface = new WWW(RemoteBlobUrl);
		
		while(! dataInterface.isDone)
			yield return 0;
		
		RawBlob = dataInterface.text;
		
		// TODO: Check that the blob has not been corrupted.
		//       If it has, set the raw blob to empty.
		
		dataInterface.Dispose();
	}

	protected string FetchLocalBlob()
	{
		if(LocalBlob == null)
			return string.Empty;
		
		return LocalBlob.text;
	}

	public void MapBlob()
	{
		var parsed = JSON.Parse(RawBlob);
		var items = parsed["Items"].AsArray;

		Items = new List<InventoryItem>();
		foreach(var item in items.Childs)
		{
			InventoryItem newItem = new InventoryItem();

			newItem.Name = item["Name"];
			newItem.Description = item["Description"];
			newItem.Value = item["Value"].AsInt;

			string itemType = item["ItemType"];
			newItem.ItemType = (ItemType) Enum.Parse(typeof(ItemType), itemType);
			newItem.Quantity = 0;

			var equipEffects = item["EquipmentEffects"].AsArray;
			newItem.EquipmentEffects = new List<ItemEffect>();

			foreach(var effect in equipEffects.Childs)
			{
				ItemEffect newEquipEffect = new ItemEffect();

				newEquipEffect.TargetStat = effect["TargetStat"];
				newEquipEffect.FixedEffect = effect["FixedEffect"].AsInt;
				newEquipEffect.ScalingEffect = effect["ScalingEffect"].AsFloat;
				newEquipEffect.EffectDuration = effect["EffectDuration"].AsFloat;

				newItem.EquipmentEffects.Add(newEquipEffect);
			}

			var consumeEffects = item["ConsumeEffects"].AsArray;
			newItem.ConsumeEffects = new List<ItemEffect>();

			foreach(var effect in consumeEffects.Childs)
			{
				ItemEffect newUseEffect = new ItemEffect();
				
				newUseEffect.TargetStat = effect["TargetStat"];
				newUseEffect.FixedEffect = effect["FixedEffect"].AsInt;
				newUseEffect.ScalingEffect = effect["ScalingEffect"].AsFloat;
				newUseEffect.EffectDuration = effect["EffectDuration"].AsFloat;
				
				newItem.ConsumeEffects.Add(newUseEffect);
			}

			Items.Add(newItem);
		}
	}

	#endregion Data Access Methods
}
