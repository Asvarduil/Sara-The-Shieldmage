using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class HealerAI : AIBase
{
    #region Variables / Properties

    public int turnToHealOn = 3;
    private int _turnCounter = 1;

    private bool IsHealingThisTurn
    {
        get { return _turnCounter % turnToHealOn == 0; }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        BattleEntity _piece = GetComponent<BattleEntity>();
        _piece.AI = this;
    }

    public override Ability DetermineAction(List<Ability> abilities)
    {
        // Every so many-th turn, heal yourself.  Otherwise, do something else.
        if (!IsHealingThisTurn)
        {
            _selectedAbility = abilities[0];
        }
        else
        {
            _selectedAbility = abilities[1];
        }

        return _selectedAbility;
    }

    public override CombatEntity DetermineTarget(List<CombatEntity> availableTargets)
    {
        int targetIndex = 0;
        CombatEntity result;

        if (!IsHealingThisTurn)
        {
            // Attack a random player.
            List<CombatEntity> playerList = availableTargets.Where(FilterPlayersByOnesStillAlive).ToList();

            if (playerList.Count > 1)
                targetIndex = Random.Range(0, playerList.Count);

            DebugMessage("Target player is at position: " + targetIndex + "; there are " + playerList.Count + " living players to kill.");
            result = playerList[targetIndex];
        }
        else
        {
            // Prioritize fellow enemies that have taken damage.  If none, heal yourself.
            List<CombatEntity> enemyList = availableTargets.Where(FilterEnemiesByOnesThatNeedHealing).ToList();

            if (enemyList.Count > 1)
                targetIndex = Random.Range(0, enemyList.Count);

            DebugMessage("Target enemy is at position: " + targetIndex + "; there are " + enemyList.Count + " living enemies to heal.");
            result = enemyList[targetIndex];
        }

        _turnCounter++;
        return result;
    }

    #endregion Hooks

    #region Methods

    private bool FilterPlayersByOnesStillAlive(CombatEntity current)
    {
        return current is PlayableCharacter
               && !current.Health.IsDead;
    }

    private bool FilterEnemiesByOnesThatNeedHealing(CombatEntity current)
    {
        return current is Enemy
               && !current.Health.IsDead
               && current.Health.HP < current.Health.MaxHP;
    }

    #endregion Methods
}
