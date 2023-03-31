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

    [Header("Values")]
    [Range(10, 100)]
    [SerializeField] int count;
    [SerializeField] bool drawGizmos;

    List<MeshVertices> meshes = new List<MeshVertices>();
    Mesh mesh;
    List<int> triangleIndices = new List<int>();


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
        for (int i = 0; i < meshes.Count - 1; i++)
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

            // Loop all faces that will be between two 2D meshes
            for (int x = 0; x < meshPrefab.vertices.Length - 1; x++)
            {
                Vector3 curVertex1 = currentMesh.vertices[x].position;
                Vector3 curVertex2 = currentMesh.vertices[x + 1].position;
                Vector3 nextVertex1 = nextMesh.vertices[x].position;
                Vector3 nextVertex2 = nextMesh.vertices[x + 1].position;

                //Create vertices
                Vector3[] verts = new Vector3[]
                {
       
                    curVertex1,
      
                    curVertex2,
       
                    nextVertex1,
       
                    nextVertex2
                };

                //Create triangles
                int[] Newtriangles = new int[]
                {
       
                    0, 1, 2,
       
                    1, 3, 2
                };

                //Add indices to triangleIndices list
                int startIndex = vertices.Count;
                for (int j = 0; j < Newtriangles.Length; j++)
                    triangleIndices.Add(startIndex + Newtriangles[j]);

                //Add vertices to vertices list
                vertices.AddRange(verts);
            }

        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleIndices.ToArray();

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
