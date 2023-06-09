using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{

    [Range(1.0f, 1000.0f)]
    public float Size = 10.0f;

    [Range(2, 255)]
    public int Segments = 10;

    private Mesh mesh = null;
    public bool FlattenBelowZero = true;

    [Range(1f, 100f)]
    public float NoiseFactor;

    [Range(-2f, 1f)]
    public float Flattener;

    [Range(0f, 10f)]
    public float Factor1;
    [Range(0f, 10f)]
    public float Factor2;
    [Range(0f, 10f)]
    public float Factor3;
    [Range(0f, 10f)]
    public float Factor4;

    private void OnEnable()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        GenerateMesh();
    }

    private void GenerateMesh()
    {
        mesh.Clear();
        // vertices
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        // Delta between segments
        float delta = Size / (float)Segments;

        // Generate the vertices
        float x = 0.0f;
        float y = 0.0f;
        for (int seg_x = 0; seg_x <= Segments; seg_x++)
        {
            x = (float)seg_x * delta;
            for (int seg_y = 0; seg_y <= Segments; seg_y++)
            {
                y = (float)seg_y * delta;

                float z1 = Factor1 * (Mathf.PerlinNoise(x / 500f, y / 500f) - 0.5f);
                float z2 = Factor2 * (Mathf.PerlinNoise(x / 100f, y / 100f) - 0.5f);
                float z3 = Factor3 * (Mathf.PerlinNoise(x / 70f, y / 70f) - 0.5f);
                float z4 = Factor4 * (Mathf.PerlinNoise(x, y) - 0.5f);
                float z = z1 + z2 + z3 + z4;
                //float z = (Mathf.PerlinNoise(x, y) - 0.5f);
                if (FlattenBelowZero && z < Flattener)
                    z = Flattener;

                verts.Add(new Vector3(x, z * NoiseFactor, y));
            }
        }

        // Generate the triangle indices
        for (int seg_x = 0; seg_x < Segments; seg_x++)
        {
            for (int seg_y = 0; seg_y < Segments; seg_y++)
            {
                // Total count of vertices per row & col is: Segments + 1
                int index = seg_x * (Segments + 1) + seg_y;
                int index_lower = index + 1;
                int index_next_col = index + (Segments + 1);
                int index_next_col_lower = index_next_col + 1;

                tris.Add(index);
                tris.Add(index_lower);
                tris.Add(index_next_col);

                tris.Add(index_next_col);
                tris.Add(index_lower);
                tris.Add(index_next_col_lower);
            }
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
    }

    private void OnValidate()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        GenerateMesh();
        //AddNoiseToMesh(mesh);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
