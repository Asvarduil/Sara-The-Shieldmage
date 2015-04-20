using UnityEngine;
using System.Collections.Generic;

public class FloorGenerator : DebuggableBehavior
{
    #region Variables / Properties

    public GameObject FloorTile;
    public Vector2 FloorDimensions;

    private Vector3 _offset;
    private static List<GameObject> _generatedTiles;

    #endregion Variables / Properties

    #region Hooks

    [ContextMenu("Delete Floor")]
    public void DestroyFloor()
    {
        for(int i = 0; i < _generatedTiles.Count; i++)
        {
            GameObject tile = _generatedTiles[i];
            GameObject.DestroyImmediate(tile);
        }
    }

    [ContextMenu("Generate Floor")]
    public void GenerateFloor()
    {
        DetermineFloorOffset();
        Transform thisTransform = gameObject.transform;
        _generatedTiles = new List<GameObject>();

        for(int xOffset = 0; xOffset < FloorDimensions.x; xOffset++)
        {
            for(int yOffset = 0; yOffset < FloorDimensions.y; yOffset++)
            {
                Vector3 currentOffset = new Vector3(xOffset, 0, yOffset);
                Vector3 position = transform.position + _offset + currentOffset;
                GameObject currentTile = GameObject.Instantiate(FloorTile, position, Quaternion.identity) as GameObject;
                currentTile.transform.SetParent(thisTransform);
                currentTile.name = currentTile.name.Substring(0, currentTile.name.Length - 7);

                _generatedTiles.Add(currentTile);
            }
        }
    }

    #endregion Hooks

    #region Methods

    private void DetermineFloorOffset()
    {
        _offset = Vector3.zero;

        _offset.x -= (FloorDimensions.x % 2 > 0)
            ? (FloorDimensions.x / 2) - 1
            : FloorDimensions.x / 2;
        _offset.z -= (FloorDimensions.y % 2 > 0)
            ? (FloorDimensions.y / 2) - 1
            : FloorDimensions.y / 2;
    }

    #endregion Methods
}
