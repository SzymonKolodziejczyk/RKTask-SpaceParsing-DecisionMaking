using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public List<GraphGenerator.Node> FindPath(GraphGenerator.Node start, GraphGenerator.Node goal, List<GraphGenerator.Node> graph, NPCPathConfig npcPathConfig)
    {
        List<GraphGenerator.Node> path = new List<GraphGenerator.Node>();
        HashSet<GraphGenerator.Node> visited = new HashSet<GraphGenerator.Node>(); // Keep track of visited nodes

        GraphGenerator.Node currentNode = start;
        path.Add(currentNode);  // Add the start node
        visited.Add(currentNode);

        int maxIterations = graph.Count * 2; // Safety limit to prevent infinite loops
        int iterations = 0;

        while (currentNode != goal && iterations < maxIterations)
        {
            bool pathFound = false;

            // Check all neighbors to find the next node
            foreach (var neighbor in currentNode.neighbors)
            {
                if (!visited.Contains(neighbor) && !IsPathBlocked(currentNode, neighbor)) // Avoid walls and revisiting
                {
                    path.Add(neighbor);
                    currentNode = neighbor;
                    visited.Add(currentNode);
                    pathFound = true;
                    break; // Move to the next node
                }
            }

            if (!pathFound)
            {
                Debug.LogError("No valid path found to continue from current node.");
                return null; // Exit if no path can be found
            }

            iterations++;
        }

        if (iterations >= maxIterations)
        {
            Debug.LogError("Pathfinding timed out. Likely due to too many iterations or an infinite loop.");
            return null;
        }

        return path;
    }

    // Function to check if there's a wall between two nodes
    private bool IsPathBlocked(GraphGenerator.Node start, GraphGenerator.Node end)
    {
        Vector3 direction = (end.position - start.position).normalized;
        float distance = Vector3.Distance(start.position, end.position);

        // Debug the raycast parameters
        Debug.Log($"Casting 3D ray from {start.position} to {end.position} with distance {distance}");

        // Perform a 3D raycast to check for walls
        Ray ray = new Ray(start.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, LayerMask.GetMask("Wall")))
        {
            Debug.Log($"Raycast hit: {hit.collider.name} on layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            return true; // Path is blocked
        }
        else
        {
            Debug.Log($"No obstacle detected between {start.position} and {end.position}");
        }

        return false; // Path is clear
    }
}