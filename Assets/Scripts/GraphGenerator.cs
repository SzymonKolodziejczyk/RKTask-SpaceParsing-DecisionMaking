using UnityEngine;
using System.Collections.Generic;

public class GraphGenerator : MonoBehaviour
{
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

    // Generate the graph based on the ScriptableObject's node positions and paths
    public List<Node> GenerateGraph(NPCPathConfig npcPathConfig)
    {
        Debug.Log("Generating graph from NPCPathConfig...");

        // Create nodes based on the positions in NPCPathConfig
        List<Node> nodes = CreateNodesFromScriptableObject(npcPathConfig.nodePositions);

        // Connect the nodes based on paths in NPCPathConfig
        ConnectNodesFromScriptableObject(nodes, npcPathConfig.paths);

        // Debugging: Log the nodes and their neighbors
        Debug.Log("Nodes and their connections:");
        for (int i = 0; i < nodes.Count; i++)
        {
            Debug.Log($"Node {i} at position {nodes[i].position} is connected to {nodes[i].neighbors.Count} neighbors:");
            foreach (var neighbor in nodes[i].neighbors)
            {
                Debug.Log($"  Neighbor at position {neighbor.position}");
            }
        }

        return nodes;
    }

    // Create nodes based on positions defined in NPCPathConfig
    private List<Node> CreateNodesFromScriptableObject(List<Vector2> nodePositions)
    {
        List<Node> nodes = new List<Node>();
        foreach (var position in nodePositions)
        {
            Node newNode = new Node(position);
            nodes.Add(newNode);
        }
        return nodes;
    }

    // Connect nodes based on paths defined in NPCPathConfig
    private void ConnectNodesFromScriptableObject(List<Node> nodes, List<NPCPathConfig.Path> paths)
    {
        foreach (var path in paths)
        {
            if (path.startNodeIndex >= 0 && path.startNodeIndex < nodes.Count &&
                path.endNodeIndex >= 0 && path.endNodeIndex < nodes.Count)
            {
                Node startNode = nodes[path.startNodeIndex];
                Node endNode = nodes[path.endNodeIndex];

                // Add connections between nodes
                if (!startNode.neighbors.Contains(endNode))
                {
                    startNode.neighbors.Add(endNode);
                }
                if (!endNode.neighbors.Contains(startNode))
                {
                    endNode.neighbors.Add(startNode);
                }

                Debug.Log($"Connected Node {path.startNodeIndex} to Node {path.endNodeIndex}");
            }
            else
            {
                Debug.LogError($"Invalid node indices: startNodeIndex {path.startNodeIndex}, endNodeIndex {path.endNodeIndex}");
            }
        }
    }
}
