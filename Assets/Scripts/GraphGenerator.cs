using UnityEngine;
using System.Collections.Generic;

public class GraphGenerator : MonoBehaviour
{
    public GameObject nodeMarkerDefault; // Assign in the inspector

    public class Node
    {
        public Vector2 position;
        public List<Node> neighbors;
        public GameObject markerInstance; // Reference to the marker instance

        public Node(Vector2 pos)
        {
            position = pos;
            neighbors = new List<Node>();
        }
    }

    public List<Node> GenerateGraph(NPCPathConfig npcPathConfig)
    {
        Debug.Log("Generating graph from NPCPathConfig...");

        List<Node> nodes = CreateNodesFromScriptableObject(npcPathConfig.nodePositions);

        ConnectNodesInSequence(nodes); // Connect nodes in sequence

        // Instantiate a marker at each node position
        foreach (var node in nodes)
        {
            InstantiateNodeMarker(node.position);
        }

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

    // Connect nodes in sequence based on their positions in the list
    private void ConnectNodesInSequence(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            nodes[i].neighbors.Add(nodes[i + 1]);  // Connect to the next node
            nodes[i + 1].neighbors.Add(nodes[i]);  // Connect back to the previous node (if bi-directional)
        }
    }

    // Instantiate a marker at each node position
    private void InstantiateNodeMarker(Vector2 position)
    {
        if (nodeMarkerDefault != null)
        {
            Instantiate(nodeMarkerDefault, new Vector3(position.x, position.y, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Node marker prefab is not assigned.");
        }
    }
}