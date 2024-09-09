using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    private List<GraphGenerator.Node> currentPath;
    private int currentPathIndex = 0;
    private bool isMoving = false;
    private float moveSpeed;

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
            MoveAlongPath();
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

        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            Debug.Log($"NPC reached node {currentPathIndex} at position {targetPosition}");
            currentPathIndex++;

            if (currentPathIndex >= currentPath.Count)
            {
                currentPathIndex = 0; // Loop back to the start
                isMoving = true;
            }
        }
    }
}