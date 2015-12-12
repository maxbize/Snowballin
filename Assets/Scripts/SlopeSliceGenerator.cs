using UnityEngine;
using System.Collections;
using System;

/*
 * Mesh generation strategy:
 *  - Generate the slope as mesh "slices" that have limited length but full width
 *  - Only generate enough length to where the player can see (there's going to be fog)
 *  - As the player moves past a slice, move it from the back to the front and regenerate it
 */
public class SlopeSliceGenerator : MonoBehaviour {

    // Set in editor
    public GameObject slopeSlicePrefab;
    public int vertexWidthSpacing;
    public int vertexLengthSpacing;
    public float slopeAngle;
    public int sliceWidth;
    public int sliceLength;

	// Use this for initialization
	void Start () {
        Debug.Log("Hello");
	}
	
	// Update is called once per frame
	void Update () {
	}

    // back/left/right slice are slices surrounding the new one
    public SlopeSlice MakeMeshSlice(GameObject existingSlice, SlopeSlice leftSlice, SlopeSlice backSlice, SlopeSlice rightSlice) {
        int numColumns = (int)(sliceWidth / vertexWidthSpacing) + 1; 
        int numRows = (int)(sliceLength / vertexLengthSpacing) + 1; 

        GameObject slice = existingSlice;
        if (slice == null) {
            slice = (GameObject)Instantiate(slopeSlicePrefab);
            slice.GetComponent<SlopeSlice>().Initialize(numColumns, numRows);
        }
        MeshFilter mf = slice.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        Vector3[] verts = MakeVertices(numColumns, numRows, slice.GetComponent<SlopeSlice>(), leftSlice, backSlice, rightSlice);
        int[] tris = MakeTris(verts, numColumns, numRows);
        verts = unshareVerts(tris, verts);
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        slice.GetComponent<MeshCollider>().sharedMesh = mesh;

        slice.transform.rotation = Quaternion.Euler(new Vector3(slopeAngle, 0, 0));
        slice.transform.position = calcSliceOrigin(leftSlice, backSlice, rightSlice);

        return slice.GetComponent<SlopeSlice>();
    }

    private Vector3[] MakeVertices(int numColumns, int numRows, SlopeSlice thisSlice, SlopeSlice leftSlice, SlopeSlice backSlice, SlopeSlice rightSlice) {
        Vector3[] verts = new Vector3[numColumns * numRows];
        for (int i = 0; i < verts.Length; i++) {
            float x = vertexWidthSpacing * (i % numColumns);
            float z = vertexLengthSpacing * (i / numColumns);
            Vector3 vert = new Vector3(x, UnityEngine.Random.Range(0f, 0.5f), z);

            // Check for any edges
            if (i % numColumns == 0) { // Left edge
                if (leftSlice != null) {
                    vert.y = leftSlice.rightVerts[i / numColumns].y;
                }
                thisSlice.leftVerts[i / numColumns] = vert;
            } else if (i % numColumns == numColumns - 1) { // Right edge
                if (rightSlice != null) {
                    vert.y = rightSlice.leftVerts[i / numColumns].y;
                }
                thisSlice.rightVerts[i / numColumns] = vert;
            } 
            // Can't be an else if - it shares corners with above
            if (i >= numColumns * (numRows - 1)) { // Front edge
                thisSlice.frontVerts[i - numColumns * (numRows - 1)] = vert;
            }
            if (i < numColumns) { // Back edge
                if (backSlice != null) {
                    vert.y = backSlice.frontVerts[i].y;
                }
            }

            verts[i] = vert;
        }
        return verts;
    }

    private Vector3 calcSliceOrigin(SlopeSlice leftSlice, SlopeSlice backSlice, SlopeSlice rightSlice) {
        if (leftSlice != null) {
            return leftSlice.gameObject.transform.position + Vector3.right * sliceWidth;
        } else if (backSlice != null) {
            return backSlice.gameObject.transform.position + backSlice.transform.forward * sliceLength;
        } else if (rightSlice != null) {
            return rightSlice.gameObject.transform.position - Vector3.right * sliceWidth;
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
