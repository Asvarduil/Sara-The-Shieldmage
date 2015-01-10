using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class ConversationSoundEvents : DebuggableBehavior 
{
	#region Variables / Properties

	public List<AudioClip> AllAudio;

	private Maestro _maestro;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_maestro = Maestro.Instance;
	}

	#endregion Engine Hooks

	#region Methods

	public void StopBGM(List<string> args)
	{
		_maestro.StopBGM();
	}

	public void ResumeBGM(List<string> args)
	{
		_maestro.ResumeBGM();
	}

	public void CueSounds(List<string> args)
	{
		PlayOneShot(args);
	}

    public void HaltThenPlay(List<string> args)
    {
        _maestro.StopBGM();
        PlayOneShot(args);
    }

	public void PlayOneShot(List<string> args)
	{
		string soundName;
		if(! string.IsNullOrEmpty(args[0]))
		{
			soundName = args[0];
		}
		else
		{
			DebugMessage("No effect name was sent!", LogLevel.LogicError);
			return;
		}

		float musicVolume;
		if(! string.IsNullOrEmpty(args[1]))
		{
			musicVolume = Convert.ToSingle(args[1]);
		}
		else
		{
			musicVolume = 1.0f;
		}

		DebugMessage("Playing one-shot sound: " + soundName + " at " + (musicVolume * 100) + "% volume.");

		AudioClip effect = AllAudio.FirstOrDefault(a => a.name == soundName);
		_maestro.PlayOneShot(effect, musicVolume);
	}

	#endregion Methods
}
