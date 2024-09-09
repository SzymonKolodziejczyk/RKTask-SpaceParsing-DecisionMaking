using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionCone : MonoBehaviour
{
    public float viewAngle = 90f;  // Field of view angle
    public float viewDistance = 5f;  // Vision cone length
    public int rayCount = 20;  // Number of rays to create smoothness of the cone

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        UpdateVisionCone();
    }

    public void UpdateVisionCone()
    {
        float angleStep = viewAngle / rayCount;
        float angle = -viewAngle / 2;  // Start angle (half FOV to the left)

        Vector3[] vertices = new Vector3[rayCount + 2];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;  // Center of the cone at the NPC's position

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle) * viewDistance,
                0, // Y axis is 0, since we are generating the cone in X-Z plane
                Mathf.Sin(Mathf.Deg2Rad * angle) * viewDistance
            );
            vertices[i + 1] = vertex;

            if (i < rayCount)
            {
                // Create triangles for the cone
                triangles[i * 3] = 0;  // Center vertex
                triangles[i * 3 + 1] = i + 1;  // Current vertex
                triangles[i * 3 + 2] = i + 2;  // Next vertex
            }

            angle += angleStep;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}