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

    public override CombatEntity DetermineTarget(System.Collections.Generic.List<CombatEntity> availableTargets)
    {
        int index = 0;
        List<CombatEntity> targets = availableTargets;
        
        switch(_ability.TargetType)
        {
            case AbilityTargetType.TargetAlly:
                targets = availableTargets.Where(t => t is PlayableCharacter).ToList();
                index = Random.Range(0, targets.Count);
                break;

            case AbilityTargetType.TargetEnemy:
                targets = availableTargets.Where(t => t is Enemy).ToList();
                index = Random.Range(0, targets.Count);
                break;

            // TODO: Support group- and all- attacks.
        }

        return targets[index];
    }

    #region Methods

    #endregion Methods
}
