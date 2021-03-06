﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ConversationNPCEvents : ConversationEventBase
{
    #region Variables / Properties

    private List<GameObject> _npcPool;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _npcPool = new List<GameObject>();
        base.Start();
    }

    protected override void RegisterEventHooks()
    {
        _controller.RegisterEventHook("SpawnNPC", SpawnNPC);
        _controller.RegisterEventHook("DespawnNPC", DespawnNPC);
        _controller.RegisterEventHook("DespawnAllNPCs", DespawnAllNPCs);
        _controller.RegisterEventHook("ForceNPCAnimation", ForceNPCAnimation);
    }

    #endregion Hooks

    #region Methods

    public IEnumerator SpawnNPC(List<string> args)
    {
        DebugMessage("Spawning a new NPC...");

        if (args == null
           || args.Count == 0)
            throw new ArgumentException("Spawn NPC requires the path of the NPC prefab, the coordinates to spawn at, and the NPC's scene name!");

        string prefabName = args[0];
        string coords = args[1];
        string npcName = args[2];

        DebugMessage("Given path: " + prefabName);

        GameObject npc = Resources.Load<GameObject>(prefabName);
        if (npc == null)
            throw new ArgumentException("Path " + prefabName + " is incorrect.");

        string[] coordComponents = coords.Split(',');
        Vector3 npcCoordinates = new Vector3();
        npcCoordinates.x = Convert.ToSingle(coordComponents[0].Trim());
        npcCoordinates.y = Convert.ToSingle(coordComponents[1].Trim());
        npcCoordinates.z = Convert.ToSingle(coordComponents[2].Trim());

        GameObject newNPC = (GameObject) GameObject.Instantiate(npc, npcCoordinates, Quaternion.identity);
        newNPC.name = npcName;
        _npcPool.Add(newNPC);
        DebugMessage("Spawned NPC " + newNPC.name + " at " + newNPC.transform.position);

        yield return null;
    }

    public IEnumerator DespawnNPC(List<string> args)
    {
        if (args == null
           || args.Count == 0
           || args.Count > 1)
            throw new ArgumentException("DespawnNPC requires one argument!");

        string name = args[0];
        for(int i = 0; i < _npcPool.Count; i++)
        {
            GameObject npc = _npcPool[i];

            if (npc.name != name)
                continue;

            GameObject.Destroy(npc);
            _npcPool.Remove(npc);
            break;
        }

        yield return null;
    }
    
    public IEnumerator DespawnAllNPCs(List<string> args)
    {
        for(int i = 0; i < _npcPool.Count; i++)
        {
            GameObject npc = _npcPool[i];
            GameObject.Destroy(npc);
            _npcPool.Remove(npc);
        }

        yield return null;
    }

    public IEnumerator ForceNPCAnimation(List<string> args)
    {
        if (args == null || args.Count < 2)
            throw new ArgumentException("ForceNPCAnimation requires the name of the NPC, and the name of the animation to force.");

        string npcName = args[0];
        string animationName = args[1];

        GameObject npc = _npcPool.FirstOrDefault(n => n.name == npcName);
        if (npc == default(GameObject))
            throw new ArgumentException("NPC " + npcName + " has not been spawned yet!");

        NPCControlSystem sprite = npc.GetComponent<NPCControlSystem>();
        if(sprite == null)
        {
            DebugMessage("NPC " + npcName + " has no NPC Control System as a top-level component!");
            yield break;
        }

        DebugMessage("NPC " + npcName + " is being set in pose: " + animationName + "...");
        sprite.ActionState = (NPCControlState) Enum.Parse(typeof(NPCControlState), animationName);

        yield return null;
    }

    #endregion Methods
}
