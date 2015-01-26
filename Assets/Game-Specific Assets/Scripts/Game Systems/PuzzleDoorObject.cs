using UnityEngine;
using System.Collections;

public class PuzzleDoorObject : PuzzleSolvedObjectBase
{
    #region Variables / Properties

    public Vector3 TargetPosition;
    public AudioClip DoorOpenSound;

    private Maestro _maestro;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _maestro = Maestro.Instance;
    }

    public override void OnPuzzleSolved()
    {
        _maestro.PlayOneShot(DoorOpenSound);
        StartCoroutine(DoorOpenSequence());
    }

    public override void OnPuzzleAlreadySolved()
    {
        transform.position = TargetPosition;
    }

    private IEnumerator DoorOpenSequence()
    {
        while (Vector3.Distance(transform.position, TargetPosition) > 0.01f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, TargetPosition, 0.1f);
            transform.position = newPosition;

            yield return 0;
        }
    }

    #endregion Hooks
}
