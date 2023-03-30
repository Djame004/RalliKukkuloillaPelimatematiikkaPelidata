using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RollerCoasterTrack : MonoBehaviour
{
    public BezierCurve bezierCurve;
    public float width = 1f;
    public float thickness = 0.1f;

    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void OnValidate()
    {
        if (bezierCurve == null)
            return;

        GenerateMesh();
    }

    private void GenerateMesh()
    {
        int numPoints = bezierCurve.numPoints;

        Vector3[] vertices = new Vector3[numPoints * 2];
        Vector2[] uv = new Vector2[numPoints * 2];
        int[] triangles = new int[(numPoints - 1) * 6];

        for (int i = 0; i < numPoints; i++)
        {
            float t = (float)i / (numPoints - 1);
            Vector3 point = bezierCurve.CalculateBezierPoint(t, bezierCurve.StartPoint.position, bezierCurve.AnchorPoints[i].position, bezierCurve.EndPoints[i].position);
            Vector3 direction = bezierCurve.CalculateBezierPointDerivative(t, bezierCurve.StartPoint.position, bezierCurve.AnchorPoints[i].position, bezierCurve.EndPoints[i].position).normalized;
            Vector3 normal = Vector3.Cross(Vector3.up, direction).normalized;

            vertices[i * 2] = point - normal * width / 2f;
            vertices[i * 2 + 1] = point + normal * width / 2f;

            uv[i * 2] = new Vector2(t, 0f);
            uv[i * 2 + 1] = new Vector2(t, 1f);

            if (i > 0)
            {
                int triangleIndex = (i - 1) * 6;

                triangles[triangleIndex] = i * 2 - 2;
                triangles[triangleIndex + 1] = i * 2 + 1;
                triangles[triangleIndex + 2] = i * 2 - 1;

                triangles[triangleIndex + 3] = i * 2 - 2;
                triangles[triangleIndex + 4] = i * 2;
                triangles[triangleIndex + 5] = i * 2 + 1;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -normals[i];
        mesh.normals = normals;

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] subTriangles = mesh.GetTriangles(i);
            for (int j = 0; j < subTriangles.Length; j += 3)
            {
                int temp = subTriangles[j];
                subTriangles[j] = subTriangles[j + 1];
                subTriangles[j + 1] = temp;
            }
            mesh.SetTriangles(subTriangles, i);
        }
    }
}
