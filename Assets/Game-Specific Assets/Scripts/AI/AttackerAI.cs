using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class AttackerAI : AIBase
{
    #region Variables / Properties

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        BattleEntity _piece = GetComponent<BattleEntity>();
        _piece.AI = this;
    }

    public override Ability DetermineAction(List<Ability> abilities)
    {
        _selectedAbility = abilities[0];
        return _selectedAbility;
    }

    public override CombatEntity DetermineTarget(List<CombatEntity> availableTargets)
    {
        List<CombatEntity> playerList = availableTargets.Where(t => t is PlayableCharacter && !t.Health.IsDead).ToList();

        int targetIndex = Random.Range(0, playerList.Count - 1);
        return availableTargets[targetIndex];
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
