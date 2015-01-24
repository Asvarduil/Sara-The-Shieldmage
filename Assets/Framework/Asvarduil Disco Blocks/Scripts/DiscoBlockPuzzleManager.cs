using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DiscoBlockPuzzleManager : DebuggableBehavior
{
    #region Variables / Properties

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

    #endregion Variables / Properties

    #region Hooks

    // Use this for initialization
	public void Awake () 
    {
        IntroduceManagerToChildren();
	}

    public void ActivateSwitch(DiscoBlockSwitch currentSwitch)
    {
        if (_currentSwitch != null)
        {
            _currentSwitch.State = DiscoBlockState.Passed;
            SetStateOfCurrentBlockNeighbors(DiscoBlockState.Untouched);
        }

        // Update the current switch...
        _currentSwitch = currentSwitch;
        _currentSwitch.State = DiscoBlockState.Active;
        SetStateOfCurrentBlockNeighbors(DiscoBlockState.Activatable);
        DebugMessage("Disco Block Switch " + _currentSwitch.name + " has been activated!");
    }

    #endregion Hooks

    #region Methods

    private void SetStateOfCurrentBlockNeighbors(DiscoBlockState newState)
    {
        if (_currentSwitch == null)
            return;

        if (_currentSwitch.ReadyWhenActivated.Count == 0)
            return;

        for (int i = 0; i < _currentSwitch.ReadyWhenActivated.Count; i++)
        {
            DiscoBlockSwitch currentAffected = _currentSwitch.ReadyWhenActivated[i];

            // Don't modify active or passed blocks!
            if(currentAffected.State == DiscoBlockState.Untouched
               || currentAffected.State == DiscoBlockState.Activatable)
                currentAffected.State = newState;
        }
    }

    private void IntroduceManagerToChildren()
    {
        _switches = GetComponentsInChildren<DiscoBlockSwitch>().ToList();

        for (int i = 0; i < _switches.Count; i++)
        {
            DiscoBlockSwitch currentSwitch = _switches[i];
            currentSwitch.Manager = this;

            // If the block is active, it'll be our first current switch.  Activate!
            if (currentSwitch.State == DiscoBlockState.Active)
                ActivateSwitch(currentSwitch);
        }
    }

    #endregion Methods
}
