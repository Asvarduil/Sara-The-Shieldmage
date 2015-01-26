using UnityEngine;
using System.Collections;

public abstract class PuzzleSolvedObjectBase : MonoBehaviour
{
    #region Hooks

    public abstract void OnPuzzleSolved();
    public abstract void OnPuzzleAlreadySolved();

    #endregion Hooks
}
