using UnityEngine;
using System.Collections.Generic;

public class MasterController : MonoBehaviour
{
    private Pathfinding pathfinding;
    private GraphGenerator graphGenerator;
    private NPCController npcController;
    public NPCPathConfig npcPathConfig;  // Reference to the ScriptableObject

    void Awake()
    {
        // Retrieve components attached to the same GameObject
        pathfinding = GetComponent<Pathfinding>();
        graphGenerator = GetComponent<GraphGenerator>();
        npcController = GetComponent<NPCController>();

        // Error handling if the components are missing
        if (pathfinding == null || graphGenerator == null || npcController == null)
        {
            Debug.LogError("One or more required components are missing from the MasterController's GameObject!");
            return;
        }

        if (npcPathConfig == null)
        {
            Debug.LogError("NPCPathConfig is not assigned in MasterController!");
            return;
        }
    }

    void Start()
    {
        // Generate the graph using GraphGenerator
        List<GraphGenerator.Node> graph = graphGenerator.GenerateGraph(npcPathConfig);
        if (graph == null)
        {
            Debug.LogError("Failed to generate graph.");
            return;
        }

        // Set start and goal nodes
        GraphGenerator.Node startNode = graph[0];
        GraphGenerator.Node goalNode = graph[graph.Count - 1];

        // Debugging
        Debug.Log($"Start Node Position: {startNode.position}");
        Debug.Log($"Goal Node Position: {goalNode.position}");

        // Find the path using Pathfinding
        List<GraphGenerator.Node> path = pathfinding.FindPath(startNode, goalNode, graph);
        if (path == null || path.Count == 0)
        {
            Debug.LogError("No path found.");
            return;
        }

        // Pass the path to NPCController
        npcController.SetPath(path, npcPathConfig.moveSpeed);
    }
}