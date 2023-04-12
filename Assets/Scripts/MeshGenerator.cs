using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshGenerator : MonoBehaviour
{
    public bool generate;
    public bool clearMeshList;

    [Header("Components")]
    [SerializeField] BezierCurve bezierCurve;
    [SerializeField] MeshVertices meshPrefab;
    [SerializeField] Transform meshParent;
    [SerializeField] MeshCollider meshCollider;

    [Header("Values")]
    [Range(10, 100)]
    [SerializeField] int count;
    [SerializeField] bool drawGizmos;

    List<MeshVertices> meshes = new List<MeshVertices>();
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            generate = false;
            GenerateMesh();
        }

        if (clearMeshList)
        {
            clearMeshList = false;
            meshes.Clear();
        }
    }

    void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //Clear & delete old meshes
        if (meshes.Count > 0)
        {
            foreach (MeshVertices mesh in meshes)
                DestroyImmediate(mesh.gameObject, false);

            meshes.Clear();
        }

        //Get all points on curve
        int countPerLine = Mathf.CeilToInt(((float)count / bezierCurve.GetControlPointsCount()));
        Vector3[] points = bezierCurve.GetAllPointsOnCurve(countPerLine);
        Debug.Log("Bezier curve points: " + points.Length);

        //Loop the points and instantiate meshes along it
        int interval = Mathf.RoundToInt(((float)points.Length / count));
        for (int i = 0; i < points.Length; i++)
        {
            if (i % interval == 0)
            {
                //Get direction to next point
                Vector3 direction = Vector3.one;
                if (i == points.Length - 1) //From last to first
                {
                    direction = points[0] - points[i];
                }
                else //From current to next
                {
                    direction = points[i + 1] - points[i];
                }

                //Instantiate mesh
                MeshVertices mesh = Instantiate(meshPrefab, points[i], Quaternion.LookRotation(direction));
                mesh.transform.SetParent(meshParent);

                //Add to list
                meshes.Add(mesh);
            }
        }

        //Start making the whole mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //Create list for vertices & triangles
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        //Loop all instantiated 2D meshes through
        int faceIndex = 0;
        for (int i = 0; i < meshes.Count; i++)
        {
            MeshVertices currentMesh = null;
            MeshVertices nextMesh = null;

            if (i == meshes.Count - 1) //From last to first
            {
                currentMesh = meshes[i];
                nextMesh = meshes[0];
            }
            else //From current to next
            {
                currentMesh = meshes[i];
                nextMesh = meshes[i + 1];
            }

            //Loop all faces that will be between two 2D meshes
            for (int x = 0; x < meshPrefab.vertices.Length; x++)
            {
                int curIndex = 0;
                int nextIndex = 0;

                if (x == meshPrefab.vertices.Length - 1) //From last to first
                {
                    curIndex = x;
                    nextIndex = 0;
                }
                else //From current to next
                {
                    curIndex = x;
                    nextIndex = x + 1;
                }

                Vector3 curVertex1 = currentMesh.vertices[curIndex].position;
                Vector3 curVertex2 = currentMesh.vertices[nextIndex].position;
                Vector3 nextVertex1 = nextMesh.vertices[curIndex].position;
                Vector3 nextVertex2 = nextMesh.vertices[nextIndex].position;

                //Create vertices
                Vector3[] verts = new Vector3[]
                {
                curVertex1,
                curVertex2,
                nextVertex1,
                nextVertex2
                };

                //Create vertices
                int trisMult = faceIndex * 4; //This goes to next vertices on mesh as it goes along the path
                int[] tris = new int[]
                {
                0 + trisMult, 1 + trisMult, 2 + trisMult,
                3 + trisMult, 2 + trisMult, 1 + trisMult
                };

                vertices.AddRange(verts);
                triangles.AddRange(tris);

                faceIndex++;
            }
        }

        //Assign to mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        //Assign UVs to mesh
        Vector2[] uvs = new Vector2[vertices.Count];

        Debug.Log("Mesh count: " + meshes.Count);
        Debug.Log("Vertices count: " + vertices.Count);
        Debug.Log("Faces count: " + vertices.Count / 4);

        //Setup for loop with overall index and loop all vertices 
        int index = 0;
        for (int i = 0; i < meshes.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (i == meshes.Count - 1) //last to first
                {
                    //Get uv scale based on vertice direction and apply it to the uv cords
                    Vector2 uvScale = GetUVScale(meshes[i].transform.position, meshes[0].transform.position);

                    float xCord = uvScale.x * vertices[index].x;
                    float zCord = uvScale.y * vertices[index].z;

                    uvs[index] = new Vector2(xCord + zCord, vertices[index].y);
                }
                else
                {
                    //Get uv scale based on vertice direction and apply it to the uv cords
                    Vector2 uvScale = GetUVScale(meshes[i].transform.position, meshes[i + 1].transform.position);

                    float xCord = uvScale.x * vertices[index].x;
                    float zCord = uvScale.y * vertices[index].z;

                    uvs[index] = new Vector2(xCord + zCord, vertices[index].y);
                }
                index++;
            }
        }

        //Apply uv and recalculate normals
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        //Flip the mesh normals
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < mesh.normals.Length; i++)
        {
            normals[i] = -1 * normals[i];
        }

        mesh.normals = normals;

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for (int j = 0; j < tris.Length; j += 3)
            {
                int temp = tris[j];
                tris[j] = tris[j + 1];
                tris[j + 1] = temp;
            }
            mesh.SetTriangles(tris, i);
        }

        meshCollider.sharedMesh = mesh;
    }



    Vector2 GetUVScale(Vector3 from, Vector3 to)
    {
        //Direction
        Vector3 dir = to - from;
        dir.Normalize();

        //Create the scale from the normalized direction
        Vector2 uvScale;
        uvScale.x = dir.x;
        uvScale.y = dir.z;

        return uvScale;
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        if (meshes.Count == 0)
            return;

        //Try to draw lines between meshes, can give error if some entry is null on the list
        try
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                MeshVertices currentMesh = null;
                MeshVertices nextMesh = null;

                if (i == meshes.Count - 1) //From last to first
                {
                    currentMesh = meshes[i];
                    nextMesh = meshes[0];
                }
                else //From current to next
                {
                    currentMesh = meshes[i];
                    nextMesh = meshes[i + 1];
                }

                //Draw lines between meshes
                for (int x = 0; x < currentMesh.vertices.Length; x++)
                    Gizmos.DrawLine(currentMesh.vertices[x].position, nextMesh.vertices[x].position);
            }
        }
        catch { }
    }
}
