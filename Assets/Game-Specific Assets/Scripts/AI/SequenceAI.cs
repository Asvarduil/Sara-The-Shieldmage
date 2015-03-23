using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class SequenceAI : AIBase
{
    #region Variables / Properties

    public List<string> moveSequence;

    private int _turnCounter = 1;
    private Ability _ability;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    public void Start()
    {
        BattleEntity _piece = GetComponent<BattleEntity>();
        _piece.AI = this;
    }

    public override Ability DetermineAction(System.Collections.Generic.List<Ability> abilities)
    {
        int index = _turnCounter % abilities.Count - 1;
        string abilityName = moveSequence[index];

        _ability = abilities.FirstOrDefault(a => a.Name == abilityName);
        if (_ability == default(Ability))
            _ability = abilities[0];

        return _ability;
    }

    public override List<CombatEntity> DetermineTarget(System.Collections.Generic.List<CombatEntity> availableTargets)
    {
        int index = 0;
        List<CombatEntity> results = new List<CombatEntity>();
        List<CombatEntity> targets = availableTargets;
        
        switch(_ability.TargetType)
        {
            case AbilityTargetType.TargetAlly:
                targets = availableTargets.Where(FilterPlayersByOnesStillAlive).ToList();
                index = Random.Range(0, targets.Count);
                results.Add(targets[index]);
                break;

            case AbilityTargetType.TargetEnemy:
                targets = availableTargets.Where(FilterEnemiesByOnesStillAlive).ToList();
                index = Random.Range(0, targets.Count);
                results.Add(targets[index]);
                break;

            case AbilityTargetType.AllAlly:
                targets = availableTargets.Where(FilterPlayersByOnesStillAlive).ToList();
                results.AddRange(targets);
                break;

            case AbilityTargetType.AllEnemy:
                targets = availableTargets.Where(FilterEnemiesByOnesStillAlive).ToList();
                results.AddRange(targets);
                break;

            default:
                throw new Exception("Unexpected Target Type: " + _ability.TargetType);
        }

        return results;
    }

    #region Methods

    private bool FilterPlayersByOnesStillAlive(CombatEntity current)
    {
        return current is PlayableCharacter
               && !current.Health.IsDead;
    }

    private bool FilterEnemiesByOnesStillAlive(CombatEntity current)
    {
        return current is Enemy
               && !current.Health.IsDead;
    }

    #endregion Methods
}
