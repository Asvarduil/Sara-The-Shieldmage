using UnityEngine;
using System.Collections;

public class AudioManager : ManagerBase<AudioManager>
{
	#region Variables / Properties

	public bool SoundEnabled;
	public float MasterVolume;
	public float MusicVolume;
	public float EffectVolume;

	public float EffectiveMasterVolume
	{
		get { return SoundEnabled ? MasterVolume : 0.0f; }
	}

	#endregion Variables / Properties
}
