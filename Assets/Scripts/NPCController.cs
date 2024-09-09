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

            // Validate that combined chances don't exceed 100
            float totalChance = npcPathConfig.thinkingChance + npcPathConfig.rotatingChance;

            if (totalChance > 100f)
            {
                Debug.LogError("Total of thinkingChance and rotatingChance exceeds 100%! Please adjust the values.");
                return;
            }

            // Generate a random number between 0 and 100
            float randomValue = Random.Range(0f, 100f);

            // Decision logic based on percentage chances
            if (randomValue < npcPathConfig.thinkingChance) // Think
            {
                StartCoroutine(PauseAndThink());
            }
            else if (randomValue < npcPathConfig.thinkingChance + npcPathConfig.rotatingChance) // Rotate
            {
                StartCoroutine(LookAround());
            }
            else // Do nothing, go to the next node
            {
                Debug.Log("NPC does nothing and continues to the next node.");
                isPaused = false; // Continue to the next node
            }

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

    private IEnumerator LookAround()
    {
        isPaused = true;

        // Rotate by 90 degrees to the left
        yield return StartCoroutine(Rotate(90));
        yield return new WaitForSeconds(0.5f); // Short pause between rotations

        // Rotate back to the original position (180 degrees total to complete the "look around")
        yield return StartCoroutine(Rotate(-180));
        yield return new WaitForSeconds(0.5f);

        // Rotate back to the original direction
        yield return StartCoroutine(Rotate(90));

        Debug.Log("NPC finished looking around.");
        isPaused = false;
    }

    // Coroutine to smoothly rotate the NPC over time
    private IEnumerator Rotate(float angle)
    {
        float duration = 1f; // Time to complete the rotation
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, angle); // Rotate around Z-axis in 2D

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation; // Ensure we end at the exact final rotation
    }
}