using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    public float viewAngle = 90f;  // Field of view angle
    public float viewDistance = 5f;  // Maximum vision cone length
    public int rayCount = 20;  // Number of rays for smoothness
    public LayerMask wallMask;  // Layer mask for detecting walls (set to "Wall" layer)

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        UpdateVisionCone();
    }

    public void UpdateVisionCone()
    {
        mesh.Clear();  // Clear the mesh before recalculating

        // Calculate the angle step between each ray
        float angleStep = viewAngle / rayCount;
        float startAngle = -viewAngle / 2f;  // Start angle at the leftmost side of the cone

        // Prepare arrays for vertices and triangles
        Vector3[] vertices = new Vector3[rayCount + 2];  // Center point + ray points
        int[] triangles = new int[rayCount * 3];  // Each ray forms a triangle

        // Center point (where the NPC is)
        vertices[0] = Vector3.zero;  // Center vertex

        // Generate the ray vertices at max view distance
        for (int i = 0; i <= rayCount; i++)
        {
            // Calculate the current angle for this ray
            float currentAngle = startAngle + (i * angleStep);
            Vector3 rayDirection = new Vector3(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),  // X-axis
                0,  // Y-axis stays zero (X-Z plane)
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)   // Z-axis
            ).normalized;

            // Store the ray vertex at the max vision distance
            vertices[i + 1] = rayDirection * viewDistance;
        }

        // Generate the triangles to form the cone
        for (int i = 0; i < rayCount; i++)
        {
            triangles[i * 3] = 0;        // Center vertex
            triangles[i * 3 + 1] = i + 1;  // Current vertex
            triangles[i * 3 + 2] = i + 2;  // Next vertex
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Ensure correct normals and winding order
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // Debugging: Visualize the vertices with Gizmos in the Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw rays from the center to each vertex
        for (int i = 1; i <= rayCount; i++)
        {
            Gizmos.DrawLine(transform.position, transform.position + mesh.vertices[i]);
        }

        // Draw the center point for reference
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}