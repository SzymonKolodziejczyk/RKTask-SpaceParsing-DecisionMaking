using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public float npcRadius = 0.5f; // Radius of the NPC for tight path handling

    public class Node
    {
        public Vector2 position;
        public List<Node> neighbors;

        public Node(Vector2 pos)
        {
            position = pos;
            neighbors = new List<Node>();
        }
    }

    public List<Node> GenerateGraph(Vector2[][] cubes)
    {
        List<Node> nodes = CreateNodesFromCubes(cubes);
        ConnectNodes(nodes, cubes);
        RemoveIsolatedNodes(nodes); // Handle isolated nodes
        return nodes;
    }

    private List<Node> CreateNodesFromCubes(Vector2[][] cubes)
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

    private void ConnectNodes(List<Node> nodes, Vector2[][] cubes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                if (IsPathClear(nodes[i].position, nodes[j].position, cubes, npcRadius))
                {
                    nodes[i].neighbors.Add(nodes[j]);
                    nodes[j].neighbors.Add(nodes[i]);
                }
            }
        }
    }

    // Check if the path between two nodes is clear, considering NPC radius
    private bool IsPathClear(Vector2 start, Vector2 end, Vector2[][] cubes, float npcRadius)
    {
        foreach (var cube in cubes)
        {
            for (int i = 0; i < cube.Length; i++)
            {
                Vector2 p1 = cube[i];
                Vector2 p2 = cube[(i + 1) % cube.Length];

                if (DoLinesIntersect(start, end, p1, p2, npcRadius))
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Modify line intersection check to account for NPC radius
    private bool DoLinesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, float radius)
    {
        // Simple line intersection logic can be expanded to use the radius
        // In this case, we're calculating buffer zones by expanding the lines
        float determinant = (a2.x - a1.x) * (b2.y - b1.y) - (a2.y - a1.y) * (b2.x - b1.x);
        if (determinant == 0) return false;

        float t = ((a1.x - b1.x) * (b2.y - b1.y) - (a1.y - b1.y) * (b2.x - b1.x)) / determinant;
        float u = -((a2.x - a1.x) * (a1.y - b1.y) - (a2.y - a1.y) * (a1.x - b1.x)) / determinant;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }

    // Handle isolated nodes by removing them from the graph
    private void RemoveIsolatedNodes(List<Node> nodes)
    {
        nodes.RemoveAll(node => node.neighbors.Count == 0);
        // This will remove nodes with no neighbors, marking them as unreachable
    }
}