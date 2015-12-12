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
    public float vertexWidthSpacing;
    public float vertexLengthSpacing;
    public float slopeAngle;
    public float sliceWidth;
    public float sliceLength;
    public float totalLength;


	// Use this for initialization
	void Start () {
        MakeMeshSlice(null, null, null, null);
        Debug.Log("Hello");
	}
	
	// Update is called once per frame
	void Update () {
        HandleSlices();
	}

    // Generate new slices and recycle old ones
    private void HandleSlices() {
    
    }

    // back/left/right slice are slices surrounding the new one
    private void MakeMeshSlice(GameObject existingSlice, SlopeSlice leftSlice, SlopeSlice backSlice, SlopeSlice rightSlice) {
        GameObject slice = existingSlice;
        if (slice == null) {
            slice = (GameObject)Instantiate(slopeSlicePrefab);
        }
        MeshFilter mf = slice.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        int numColumns = (int)(sliceWidth / vertexWidthSpacing); // If you update this logic, update hack in SlopeSlice.Start()
        int numRows = (int)(sliceLength / vertexLengthSpacing); // If you update this logic, update hack in SlopeSlice.Start()

        Vector3[] verts = MakeVertices(numColumns, numRows, slice.GetComponent<SlopeSlice>(), leftSlice, backSlice, rightSlice);
        int[] tris = MakeTris(verts, numColumns, numRows);
        verts = unshareVerts(tris, verts);
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        slice.GetComponent<MeshCollider>().sharedMesh = mesh;

        slice.transform.rotation = Quaternion.Euler(new Vector3(slopeAngle, 0, 0));
    }

    private Vector3[] MakeVertices(int numColumns, int numRows, SlopeSlice thisSlice, SlopeSlice leftSlice, SlopeSlice backSlice, SlopeSlice rightSlice) {
        Vector3 origin = calcSliceOrigin(leftSlice, backSlice, rightSlice);
        Vector3[] verts = new Vector3[numColumns * numRows];
        for (int i = 0; i < verts.Length; i++) {
            float x = vertexWidthSpacing * (i % numColumns);
            float z = vertexLengthSpacing * (i / numColumns);
            Vector3 vert = origin + new Vector3(x, UnityEngine.Random.Range(0f, 0.5f), z);
            verts[i] = vert;

            // Record any edges
            if (i % numColumns == 0) {
                thisSlice.leftVerts[i % numColumns] = vert;
            } else if (i % numColumns == numColumns - 1) {
                thisSlice.rightVerts[i % numColumns] = vert;
            } else if (i > numColumns * (numRows - 1)) {
                thisSlice.frontVerts[i - numColumns * (numRows - 1)] = vert;
            }
        }
        return verts;
    }

    private Vector3 calcSliceOrigin(SlopeSlice leftSlice, SlopeSlice backSlice, SlopeSlice rightSlice) {
        if (leftSlice != null) {
            return leftSlice.rightVerts[0];
        } else if (backSlice != null) {
            return backSlice.leftVerts[backSlice.leftVerts.Length - 1];
        } else if (rightSlice != null) {
            return rightSlice.leftVerts[0] - Vector3.right * sliceWidth;
        } else {
            return new Vector3(-sliceWidth / 2, 0f, 0f);
        }
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

    private Vector3[] unshareVerts(int[] tris, Vector3[] verts) {
        // Get rid of soft edges
        Vector3[] vertices = new Vector3[tris.Length];
        for (int i = 0; i < tris.Length; i++) {
            vertices[i] = verts[tris[i]];
            tris[i] = i;
        }
        return vertices;
    }

    private Vector2[] MakeUvs(Vector3[] verts) {
        Vector2[] uvs = new Vector2[verts.Length];
        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = Vector2.zero;
        }
        return uvs;
    }
}
