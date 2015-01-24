using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBlockPuzzleManager : DebuggableBehavior
{
    #region Variables / Properties

    public string TreasureKey;
    public List<PuzzleSolvedObjectBase> AffectedObjects;

    public bool IsSolved
    {
        get 
        {
            bool result = true;

            // The puzzle is solved if all of the related blocks are either
            // passed over or active.
            for(int i = 0; i < _switches.Count; i++)
            {
                DiscoBlockSwitch current = _switches[i];
                result &= (current.State == DiscoBlockState.Passed 
                           || current.State == DiscoBlockState.Active);
            }

            return result;
        }
    }

    private DiscoBlockSwitch _currentSwitch;
    private List<DiscoBlockSwitch> _switches;

    private TreasureManager _treasures;

    #endregion Variables / Properties

    #region Hooks

	public void Awake() 
    {
        _treasures = TreasureManager.Instance;

        if (_treasures.ObtainedTreasures.Contains(TreasureKey))
        {
            ActivateAll();
            PuzzleIsComplete();
        }
        else
        {
            IntroduceManagerToChildren();
        }
	}

    public void ActivateSwitch(DiscoBlockSwitch currentSwitch)
    {
        // Check the argument first.
        if (currentSwitch == null)
            throw new ArgumentNullException("currentSwitch", "currentSwitch cannot be null!");

        // Only set the current switch to 'passed', and reset its neighbors, if it exists.
        if (_currentSwitch != null)
        {
            _currentSwitch.SetPassed();
            SetNeighboringBlockState(DiscoBlockState.Untouched);
        }

        // Update the current switch...
        _currentSwitch = currentSwitch;
        _currentSwitch.Activate();
        SetNeighboringBlockState(DiscoBlockState.Activatable);
        DebugMessage("Disco Block Switch " + _currentSwitch.name + " has been activated!");

        CheckForCompletion();
    }

    #endregion Hooks

    #region Methods

    private void ActivateAll()
    {
        _switches = GetComponentsInChildren<DiscoBlockSwitch>().ToList();

        for (int i = 0; i < _switches.Count; i++)
        {
            DiscoBlockSwitch currentSwitch = _switches[i];
            currentSwitch.Manager = this;

            currentSwitch.State = DiscoBlockState.Passed;
            currentSwitch.UpdateColor();
        }
    }

    private void IntroduceManagerToChildren()
    {
        _switches = GetComponentsInChildren<DiscoBlockSwitch>().ToList();
        DebugMessage("There are " + _switches.Count + " switches in this puzzle.");

        for (int i = 0; i < _switches.Count; i++)
        {
            DiscoBlockSwitch currentSwitch = _switches[i];
            currentSwitch.Manager = this;

            if (currentSwitch.State == DiscoBlockState.Active)
            {
                _currentSwitch = currentSwitch;
                SetNeighboringBlockState(DiscoBlockState.Activatable);
            }
        }
    }

    private void SetNeighboringBlockState(DiscoBlockState state)
    {
        if (_currentSwitch == null)
            return;

        if (_currentSwitch.ReadyWhenActivated.Count == 0)
            return;

        for (int i = 0; i < _currentSwitch.ReadyWhenActivated.Count; i++)
        {
            DiscoBlockSwitch currentAffected = _currentSwitch.ReadyWhenActivated[i];

            // Don't modify active or passed blocks!
            if (currentAffected.State == DiscoBlockState.Active
                || currentAffected.State == DiscoBlockState.Passed)
                continue;

            currentAffected.State = state;
            currentAffected.UpdateColor();
        }
    }

    private void PuzzleIsComplete()
    {
        for(int i = 0; i < AffectedObjects.Count; i++)
        {
            PuzzleSolvedObjectBase current = AffectedObjects[i];
            current.OnPuzzleAlreadySolved();
        }
    }

    private void CheckForCompletion()
    {
        if (!IsSolved)
            return;

        if (AffectedObjects.Count == 0)
            return;

        _treasures.MarkTreasureAsObtained(TreasureKey);

        for(int i = 0; i < AffectedObjects.Count; i++)
        {
            PuzzleSolvedObjectBase current = AffectedObjects[i];
            current.OnPuzzleSolved();
        }
    }

    #endregion Methods
}
