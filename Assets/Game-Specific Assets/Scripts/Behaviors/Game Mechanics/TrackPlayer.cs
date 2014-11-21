using UnityEngine;
using System.Collections;

public class TrackPlayer : DebuggableBehavior 
{
	#region Variables / Properties

	private GameObject _player;

	#endregion Variables / Properties

	#region Hooks

	public void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	public void Update()
	{
		transform.position = _player.transform.position;
	}

	#endregion Hooks

	#region Methods

	#endregion Methods
}
