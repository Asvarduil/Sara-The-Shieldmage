using UnityEngine;
using System.Collections;

public class DeathReloadManager : ManagerBase<DeathReloadManager> 
{
	#region Variables / Properties

	private Fader _fader;
	private TransitionManager _transition;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_fader = FindObjectOfType<Fader>();
		_transition = TransitionManager.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public void ReloadLevel()
	{
		// TODO: Run a coroutine that fades first, then transitions.
		_transition.ChangeScenes(true);
	}

	#endregion Methods
}
