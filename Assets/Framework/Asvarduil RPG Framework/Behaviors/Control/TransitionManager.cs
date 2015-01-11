using UnityEngine;
using System;
using System.Collections;

public class TransitionManager : ManagerBase<TransitionManager>
{
	#region Variables / Properties

	public float FadeRate = 0.05f;
	public SceneState OriginalState;
	public SceneState TargetState;

	private Fader _fader;
	private bool _transitionStarted = false;
	private string _targetSceneName;

	#endregion Variables / Properties

	#region Hooks

	public void Start()
	{
    		_fader = FindObjectOfType<Fader>();
	}

	#endregion Hooks

	#region Methods

	public void PrepareSceneChange(SceneState state, bool isSource = false)
	{
		if(isSource)
			OriginalState = state;
		else
			TargetState = state;
	}

	public void ChangeScenes(bool toSource = false)
	{
		_transitionStarted = true;

		if(toSource)
		{
			DebugMessage("Transitioning back to the source scene...");
			TargetState = OriginalState;
			_targetSceneName = OriginalState.SceneName;
		}
		else
		{
			DebugMessage("Transitioning to the target scene...");
			_targetSceneName = TargetState.SceneName;
		}

        	//StartCoroutine(SceneChangeProcess());
        	Application.LoadLevel(_targetSceneName);
	}

	private IEnumerator SceneChangeProcess()
	{
		_fader.FadeOut(FadeRate);

		while (!_fader.ScreenHidden)
			yield return 0;
			
		Application.LoadLevel(_targetSceneName);
		
	}

	#endregion Methods
}
