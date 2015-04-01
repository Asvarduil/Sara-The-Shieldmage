using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class SceneObjectSet
{
    public string SourceScene;
    public List<PathPositionRotationSet> MapObjects;
}

[Serializable]
public class PathPositionRotationSet
{
    public string ObjectPath;
    public Vector3 Position;
    public Quaternion Rotation;

    new public string ToString()
    {
        return ObjectPath + "/" + Position + "/" + Rotation;
    }

    public void Realize()
    {
        var currentObject = Resources.Load(ObjectPath);
        if (currentObject == null)
            throw new InvalidOperationException(string.Format("Could not find a resource at path: {0}", ObjectPath));

        GameObject.Instantiate(currentObject, Position, Rotation);
    }
}

public class MapObjectPlacer : JsonBlobLoaderBase
{
    #region Variables / Properties

    public List<SceneObjectSet> ScenePhases;

    private TransitionManager _transition;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _transition = TransitionManager.Instance;

        LoadObjectsFromJson();
        SetupScenePhase();
    }

    #endregion Hooks

    #region Methods

    [ContextMenu("Sync Local Object to JSON data")]
    public void LoadObjectsFromJson()
    {
        if (TryDownloadingBlob)
            StartCoroutine(DownloadBlob());

        // First: Is the downloaded blob empty?  If so, use a local data file.
        if (string.IsNullOrEmpty(RawBlob))
        {
            RawBlob = FetchLocalBlob();

            // Second: Is the local data file empty?  If so, don't go any further,
            //         because this is a waste of time.
            if (string.IsNullOrEmpty(RawBlob))
                return;   
        }

        MapBlob(RawBlob);
    }

    private void MapBlob(string blob)
    {
        ScenePhases = new List<SceneObjectSet>();

        var parsed = JSON.Parse(blob);
        var phases = parsed["Phases"].AsArray;

        foreach (var phase in phases.Childs)
        {
            SceneObjectSet currentPhase = new SceneObjectSet();
            currentPhase.SourceScene = phase["SourceScene"];
            currentPhase.MapObjects = new List<PathPositionRotationSet>();

            var mapObjects = phase["MapObjects"].AsArray;
            foreach (var current in mapObjects.Childs)
            {
                PathPositionRotationSet currentObject = new PathPositionRotationSet();

                var objectPath = current["Object"];
                var rawPosition = current["Position"];
                var rawRotation = current["Rotation"];

                Vector3 position = new Vector3
                (
                    rawPosition["x"].AsFloat,
                    rawPosition["y"].AsFloat,
                    rawPosition["z"].AsFloat
                );

                Quaternion rotation = Quaternion.Euler
                (
                    rawRotation["x"].AsFloat,
                    rawRotation["y"].AsFloat,
                    rawRotation["z"].AsFloat
                );

                currentObject.ObjectPath = current["Object"];
                currentObject.Position = position;
                currentObject.Rotation = rotation;

                currentPhase.MapObjects.Add(currentObject);
            }

            ScenePhases.Add(currentPhase);
        }
    }

    private void SetupScenePhase()
    {
        string sourceScene = _transition.OriginalState.SceneName;
        SceneObjectSet phase = ScenePhases.FirstOrDefault(p => p.SourceScene == sourceScene);
        if (phase == default(SceneObjectSet))
            throw new InvalidOperationException(string.Format("No scene phase data found for {0}", sourceScene));

        DebugMessage("Processing scene phase: " + phase.SourceScene);

        foreach(PathPositionRotationSet currentThing in phase.MapObjects)
        {
            DebugMessage("Processing scene phase object: " + currentThing.ToString());
            currentThing.Realize();   
        }
    }

    #endregion Methods
}
