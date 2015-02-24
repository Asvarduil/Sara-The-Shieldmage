using UnityEngine;
using System.Collections;

public class ExpireAfterDuration : DebuggableBehavior
{
    #region Variables / Properties

    public float Duration;

    private float _spawnTime;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _spawnTime = Time.time;
    }

    public void Update()
    {
        if (Time.time < _spawnTime + Duration)
            return;

        Destroy(gameObject);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
