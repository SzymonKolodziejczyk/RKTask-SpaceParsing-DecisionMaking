using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    private List<GraphGenerator.Node> currentPath;
    private int currentPathIndex = 0;
    private bool isMoving = false;
    private float moveSpeed; // We'll now set this via MasterController

    public void SetPath(List<GraphGenerator.Node> path, float speed)
    {
        currentPath = path;
        currentPathIndex = 0;
        moveSpeed = speed; // Speed set by MasterController
        isMoving = true;
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
        Vector2 targetPosition = currentPath[currentPathIndex].position;
        Vector2 currentPosition = transform.position;
        float step = moveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPathIndex++;
            if (currentPathIndex >= currentPath.Count)
            {
                Debug.Log("NPC has reached the goal.");
                isMoving = false;
            }
        }
    }
}