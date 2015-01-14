using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class BattleTrigger : DebuggableBehavior
{
	#region Variables / Properties

	public string AffectedTag = "Player";

	public bool CheckThisRegion = false;
    public int MaxEnemies = 3;
    public float LoadLockout = 3.0f;
	public float BattleCheckRate = 0.5f;
	public float BattleLikelihood = 0.1f;
	public AudioClip BattleTheme;
	public string BattleScene;
	public List<string> EnemyNames;

	private float _lastCheck;
	private JrpgMapControlSystem _movement;
	private BattleManager _battleManager;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_battleManager = BattleManager.Instance;
		_movement = GameObject.FindGameObjectWithTag(AffectedTag).GetComponent<JrpgMapControlSystem>();

        // On load lockout.
        _lastCheck = Time.time + LoadLockout;
	}

	public void OnTriggerStay(Collider e)
	{
		if(e.tag != AffectedTag)
			return;

        ReacquirePlayerIfNotFound();

		// Don't trigger random battles if the player is not moving.
		if (! _movement.IsMoving)
			return;

		if (Time.time < _lastCheck + BattleCheckRate)
			return;

		_lastCheck = Time.time;
		
		if (Random.Range(0.0f, 1.0f) > BattleLikelihood)
			return;

        List<string> enemyFormation = RollEnemySet();
		
		_battleManager.PrepareBattle(enemyFormation, BattleScene, BattleTheme);
		// TODO: Trigger Battle Transition...
		_battleManager.InitiateBattle();
	}

	#endregion Engine Hooks

    #region Methods

    private List<string> RollEnemySet()
    {
        int enemyCount = Random.Range(1, MaxEnemies + 1);
        DebugMessage("The next battle will have " + enemyCount + " enemies.");

        List<string> enemyList = new List<string>();
        for(int i = 0; i < enemyCount; i++)
        {
            int nameIndex = Random.Range(0, EnemyNames.Count);
            enemyList.Add(EnemyNames[nameIndex]);
        }

        return enemyList;
    }

    private void ReacquirePlayerIfNotFound()
    {
        if (_movement == null)
            _movement = GameObject.FindGameObjectWithTag(AffectedTag).GetComponent<JrpgMapControlSystem>();

        if (_movement == null)
            throw new InvalidOperationException("There is no Game Object with tag " + AffectedTag + " with a JrpgMapControlSystem on it.");
    }

    #endregion Methods
}
