using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<GraphGenerator.Node> FindPath(GraphGenerator.Node start, GraphGenerator.Node goal, List<GraphGenerator.Node> graph)
    {
        Debug.Log($"Starting pathfinding from {start.position} to {goal.position}");

        // Perform pathfinding logic
        List<GraphGenerator.Node> path = AStarPathfinding(start, goal);
        
        if (path == null || path.Count == 0)
        {
            Debug.LogError("No path found in A*.");
            return null;
        }
        
        return path;
    }

    private List<GraphGenerator.Node> AStarPathfinding(GraphGenerator.Node start, GraphGenerator.Node goal)
    {
        HashSet<GraphGenerator.Node> closedSet = new HashSet<GraphGenerator.Node>();
        PriorityQueue<GraphGenerator.Node> openSet = new PriorityQueue<GraphGenerator.Node>();
        Dictionary<GraphGenerator.Node, GraphGenerator.Node> cameFrom = new Dictionary<GraphGenerator.Node, GraphGenerator.Node>();

        Dictionary<GraphGenerator.Node, float> gScore = new Dictionary<GraphGenerator.Node, float>();
        Dictionary<GraphGenerator.Node, float> fScore = new Dictionary<GraphGenerator.Node, float>();

        gScore[start] = 0;
        fScore[start] = Vector2.Distance(start.position, goal.position);

        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            GraphGenerator.Node current = openSet.Dequeue();

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (GraphGenerator.Node neighbor in current.neighbors)
            {
                if (closedSet.Contains(neighbor)) continue;

                float tentative_gScore = gScore[current] + Vector2.Distance(current.position, neighbor.position);

                if (!gScore.ContainsKey(neighbor) || tentative_gScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = gScore[neighbor] + Vector2.Distance(neighbor.position, goal.position);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }
        }

        return null; // Path not found
    }

    private List<GraphGenerator.Node> ReconstructPath(Dictionary<GraphGenerator.Node, GraphGenerator.Node> cameFrom, GraphGenerator.Node current)
    {
        List<GraphGenerator.Node> path = new List<GraphGenerator.Node> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}