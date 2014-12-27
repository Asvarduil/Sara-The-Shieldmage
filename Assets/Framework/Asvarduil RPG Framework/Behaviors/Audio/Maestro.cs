using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
 
public class Maestro : DebuggableBehavior
{
	#region Variables / Properties

    public float loopTime = 0.0f;

	private AudioSource _soundSource;
	private AudioManager _audioManager;

	public static Maestro Instance
	{
		get { return FindObjectOfType<Maestro>(); }
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_audioManager = AudioManager.Instance;
		_soundSource = gameObject.GetComponentInChildren<AudioSource>();
	}

	public void Update()
	{
		AudioListener.volume = _audioManager.EffectiveMasterVolume;
		_soundSource.volume = _audioManager.MusicVolume;
	}

	#endregion Engine Hooks

	#region Methods

	public void StopBGM()
	{
		_soundSource.Stop();
	}

	public void ResumeBGM()
	{
		_soundSource.time = 0.0f;
		_soundSource.Play();
	}

	public void PlayOneShot(AudioClip oneShot, float? effectVolume = null)
	{
		if(oneShot == null)
			return;

		if(effectVolume == null)
			effectVolume = _audioManager.EffectVolume;

		_soundSource.PlayOneShot(oneShot, (float) effectVolume);
	}

	public void ChangeTunes(AudioClip newTune)
	{
		if(newTune == null)
			return;

		StopBGM();
		_soundSource.clip = newTune;
		_soundSource.time = 0.0f;
		ResumeBGM();
	}

	public void InterjectTune(AudioClip tempTune, float switchTime = 0.1f)
	{
		PlayOneShotTune(tempTune, switchTime);
	}

	private IEnumerator PlayOneShotTune(AudioClip tempTune, float switchTime = 0.1f)
	{
		if(tempTune == null)
			yield break;

		// Halt Audio
		audio.Stop();
		yield return 0;

		// Play the one-shot in its entirety
		float resumeTime = Time.time + tempTune.length + switchTime;
		_soundSource.PlayOneShot(tempTune, _audioManager.EffectVolume);
		yield return new WaitForSeconds(resumeTime);

		// Restart the old tune.
		audio.Play();
	}

	#endregion Methods
}
