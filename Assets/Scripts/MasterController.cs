using UnityEngine;
using System.Collections.Generic;

public class MasterController : MonoBehaviour
{
    private Pathfinding pathfinding;
    private GraphGenerator graphGenerator;
    private NPCController npcController;
    public NPCPathConfig npcPathConfig;

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        graphGenerator = GetComponent<GraphGenerator>();
        npcController = GetComponent<NPCController>();

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

        if (graph == null || graph.Count == 0)
        {
            Debug.LogError("Graph is empty or failed to generate.");
            return;
        }

        // Set start and goal nodes
        GraphGenerator.Node startNode = graph[0];
        GraphGenerator.Node goalNode = graph[graph.Count - 1];

        // Find the path using Pathfinding
        List<GraphGenerator.Node> path = pathfinding.FindPath(startNode, goalNode, graph, npcPathConfig);
        if (path == null || path.Count == 0)
        {
            Debug.LogError("No path found.");
            return;
        }

        // Pass the path to NPCController to handle NPC movement
        npcController.SetPath(path, npcPathConfig.moveSpeed);
    }
}