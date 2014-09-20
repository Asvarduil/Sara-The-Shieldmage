using UnityEngine;
using System.Collections;

public class TransitionManager : ManagerBase<TransitionManager>
{
	#region Variables / Properties

	public SceneState OriginalState;
	public SceneState TargetState;

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
			Application.LoadLevel(OriginalState.SceneName);
		}
		else
		{
			DebugMessage("Transitioning to the target scene...");
			Application.LoadLevel(TargetState.SceneName);
		}
	}

	#endregion Methods
}
