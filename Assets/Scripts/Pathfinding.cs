using UnityEngine;
using System.Collections.Generic;

public class SimplePathfinding : MonoBehaviour
{
    public List<GraphGenerator.Node> FindPath(GraphGenerator.Node start, GraphGenerator.Node goal, List<GraphGenerator.Node> graph, NPCPathConfig npcPathConfig)
    {
        List<GraphGenerator.Node> path = new List<GraphGenerator.Node>();

        GraphGenerator.Node currentNode = start;
        path.Add(currentNode);  // Add the start node

        while (currentNode != goal)
        {
            // Find the next node that is a neighbor and hasn't been visited yet
            foreach (var pathConfig in npcPathConfig.paths)
            {
                if (pathConfig.startNodeIndex == graph.IndexOf(currentNode))
                {
                    GraphGenerator.Node nextNode = graph[pathConfig.endNodeIndex];
                    path.Add(nextNode);
                    currentNode = nextNode;
                    break;
                }
            }
        }

        if (path.Count == 0)
        {
            Debug.LogError("No valid path found in SimplePathfinding.");
        }

        return path;
    }
}