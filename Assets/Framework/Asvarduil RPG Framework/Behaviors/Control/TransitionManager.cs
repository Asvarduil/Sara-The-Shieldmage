using UnityEngine;
using System;
using System.Collections;

public class TransitionManager : ManagerBase<TransitionManager>
{
	#region Variables / Properties

    public float FadeRate = 0.05f;
	public SceneState OriginalState;
	public SceneState TargetState;

	private bool _transitionStarted = false;
	private string _targetSceneName;

	#endregion Variables / Properties

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

        ControlManager.Instance.RadiateSuspendCommand();
        StartCoroutine(SceneChangeProcess());
	}

	private IEnumerator SceneChangeProcess()
	{
        Fader fader = FindObjectOfType<Fader>();
		fader.FadeOut(FadeRate);

        Maestro maestro = Maestro.Instance;
        maestro.FadeOut(FadeRate);

		while (!fader.ScreenHidden)
			yield return 0;
			
		Application.LoadLevel(_targetSceneName);
	}

	#endregion Methods
}
