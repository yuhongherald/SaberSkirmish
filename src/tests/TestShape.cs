using UnityEngine;

[ExecuteInEditMode]
public class TestShape : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
        }
        Shader shader = Shader.Find("Standard");
        Material material = new Material(shader);
        material.color = new Color(0, 0, 0, 0);
        renderer.material = material;

        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-1, -1, 0);
        vertices[1] = new Vector3(-1, 1, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(1, -1, 0);
        mesh.vertices = vertices;

        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 3;
        triangles[4] = 1;
        triangles[5] = 2;
        // we use the center of the arrow as its position, so we compute it here
        // movement is linear for parameter

        mesh.triangles = triangles;
        filter.mesh = mesh;
    }
}
