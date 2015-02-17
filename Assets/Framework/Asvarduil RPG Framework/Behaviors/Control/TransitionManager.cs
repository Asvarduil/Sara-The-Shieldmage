using UnityEngine;
using System;
using System.Collections;

public class TransitionManager : ManagerBase<TransitionManager>
{
	#region Variables / Properties

    public float FadeRate = 0.05f;
	public SceneState OriginalState;
	public SceneState TargetState;

    private string _currentSceneName;
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

        StartCoroutine(SceneChangeProcess());
	}

	private IEnumerator SceneChangeProcess()
	{
        ControlManager.Instance.RadiateSuspendCommand();
        _currentSceneName = Application.loadedLevelName;

        Fader fader = FindObjectOfType<Fader>();
		fader.FadeOut(FadeRate);

        // If loading a different level, fade the music too.
        if (_targetSceneName != _currentSceneName)
        {
            Maestro maestro = Maestro.Instance;
            maestro.FadeOut(FadeRate);
        }

        // Wait for the fade...
		while (!fader.ScreenHidden)
			yield return 0;

        // If loading a different level, load it.
        if (_targetSceneName != _currentSceneName)
        {
            Application.LoadLevel(_targetSceneName);
        }
        // Otherwise, move the player.  Fade in and restore control once that's done.
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = TargetState.Position;
            player.transform.rotation = Quaternion.Euler(TargetState.Rotation);

            fader.FadeIn(FadeRate);
            while (!fader.ScreenShown)
                yield return 0;

            ControlManager.Instance.RadiateResumeCommand();
        }
	}

	#endregion Methods
}
