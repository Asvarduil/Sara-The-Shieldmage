using UnityEngine;
using System.Collections.Generic;

public enum DiscoBlockState
{
    Untouched,
    Activatable,
    Active,
    Passed
}

public class DiscoBlockSwitch : DebuggableBehavior
{
    #region Variables / Properties

    public string AffectedTag = "Player";
    public string MaterialColorProperty = "_Color";

    public Color UntouchedColor;
    public Color ActivatableColor;
    public Color ActiveColor;
    public AudioClip ActivationSoundEffect;

    public float ColorSwitchRate = 0.5f;
    public List<Color> ActivatedColors;

    // Assigned by the parent object, which sniffs its children on Awake.
    public DiscoBlockPuzzleManager Manager;
    public DiscoBlockState State;

    public List<DiscoBlockSwitch> ReadyWhenActivated;

    private int _currentColorId = 0;
    private float _lastColorUpdate;

    private Material _material;
    private Maestro _maestro;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _maestro = Maestro.Instance;

        UpdateColor();
    }

    public void Update()
    {
        if (State != DiscoBlockState.Passed)
            return;

        UpdateColor();
    }

    public void OnTriggerEnter(Collider who)
    {
        // Unexpected objects can't alter disco blocks.
        if (who.tag != AffectedTag)
            return;

        DebugMessage("Disco Block Switch was stepped on!");

        if (State == DiscoBlockState.Passed 
            || State == DiscoBlockState.Active
            || State == DiscoBlockState.Untouched)
            return;

        if(State == DiscoBlockState.Activatable)
        {
            _maestro.PlayOneShot(ActivationSoundEffect);
            Manager.ActivateSwitch(this);
        }
    }

    public void Activate()
    {
        State = DiscoBlockState.Active;
        _material.SetColor(MaterialColorProperty, ActiveColor);
    }

    public void SetPassed()
    {
        _lastColorUpdate = Time.time;
        State = DiscoBlockState.Passed;
    }

    #endregion Hooks

    #region Methods

    public void UpdateColor()
    {
        if(_material == null)
            _material = renderer.material;

        switch (State)
        {
            case DiscoBlockState.Untouched:
                _material.SetColor(MaterialColorProperty, UntouchedColor);
                break;

            case DiscoBlockState.Activatable:
                _material.SetColor(MaterialColorProperty, ActivatableColor);
                break;

            case DiscoBlockState.Active:
                _material.SetColor(MaterialColorProperty, ActiveColor);
                break;

            case DiscoBlockState.Passed:
                if (Time.time < _lastColorUpdate + ColorSwitchRate)
                    return;

                // Loop the disco colors!
                _currentColorId++;
                if (_currentColorId > ActivatedColors.Count - 1)
                    _currentColorId = 0;

                _material.SetColor(MaterialColorProperty, ActivatedColors[_currentColorId]);
                _lastColorUpdate = Time.time;
                break;
        }
    }

    #endregion Methods
}
