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

    // Reference to VisionCone
    public VisionCone visionCone;

    void Start()
    {
        // Reset the NPC's rotation to eliminate complications with 0, 90, 90 setup
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

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

        // Update vision cone (if needed)
        if (visionCone != null)
        {
            visionCone.UpdateVisionCone();
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
            //Change it if you want to end the travel when the last node was reached

            /*Debug.Log("NPC has reached the goal.");
            isMoving = false;
            return;*/

            Debug.Log("Looping back to the first node.");
            currentPathIndex = 0;
        }

        Vector2 targetPosition = currentPath[currentPathIndex].position;
        Vector2 currentPosition = transform.position;

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        // Rotate towards the direction of movement
        RotateTowards(targetPosition);

        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            Debug.Log($"NPC reached node {currentPathIndex} at position {targetPosition}");

            float totalChance = npcPathConfig.thinkingChance + npcPathConfig.rotatingChance;

            if (totalChance > 100f)
            {
                Debug.LogError("Total of thinkingChance and rotatingChance exceeds 100%! Please adjust the values.");
                return;
            }

            float randomValue = Random.Range(0f, 100f);

            if (randomValue < npcPathConfig.thinkingChance) 
            {
                StartCoroutine(PauseAndThink());
            }
            else if (randomValue < npcPathConfig.thinkingChance + npcPathConfig.rotatingChance) 
            {
                StartCoroutine(LookAround());
            }
            else 
            {
                Debug.Log("NPC does nothing and continues to the next node.");
                isPaused = false;
            }

            currentPathIndex++;
        }
    }

    // Rotate the NPC and VisionCone to face the direction it's moving towards
    private void RotateTowards(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly rotate the NPC towards the target direction
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle-90);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Ensure the VisionCone follows the NPC's rotation and points in the direction of movement
        if (visionCone != null)
        {
            // Rotate the vision cone in sync with the NPC's rotation
            visionCone.transform.localRotation = Quaternion.Euler(0, 0, 0);  // Reset local rotation so it follows the NPC
        }
    }

    private IEnumerator PauseAndThink()
    {
        isPaused = true;

        string randomThought = npcPathConfig.randomThoughts[Random.Range(0, npcPathConfig.randomThoughts.Count)];
        Debug.Log($"NPC is paused: {randomThought}");

        yield return new WaitForSeconds(pauseDuration);

        Debug.Log("NPC finished thinking.");
        isPaused = false;
    }

    private IEnumerator LookAround()
    {
        isPaused = true;

        yield return StartCoroutine(Rotate(90));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Rotate(-180));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Rotate(90));

        Debug.Log("NPC finished looking around.");
        isPaused = false;
    }

    private IEnumerator Rotate(float angle)
    {
        float duration = 1f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, angle);

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
}