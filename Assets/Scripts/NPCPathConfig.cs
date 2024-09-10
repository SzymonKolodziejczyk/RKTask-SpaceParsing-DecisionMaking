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

    [Range(0, 100)] // Thinking percentage chance (0-100%)
    public float thinkingChance = 30f; // Chance to think, in percentage

    [Range(0, 100)] // Rotating percentage chance (0-100%)
    public float rotatingChance = 30f; // Chance to rotate, in percentage

    public float movingActionChance = 10f;  // Chance to perform random actions while moving

    [System.Serializable]
    public class Path
    {
        public int startNodeIndex;
        public int endNodeIndex;
    }
}
