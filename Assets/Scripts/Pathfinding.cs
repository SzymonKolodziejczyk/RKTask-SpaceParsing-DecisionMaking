using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<GraphGenerator.Node> FindPath(GraphGenerator.Node start, GraphGenerator.Node goal, List<GraphGenerator.Node> graph)
    {
        // Check if start or goal nodes are blocked
        if (IsNodeBlocked(start))
        {
            Debug.Log("Start node is blocked.");
            return null;
        }

        if (IsNodeBlocked(goal))
        {
            Debug.Log("Goal node is blocked.");
            return null;
        }

        // Check if the graph is connected
        if (!IsGraphConnected(graph))
        {
            Debug.Log("Graph is not fully connected, some areas are unreachable.");
            return null;
        }

        // Proceed with pathfinding
        return AStarPathfinding(start, goal);
    }

    private bool IsNodeBlocked(GraphGenerator.Node node)
    {
        return node.neighbors.Count == 0;
    }

    private bool IsGraphConnected(List<GraphGenerator.Node> graph)
    {
        HashSet<GraphGenerator.Node> visited = new HashSet<GraphGenerator.Node>();
        Queue<GraphGenerator.Node> queue = new Queue<GraphGenerator.Node>();

        queue.Enqueue(graph[0]);
        visited.Add(graph[0]);

        while (queue.Count > 0)
        {
            GraphGenerator.Node current = queue.Dequeue();

            foreach (GraphGenerator.Node neighbor in current.neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        // If visited all nodes, the graph is connected
        return visited.Count == graph.Count;
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
