using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    private List<GraphGenerator.Node> currentPath;
    private int currentPathIndex = 0;
    private bool isMoving = false;
    private float moveSpeed;
    private float pauseDuration;

    private bool isPaused = false;
    private NPCPathConfig npcPathConfig;

    public void SetPath(List<GraphGenerator.Node> path, float speed, NPCPathConfig config)
    {
        currentPath = path;
        currentPathIndex = 0;
        moveSpeed = speed;
        npcPathConfig = config;
        pauseDuration = npcPathConfig.pauseDuration;
        isMoving = true;

        Debug.Log("NPC path set with the following nodes:");
        for (int i = 0; i < currentPath.Count; i++)
        {
            Debug.Log($"Node {i}: {currentPath[i].position}");
        }
    }

    void Update()
    {
        if (isMoving && currentPath != null && currentPath.Count > 0 && !isPaused)
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
            
            // Pause and perform a random action
            StartCoroutine(PauseAndThink());

            currentPathIndex++;
        }
    }

    private IEnumerator PauseAndThink()
    {
        isPaused = true;

        // Select a random thought from the list in the NPCPathConfig
        string randomThought = npcPathConfig.randomThoughts[Random.Range(0, npcPathConfig.randomThoughts.Count)];
        Debug.Log($"NPC is paused: {randomThought}");

        // Pause for the duration set in the NPCPathConfig
        yield return new WaitForSeconds(pauseDuration);

        Debug.Log("NPC finished thinking.");
        isPaused = false;
    }
}