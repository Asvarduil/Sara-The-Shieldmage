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
                _material.SetColor(MaterialColorProperty, ActivatedColors[0]);
                _lastColorUpdate = Time.time;
                break;
        }
    }

    public void Update()
    {
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

            _lastColorUpdate = Time.time;
            _material.SetColor(MaterialColorProperty, ActiveColor);
            Manager.ActivateSwitch(this);
        }
    }

    #endregion Hooks

    #region Methods

    private void UpdateColor()
    {
        if (State != DiscoBlockState.Passed)
            return;

        if (Time.time < _lastColorUpdate + ColorSwitchRate)
            return;

        // Loop the disco colors!
        _currentColorId++;
        if (_currentColorId > ActivatedColors.Count - 1)
            _currentColorId = 0;

        _material.SetColor(MaterialColorProperty, ActivatedColors[_currentColorId]);
        _lastColorUpdate = Time.time;
    }

    #endregion Methods
}
