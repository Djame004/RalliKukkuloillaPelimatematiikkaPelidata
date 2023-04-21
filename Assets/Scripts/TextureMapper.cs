using UnityEngine;

public class TextureMapper : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Material material;

    void Start()
    {
        // Get the mesh and material from the mesh renderer
        Mesh mesh = meshFilter.mesh;

        // Generate UVs for the mesh
        Vector2[] uvs = new Vector2[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            uvs[i] = new Vector2(mesh.vertices[i].x / material.mainTexture.width, mesh.vertices[i].z / material.mainTexture.height);
        }
        mesh.uv = uvs;

        // Assign the material back to the mesh renderer
        meshRenderer.material = material;
    }
}
