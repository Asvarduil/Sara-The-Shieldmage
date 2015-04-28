using System;
using System.Collections.Generic;
using UnityEngine;

public enum WallShapes
{
    Room,
    StraightLongHall,
    StraightWideHall
}

public class WallGenerator : DebuggableBehavior
{
    #region Variables / Properties

    public GameObject WallTile;
    public WallShapes WallShape;
    public Vector2 WallDimensions;
    public int FarWallHeight = 2;
    public int SideWallHeight = 2;
    public int NearWallHeight = 1;

    private Vector3 _offset;
    private Transform _thisTransform = null;
    private static List<GameObject> _generatedTiles;

    #endregion Variables / Properties

    #region Hooks

    [ContextMenu("Delete Walls")]
    public void DeleteWalls()
    {
        for (int i = 0; i < _generatedTiles.Count; i++)
        {
            GameObject tile = _generatedTiles[i];
            GameObject.DestroyImmediate(tile);
        }
    }

    [ContextMenu("Generate Walls")]
    public void GenerateWalls()
    {
        DetermineOffset();
        _thisTransform = transform;
        _generatedTiles = new List<GameObject>();

        switch(WallShape)
        {
            case WallShapes.Room:
                GenerateSideWalls(false);
                GenerateFarWall();
                GenerateNearWall();
                break;

            case WallShapes.StraightLongHall:
                GenerateSideWalls(true);
                break;

            case WallShapes.StraightWideHall:
                GenerateFarWall();
                GenerateNearWall();
                break;

            default:
                throw new InvalidOperationException("Unexpected Wall Shape: " + WallShape);
        }
    }

    #endregion Hooks

    #region Methods

    private void DetermineOffset()
    {
        _offset = Vector3.zero;

        _offset.x -= (WallDimensions.x % 2 > 0)
            ? (WallDimensions.x / 2) - 1
            : WallDimensions.x / 2;
        _offset.z -= (WallDimensions.y % 2 > 0)
            ? (WallDimensions.y / 2) - 1
            : WallDimensions.y / 2;
    }

    private void GenerateSideWalls(bool extendToEdges)
    {
        float leftX = transform.position.x + _offset.x;
        float rightX = (transform.position.x + _offset.x) + ((WallDimensions.y % 2 > 0) ? WallDimensions.x - 1 : WallDimensions.x - 2);

        float minZ = extendToEdges 
            ? _offset.z 
            : _offset.z + 1;
        float maxZ = extendToEdges 
            ? WallDimensions.y 
            : WallDimensions.y - 2;

        for(float currentZ = 0; currentZ < maxZ; currentZ++)
        {
            for(float currentY = 0; currentY <= SideWallHeight; currentY++)
            {
                float actualZ = transform.position.z + minZ + currentZ;
                float actualY = transform.position.y + currentY;

                Vector3 leftPosition = new Vector3(leftX, actualY, actualZ);
                Vector3 rightPosition = new Vector3(rightX, actualY, actualZ);

                GameObject leftSegment = GameObject.Instantiate(WallTile, leftPosition, Quaternion.identity) as GameObject;
                leftSegment.transform.SetParent(_thisTransform);
                leftSegment.name = leftSegment.name.Substring(0, leftSegment.name.Length - 7);

                GameObject rightSegment = GameObject.Instantiate(WallTile, rightPosition, Quaternion.identity) as GameObject;
                rightSegment.transform.SetParent(_thisTransform);
                rightSegment.name = rightSegment.name.Substring(0, rightSegment.name.Length - 7);

                _generatedTiles.Add(leftSegment);
                _generatedTiles.Add(rightSegment);
            }
        }
    }

    private void GenerateNearWall()
    {
        float minZ = transform.position.z + _offset.z;
        float minX = transform.position.x - _offset.x;
        float maxX = transform.position.x + _offset.x;

        for (float currentX = 0; currentX < WallDimensions.x; currentX++)
        {
            for(float currentY = 0; currentY <= NearWallHeight; currentY++)
            {
                float actualX = transform.position.x + _offset.x + currentX;
                float actualY = transform.position.y + currentY;

                Vector3 tilePosition = new Vector3(actualX, actualY, minZ);

                GameObject wallTile = GameObject.Instantiate(WallTile, tilePosition, Quaternion.identity) as GameObject;
                wallTile.transform.SetParent(_thisTransform);
                wallTile.name = wallTile.name.Substring(0, wallTile.name.Length - 7);

                _generatedTiles.Add(wallTile);
            }
        }
    }

    private void GenerateFarWall()
    {
        float maxZ = transform.position.z - ((WallDimensions.y % 2 > 0) ? (_offset.z - 1) : (_offset.z + 1));
        float minX = transform.position.x - _offset.x;
        float maxX = transform.position.x + _offset.x;

        for (float currentX = 0; currentX < WallDimensions.x; currentX++)
        {
            for (float currentY = 0; currentY <= FarWallHeight; currentY++)
            {
                float actualX = transform.position.x + _offset.x + currentX;
                float actualY = transform.position.y + currentY;

                Vector3 tilePosition = new Vector3(actualX, actualY, maxZ);

                GameObject wallTile = GameObject.Instantiate(WallTile, tilePosition, Quaternion.identity) as GameObject;
                wallTile.transform.SetParent(_thisTransform);
                wallTile.name = wallTile.name.Substring(0, wallTile.name.Length - 7);

                _generatedTiles.Add(wallTile);
            }
        }
    }

    #endregion Methods
}
