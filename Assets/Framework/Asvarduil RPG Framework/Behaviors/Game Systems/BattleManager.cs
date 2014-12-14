using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class BattleManager : ManagerBase<BattleManager>
{
	#region Variables / Properties

	public string PlayerTag = "Player";
	public List<string> EnemyNames;
	public string BattleScene;
	public AudioClip BattleTheme;

	private TransitionManager _transitionManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_transitionManager = TransitionManager.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public void PrepareBattle(List<string> enemies, string targetScene, AudioClip battleTheme = null)
	{
		EnemyNames = enemies;
		BattleScene = targetScene;
		BattleTheme = battleTheme;

		GameObject playerCharacter = GameObject.FindGameObjectWithTag(PlayerTag);
		SceneState currentState = new SceneState
		{
			SceneName = Application.loadedLevelName,
			Position = playerCharacter.transform.position,
			Rotation = playerCharacter.transform.rotation.eulerAngles
		};

		// Prep transition manager for a reversal, upon victory.
		_transitionManager.PrepareSceneChange(currentState, true);
	}

	public void InitiateBattle()
	{
		Application.LoadLevel(BattleScene);
	}

	#endregion Methods
}
