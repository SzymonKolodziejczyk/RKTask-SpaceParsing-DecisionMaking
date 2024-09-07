using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    private List<GraphGenerator.Node> currentPath;  // The path of nodes to follow
    private int currentPathIndex = 0;               // Track which node we're heading to
    private bool isMoving = false;                  // Track whether the NPC is moving
    private float moveSpeed;                        // The NPC's movement speed

    public void SetPath(List<GraphGenerator.Node> path, float speed)
    {
        currentPath = path;
        currentPathIndex = 0;
        moveSpeed = speed;
        isMoving = true;

        Debug.Log("NPC path set with the following nodes:");
        for (int i = 0; i < currentPath.Count; i++)
        {
            Debug.Log($"Node {i}: {currentPath[i].position}");
        }
    }

    void Update()
    {
        if (isMoving && currentPath != null && currentPath.Count > 0)
        {
            MoveAlongPath();  // Continuously move along the path
        }
    }

    private void MoveAlongPath()
    {
        if (currentPathIndex >= currentPath.Count)
        {
            Debug.Log("NPC has reached the goal.");
            isMoving = false;
            return;
        }

        Vector2 targetPosition = currentPath[currentPathIndex].position;
        Vector2 currentPosition = transform.position;

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)  // Adjust threshold
        {
            Debug.Log($"NPC reached node {currentPathIndex} at position {targetPosition}");
            currentPathIndex++;

            if (currentPathIndex >= currentPath.Count)
            {
                currentPathIndex = 0; // Loop back to the start
                isMoving = true; // Start moving again
            }
        }
    }
}
