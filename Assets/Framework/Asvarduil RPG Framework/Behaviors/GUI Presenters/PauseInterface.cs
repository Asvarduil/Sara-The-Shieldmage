using UnityEngine;
using System.Collections;

public class PauseInterface : DebuggableBehavior
{
	#region Variables / Properties

    public string PauseAxis = "Pause";
    public float PauseLockout = 0.1f;
    public AudioClip ButtonSound;

    private Maestro _maestro;
    private ControlManager _controls;
	private PauseController _controller;

    private bool _pauseShown = false;
    private float _lastPause;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
        _maestro = Maestro.Instance;
        _controls = ControlManager.Instance;
		_controller = PauseController.Instance;

		if(_controller == null)
			DebugMessage("Could not find a Pause Controller instance!", LogLevel.Warning);
	}

    public void Update()
    {
        if (_controls.GetAxisDown(PauseAxis)
            && Time.time > _lastPause + PauseLockout
            && !_pauseShown)
        {
            _maestro.PlayOneShot(ButtonSound);
            _controller.Pause();

            _lastPause = Time.time;
            _pauseShown = true;
        }
    }

	#endregion Engine Hooks

	#region Methods

    public void PreparePauseInterface()
    {
        _pauseShown = false;
        _lastPause = Time.time;
    }

	#endregion Methods
}
