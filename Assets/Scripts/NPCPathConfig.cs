using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NPCPathConfig", menuName = "Pathfinding/NPCPathConfig", order = 1)]
public class NPCPathConfig : ScriptableObject
{
    public List<Vector2> nodePositions; // Node positions for the graph
    public float npcRadius = 0.5f; // Radius for pathing
    public float moveSpeed = 2f; // Speed of the NPC

    public List<string> randomThoughts;
    public float pauseDuration = 1f;

    [System.Serializable]
    public class Path
    {
        public int startNodeIndex;
        public int endNodeIndex;
    }
}
