using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshVertices : MonoBehaviour
{
    public Transform[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        if (vertices.Length < 2)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (i == vertices.Length - 1) Gizmos.DrawLine(vertices[i].position, vertices[0].position);
            else Gizmos.DrawLine(vertices[i].position, vertices[i + 1].position);
        }
    }
}
