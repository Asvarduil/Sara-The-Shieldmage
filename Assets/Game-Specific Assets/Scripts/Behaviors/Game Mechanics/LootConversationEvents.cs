using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class LootConversationEvents : DebuggableBehavior
{
	#region Variables / Properties

	public float playerZOffset = 0.25f;
	public List<LootPrefabPair> LootPrefabs;

	private GameObject _playerCharacter;
	private GameObject _prefabInstance;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_playerCharacter = GameObject.FindGameObjectWithTag("Player");
	}

	public void OnDestroy()
	{
		if(_prefabInstance != null)
			DestroyImmediate(_prefabInstance);
	}

	#endregion Engine Hooks

	#region Methods

	public void ShowLoot(List<string> args)
	{
		string loot = args[0];
		float x = _playerCharacter.transform.position.x + Convert.ToSingle(args[1]);
		float y = _playerCharacter.transform.position.y + Convert.ToSingle(args[2]);
		float z = _playerCharacter.transform.position.z + playerZOffset;

		Vector3 position = new Vector3(x, y, z);

		LootPrefabPair lootArtifact = LootPrefabs.FirstOrDefault(l => l.LootName == loot);
		if(lootArtifact == default(LootPrefabPair))
			throw new ArgumentException("Loot " + loot + " does not exist in the table of conversation loot prefabs.");

		_prefabInstance = (GameObject) GameObject.Instantiate(lootArtifact.Prefab, position, Quaternion.identity);
	}

	public void HideLoot(List<string> args)
	{
		if(_prefabInstance == null)
			return;

		GameObject.Destroy(_prefabInstance);
		_prefabInstance = null;
	}

	#endregion Methods
}
