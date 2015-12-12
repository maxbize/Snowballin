using UnityEngine;
using System.Collections;

/*
 * Mesh generation strategy:
 *  - Generate the slope as mesh "slices" that have limited length but full width
 *  - Only generate enough length to where the player can see (there's going to be fog)
 *  - As the player moves past a slice, move it from the back to the front and regenerate it
 */
public class SlopeGenerator : MonoBehaviour {

    // Set in editor
    public float vertexSpacing;

	// Use this for initialization
	void Start () {
        MakeMeshSlice(3, 3, Vector3.zero);
        Debug.Log("Hello");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void MakeMeshSlice(float width, float length, Vector3 origin) {
        MakeVertices(width, length, origin);
    }

    private void MakeVertices(float width, float length, Vector3 origin) {
        int numColumns = (int)(width / vertexSpacing);
        int numRows = (int)(length / vertexSpacing);
        Vector3[] verts = new Vector3[numColumns * numRows];
        for (int i = 0; i < verts.Length; i++) {
            float x = vertexSpacing * (i % numColumns);
            float z = vertexSpacing * (i / numRows);
            verts[i] = origin + new Vector3(x, 0, z);
        }
        Debug.Log(verts);
    }

    private void MakeTriangles() {

    }

    private void MakeNormals() {

    }

    private void MakeUvs() {

    }

    /*
    function Example() {  
    var mf: MeshFilter = GetComponent.<MeshFilter>();
    var mesh = new Mesh();
    mf.mesh = mesh;
    
    var vertices: Vector3[] = new Vector3[4];
    
    vertices[0] = new Vector3(0, 0, 0);
    vertices[1] = new Vector3(width, 0, 0);
    vertices[2] = new Vector3(0, height, 0);
    vertices[3] = new Vector3(width, height, 0);
    
    mesh.vertices = vertices;
    
    var tri: int[] = new int[6];

    tri[0] = 0;
    tri[1] = 2;
    tri[2] = 1;
    
    tri[3] = 2;
    tri[4] = 3;
    tri[5] = 1;
    
    mesh.triangles = tri;
    
    var normals: Vector3[] = new Vector3[4];
    
    normals[0] = -Vector3.forward;
    normals[1] = -Vector3.forward;
    normals[2] = -Vector3.forward;
    normals[3] = -Vector3.forward;
    
    mesh.normals = normals;
    
    var uv: Vector2[] = new Vector2[4];

    uv[0] = new Vector2(0, 0);
    uv[1] = new Vector2(1, 0);
    uv[2] = new Vector2(0, 1);
    uv[3] = new Vector2(1, 1);
    
    mesh.uv = uv;
}
     */
}
