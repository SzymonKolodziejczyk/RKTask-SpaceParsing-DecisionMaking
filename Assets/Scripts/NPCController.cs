using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    public GraphGenerator graphGenerator;
    public Pathfinding pathfinding;

    public float moveSpeed = 2f;
    
    private GraphGenerator.Node startNode;
    private GraphGenerator.Node goalNode;
    private List<GraphGenerator.Node> currentPath;
    private int currentPathIndex = 0;  // Track current target node

    private bool isMoving = false;

    void Start()
    {
        Vector2[][] cubes = new Vector2[][]
    {
        new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) }
    };

        // Generate graph and find path
        List<GraphGenerator.Node> graph = graphGenerator.GenerateGraph(cubes);
        startNode = graph[0];
        goalNode = graph[graph.Count - 1];

        currentPath = pathfinding.FindPath(startNode, goalNode, graph);
        currentPathIndex = 0;

        if (currentPath != null && currentPath.Count > 0)
        {
            Debug.Log("Path found!");
            isMoving = true;
        }
        else
        {
            Debug.Log("No path found.");
        }
    }

    void Update()
    {
        if (isMoving && currentPath != null && currentPath.Count > 0)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        // Get the current target position (next node on the path)
        Vector2 targetPosition = currentPath[currentPathIndex].position;

        // Move the NPC towards the target position
        Vector2 currentPosition = transform.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;
        float step = moveSpeed * Time.deltaTime;

        // Move NPC
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        // Check if the NPC has reached the target position
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Move to the next node in the path
            currentPathIndex++;

            // If reached the end of the path
            if (currentPathIndex >= currentPath.Count)
            {
                Debug.Log("NPC has reached the goal.");
                isMoving = false;
            }
        }
    }
}