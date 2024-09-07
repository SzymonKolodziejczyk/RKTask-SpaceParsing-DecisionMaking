using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    // Define the Node class
    class Node
    {
        public Vector2 position; // Position of the node (corner of a cube)
        public List<Node> neighbors; // Neighbors connected by edges

        public Node(Vector2 pos)
        {
            position = pos;
            neighbors = new List<Node>();
        }
    }

    // Entry point to generate the graph when the scene starts
    void Start()
    {
        // Define some cubes in the scene (as an example)
        Vector2[][] cubes = new Vector2[][]
        {
            new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) },
            new Vector2[] { new Vector2(2, 2), new Vector2(3, 2), new Vector2(3, 3), new Vector2(2, 3) }
        };

        // Generate the graph from the cubes
        List<Node> graph = GenerateGraph(cubes);

        // Debugging: Print out the graph connections
        foreach (var node in graph)
        {
            Debug.Log($"Node at {node.position} has {node.neighbors.Count} neighbors.");
            foreach (var neighbor in node.neighbors)
            {
                Debug.Log($"--> Connected to neighbor at {neighbor.position}");
                Debug.DrawLine(node.position, neighbor.position, Color.green, 100.0f); // Draw connections in the scene
            }
        }
    } 

    // Function to generate the graph from cube corners
    List<Node> GenerateGraph(Vector2[][] cubes)
    {
        // Step 1: Create nodes from cube corners
        List<Node> nodes = CreateNodesFromCubes(cubes);

        // Step 2: Connect the nodes with edges, avoiding obstacles
        ConnectNodes(nodes, cubes);

        return nodes;
    }

    // Create nodes from cube corners
    List<Node> CreateNodesFromCubes(Vector2[][] cubes)
    {
        List<Node> nodes = new List<Node>();

        foreach (var cube in cubes)
        {
            foreach (var corner in cube)
            {
                Node newNode = new Node(corner);
                nodes.Add(newNode);
            }
        }

        return nodes;
    }

    // Connect nodes by checking if the path between them is clear
    void ConnectNodes(List<Node> nodes, Vector2[][] cubes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                bool pathClear = true;

                // Check if the path between nodes[i] and nodes[j] is blocked by any cube
                foreach (var cube in cubes)
                {
                    for (int k = 0; k < cube.Length; k++)
                    {
                        Vector2 p1 = cube[k];
                        Vector2 p2 = cube[(k + 1) % cube.Length];

                        if (DoLinesIntersect(nodes[i].position, nodes[j].position, p1, p2))
                        {
                            pathClear = false;
                            break;
                        }
                    }
                    if (!pathClear) break;
                }

                // If the path is clear, connect the nodes
                if (pathClear)
                {
                    nodes[i].neighbors.Add(nodes[j]);
                    nodes[j].neighbors.Add(nodes[i]);
                }
            }
        }
    }

    // Check if two lines intersect (to detect if paths cross obstacle edges)
    bool DoLinesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        float determinant = (a2.x - a1.x) * (b2.y - b1.y) - (a2.y - a1.y) * (b2.x - b1.x);
        if (determinant == 0) return false; // Lines are parallel

        float t = ((a1.x - b1.x) * (b2.y - b1.y) - (a1.y - b1.y) * (b2.x - b1.x)) / determinant;
        float u = -((a2.x - a1.x) * (a1.y - b1.y) - (a2.y - a1.y) * (a1.x - b1.x)) / determinant;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }
}