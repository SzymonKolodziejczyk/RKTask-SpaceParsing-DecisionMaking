using System.Collections.Generic;
using UnityEngine;

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

    public List<Node> GenerateGraph(NPCPathConfig npcPathConfig)
    {
        if (npcPathConfig == null)
        {
            Debug.LogError("NPCPathConfig is not assigned in GraphGenerator!");
            return null;
        }

        List<Node> nodes = CreateNodesFromScriptableObject(npcPathConfig.nodePositions);
        ConnectNodesFromScriptableObject(nodes, npcPathConfig.paths);
        return nodes;
    }

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

    private void ConnectNodesFromScriptableObject(List<Node> nodes, List<NPCPathConfig.Path> paths)
    {
        Debug.Log($"Total nodes available: {nodes.Count}");

        foreach (var path in paths)
        {
            if (path.startNodeIndex >= 0 && path.startNodeIndex < nodes.Count &&
                path.endNodeIndex >= 0 && path.endNodeIndex < nodes.Count)
            {
                Node startNode = nodes[path.startNodeIndex];
                Node endNode = nodes[path.endNodeIndex];
                startNode.neighbors.Add(endNode);
                endNode.neighbors.Add(startNode);
                
                Debug.Log($"Connected Node {path.startNodeIndex} to Node {path.endNodeIndex}");
            }
            else
            {
                Debug.LogError($"Invalid node indices: startNodeIndex {path.startNodeIndex}, endNodeIndex {path.endNodeIndex}");
            }
        }
    }
}