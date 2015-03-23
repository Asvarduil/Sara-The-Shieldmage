using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class AIBase : DebuggableBehavior
{
    #region Variables / Properties

    protected Ability _selectedAbility;

    #endregion Variables / Properties

    #region Methods

    public abstract Ability DetermineAction(List<Ability> abilities);

    public abstract List<CombatEntity> DetermineTarget(List<CombatEntity> availableTargets);

    #endregion Methods
}
