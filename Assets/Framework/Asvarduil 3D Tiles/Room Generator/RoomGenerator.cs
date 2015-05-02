using System;
using System.Collections.Generic;
using UnityEngine;

public enum RoomShapes
{
    Room,
    Cap,
    StraightLongHall,
    StraightWideHall,
    Turn,
    Intersection3,
    Intersection4,
    Floor
}

public class RoomGenerator : DebuggableBehavior
{
    #region Variables / Properties

    public GameObject WallTile;
    public GameObject FloorTile;
    public RoomShapes RoomShape;
    public Vector2 RoomDimensions;
    public int FarWallHeight = 2;
    public int SideWallHeight = 2;
    public int NearWallHeight = 1;

    public GameObject GeneratedWallTarget;
    public GameObject GeneratedFloorTarget;

    private Vector3 _offset;
    private List<GameObject> _generatedWallTiles;
    private List<GameObject> _generatedFloorTiles;

    private Vector2 FloorDimensions
    {
        get
        {
            Vector2 dimensions = new Vector2();
            switch (RoomShape)
            {
                case RoomShapes.Room:
                    dimensions.x = RoomDimensions.x - 2;
                    dimensions.y = RoomDimensions.y - 2;
                    break;

                case RoomShapes.Cap:
                    // TODO: Figure out how to make the offset work, so that dims.x can be roomdims.x - 1 instead.
                    dimensions.x = RoomDimensions.x - 2;
                    dimensions.y = RoomDimensions.y - 2;
                    break;

                case RoomShapes.StraightLongHall:
                    dimensions.x = RoomDimensions.x - 2;
                    dimensions.y = RoomDimensions.y;
                    break;

                case RoomShapes.StraightWideHall:
                    dimensions.x = RoomDimensions.x;
                    dimensions.y = RoomDimensions.y - 2;
                    break;

                case RoomShapes.Turn:
                    dimensions.x = RoomDimensions.x - 2;
                    dimensions.y = RoomDimensions.y - 2;
                    break;

                case RoomShapes.Intersection3:
                    // TODO: Figure out how to make the offset work, so that dims.x can be roomdims.x - 1 instead.
                    dimensions.x = RoomDimensions.x - 2;
                    dimensions.y = RoomDimensions.y - 2;
                    break;

                case RoomShapes.Intersection4:
                    // TODO: In this case, instruct the floor generator to not generate the corners.
                    dimensions.x = RoomDimensions.x - 2;
                    dimensions.y = RoomDimensions.y - 2;
                    break;

                case RoomShapes.Floor:
                    dimensions.x = RoomDimensions.x;
                    dimensions.y = RoomDimensions.y;
                    break;

                default:
                    throw new InvalidOperationException("Unexpected Room Shape: " + RoomShape);
            }

            return dimensions;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    [ContextMenu("Delete Room")]
    public void DeleteRoom()
    {
        if (_generatedFloorTiles.IsNullOrEmpty()
           || _generatedWallTiles.IsNullOrEmpty())
            throw new InvalidOperationException("You must first generate a room in order to delete it.");

        DestroyWalls();
        DestroyFloor();
    }

    [ContextMenu("Generate Room")]
    public void GenerateRoom()
    {
        if (GeneratedFloorTarget == null)
            throw new InvalidOperationException("To generate a room, you need to select an object on which the floor will be generated!");

        if (GeneratedWallTarget == null)
            throw new InvalidOperationException("To generate a room, youneed to select an object on which the walls will be generated!");

        GenerateWalls();
        GenerateFloor();
    }

    #endregion Hooks

    #region General Methods

    private void DetermineOffset(Vector2 dimensions)
    {
        _offset = Vector3.zero;

        _offset.x -= (dimensions.x % 2 > 0)
            ? (dimensions.x / 2) - 1
            : dimensions.x / 2;
        _offset.z -= (dimensions.y % 2 > 0)
            ? (dimensions.y / 2) - 1
            : dimensions.y / 2;
    }

    private GameObject SpawnTile(GameObject tile, Vector3 position, GameObject generateTarget)
    {
        var result = GameObject.Instantiate(tile, position, Quaternion.identity) as GameObject;
        result.transform.SetParent(generateTarget.transform);
        result.name = result.name.Substring(0, result.name.Length - 7);

        return result;
    }

    #endregion General Methods

    #region Wall Generation Methods

    private void DestroyWalls()
    {
        if (_generatedWallTiles.IsNullOrEmpty())
            return;

        for (int i = 0; i < _generatedWallTiles.Count; i++)
        {
            GameObject tile = _generatedWallTiles[i];
            _generatedWallTiles.Remove(tile);

            GameObject.DestroyImmediate(tile);
        }
    }

    private void GenerateWalls()
    {
        DetermineOffset(RoomDimensions);
        _generatedWallTiles = new List<GameObject>();

        switch (RoomShape)
        {
            case RoomShapes.Room:
                GenerateSideWalls(false);
                GenerateFarWall();
                GenerateNearWall();
                break;

            case RoomShapes.Cap:
                GenerateSideWalls(false);
                GenerateFarWall();
                break;

            case RoomShapes.StraightLongHall:
                GenerateSideWalls(true);
                break;

            case RoomShapes.StraightWideHall:
                GenerateFarWall();
                GenerateNearWall();
                break;

            case RoomShapes.Intersection3:
                GenerateFarWall();
                GenerateNearColumns();
                break;

            case RoomShapes.Intersection4:
                GenerateFarColumns();
                GenerateNearColumns();
                break;

            case RoomShapes.Floor:
                // Don't generate walls for a floor.
                break;

            default:
                throw new InvalidOperationException("Unexpected Wall Shape: " + RoomShape);
        }
    }

    private void GenerateSideWalls(bool extendToEdges)
    {
        float leftX = transform.position.x + _offset.x;
        float rightX = (transform.position.x + _offset.x) + ((RoomDimensions.y % 2 > 0) ? RoomDimensions.x - 1 : RoomDimensions.x - 2);

        float minZ = extendToEdges
            ? _offset.z
            : _offset.z + 1;
        float maxZ = extendToEdges
            ? RoomDimensions.y
            : RoomDimensions.y - 2;

        for (float currentZ = 0; currentZ < maxZ; currentZ++)
        {
            for (float currentY = 0; currentY <= SideWallHeight; currentY++)
            {
                float actualZ = transform.position.z + minZ + currentZ;
                float actualY = transform.position.y + currentY;

                Vector3 leftPosition = new Vector3(leftX, actualY, actualZ);
                Vector3 rightPosition = new Vector3(rightX, actualY, actualZ);

                GameObject leftSegment = SpawnTile(WallTile, leftPosition, GeneratedWallTarget);
                GameObject rightSegment = SpawnTile(WallTile, rightPosition, GeneratedWallTarget);

                _generatedWallTiles.Add(leftSegment);
                _generatedWallTiles.Add(rightSegment);
            }
        }
    }

    private void GenerateNearWall()
    {
        float minZ = transform.position.z + _offset.z;
        float minX = transform.position.x - _offset.x;
        float maxX = transform.position.x + _offset.x;

        for (float currentX = 0; currentX < RoomDimensions.x; currentX++)
        {
            for (float currentY = 0; currentY <= NearWallHeight; currentY++)
            {
                float actualX = transform.position.x + _offset.x + currentX;
                float actualY = transform.position.y + currentY;

                Vector3 tilePosition = new Vector3(actualX, actualY, minZ);

                GameObject wallTile = SpawnTile(WallTile, tilePosition, GeneratedWallTarget);

                _generatedWallTiles.Add(wallTile);
            }
        }
    }

    private void GenerateFarWall()
    {
        float maxZ = transform.position.z - ((RoomDimensions.y % 2 > 0) ? (_offset.z - 1) : (_offset.z + 1));
        float minX = transform.position.x - _offset.x;
        float maxX = transform.position.x + _offset.x;

        for (float currentX = 0; currentX < RoomDimensions.x; currentX++)
        {
            for (float currentY = 0; currentY <= FarWallHeight; currentY++)
            {
                float actualX = transform.position.x + _offset.x + currentX;
                float actualY = transform.position.y + currentY;

                Vector3 tilePosition = new Vector3(actualX, actualY, maxZ);

                GameObject wallTile = SpawnTile(WallTile, tilePosition, GeneratedWallTarget);

                _generatedWallTiles.Add(wallTile);
            }
        }
    }

    private void GenerateFarColumns()
    {
        float minX = transform.position.x + _offset.x;
        float maxX = transform.position.x + _offset.x + (RoomDimensions.x - 1);
        float maxZ = transform.position.z - ((RoomDimensions.y % 2 > 0) ? (_offset.z - 1) : (_offset.z + 1));

        for(float currentY = 0; currentY <= FarWallHeight; currentY++)
        {
            float actualY = transform.position.y + currentY;
            Vector3 leftColumnPosition = new Vector3(minX, actualY, maxZ);
            Vector3 rightColumnPosition = new Vector3(maxX, actualY, maxZ);

            GameObject leftColumn = SpawnTile(WallTile, leftColumnPosition, GeneratedWallTarget);
            GameObject rightColumn = SpawnTile(WallTile, rightColumnPosition, GeneratedWallTarget);

            _generatedWallTiles.Add(leftColumn);
            _generatedWallTiles.Add(rightColumn);
        }
    }

    private void GenerateNearColumns()
    {
        float minX = transform.position.x + _offset.x;
        float maxX = transform.position.x + _offset.x + (RoomDimensions.x - 1);
        float minZ = transform.position.z + _offset.z;

        for (float currentY = 0; currentY <= FarWallHeight; currentY++)
        {
            float actualY = transform.position.y + currentY;
            Vector3 leftColumnPosition = new Vector3(minX, actualY, minZ);
            Vector3 rightColumnPosition = new Vector3(maxX, actualY, minZ);

            GameObject leftColumn = SpawnTile(WallTile, leftColumnPosition, GeneratedWallTarget);
            GameObject rightColumn = SpawnTile(WallTile, rightColumnPosition, GeneratedWallTarget);

            _generatedWallTiles.Add(leftColumn);
            _generatedWallTiles.Add(rightColumn);
        }
    }

    #endregion Wall Generation Methods

    #region Floor Generation Methods

    public void DestroyFloor()
    {
        if (_generatedFloorTiles.IsNullOrEmpty())
            return;

        for (int i = 0; i < _generatedFloorTiles.Count; i++)
        {
            GameObject tile = _generatedFloorTiles[i];
            _generatedFloorTiles.Remove(tile);

            GameObject.DestroyImmediate(tile);
        }
    }

    private void GenerateFloor()
    {
        DetermineOffset(FloorDimensions);
        _generatedFloorTiles = new List<GameObject>();

        for (int xOffset = 0; xOffset < FloorDimensions.x; xOffset++)
        {
            for (int yOffset = 0; yOffset < FloorDimensions.y; yOffset++)
            {
                Vector3 currentOffset = new Vector3(xOffset, 0, yOffset);
                Vector3 position = transform.position + _offset + currentOffset;
                GameObject currentTile = GameObject.Instantiate(FloorTile, position, Quaternion.identity) as GameObject;
                currentTile.transform.SetParent(GeneratedFloorTarget.transform);
                currentTile.name = currentTile.name.Substring(0, currentTile.name.Length - 7);

                _generatedFloorTiles.Add(currentTile);
            }
        }
    }

    #endregion Floor Generation Methods
}
