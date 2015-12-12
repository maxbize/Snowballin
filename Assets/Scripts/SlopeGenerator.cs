using UnityEngine;
using System.Collections;
using System;

/*
 * Mesh generation strategy:
 *  - Generate the slope as mesh "slices" that have limited length but full width
 *  - Only generate enough length to where the player can see (there's going to be fog)
 *  - As the player moves past a slice, move it from the back to the front and regenerate it
 */
public class SlopeGenerator : MonoBehaviour {

    // Set in editor
    public GameObject slopeSlicePrefab;
    public float vertexSpacing;

	// Use this for initialization
	void Start () {
        MakeMeshSlice(15, 5, Vector3.zero, null);
        Debug.Log("Hello");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void MakeMeshSlice(float width, float length, Vector3 origin, GameObject existingSlice) {
        if (existingSlice == null) {
            existingSlice = (GameObject)Instantiate(slopeSlicePrefab);
        }
        MeshFilter mf = existingSlice.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        int numColumns = (int)(width / vertexSpacing);
        int numRows = (int)(length / vertexSpacing); 
        Vector3[] verts = MakeVertices(numColumns, numRows, origin);
    
        mesh.vertices = verts;
        mesh.triangles = MakeTris(verts, numColumns, numRows);
        mesh.RecalculateNormals(); // Not enough time to figure this out
        mesh.normals = StylizeNormals(mesh.normals);
    }

    private Vector3[] MakeVertices(int numColumns, int numRows, Vector3 origin) {
        Vector3[] verts = new Vector3[numColumns * numRows];
        for (int i = 0; i < verts.Length; i++) {
            float x = vertexSpacing * (i % numColumns);
            float z = vertexSpacing * (i / numRows);
            verts[i] = origin + new Vector3(x, UnityEngine.Random.Range(0f,1f), z);
        }
        return verts;
    }

    private int[] MakeTris(Vector3[] verts, int numColumns, int numRows) {
        // # tris needed = number of boxes * 6. Subtract one from cols/rows to account for edges
        int[] tris = new int[(numColumns - 1) * (numRows - 1) * 6];
        int lowerLeft = 0;
        for (int i = 0; i < tris.Length; i += 6) {
            // Make tris one square at a time. Lower left then upper right tri
            // 'i' marks lower left corner of square
            int upperLeft = lowerLeft + numColumns;

            tris[i + 0] = lowerLeft;
            tris[i + 1] = upperLeft;
            tris[i + 2] = lowerLeft + 1;

            tris[i + 3] = lowerLeft + 1;
            tris[i + 4] = upperLeft;
            tris[i + 5] = upperLeft + 1;

            lowerLeft++;
            if (lowerLeft % numColumns == numColumns - 1) {
                lowerLeft++; // We're on the right edge
            }
        }

        return tris;
    }

    // Lets make it look low poly
    private Vector3[] StylizeNormals(Vector3[] originalNormals) {
        Vector3[] normals = new Vector3[originalNormals.Length];
        Array.Copy(originalNormals, normals, normals.Length);
        for (int i = 0; i < normals.Length; i++) {
            normals[i] = new Vector3(normals[i].x, normals[i].y * 0.5f, normals[i].z); 
        }
        return normals;
    }

    private Vector2[] MakeUvs(Vector3[] verts) {
        Vector2[] uvs = new Vector2[verts.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = Vector2.zero;
        }
        return uvs;
    }
}
