﻿using UnityEngine;
using System.Collections;

// Carries some info about a slice for convenience
public class SlopeSlice : MonoBehaviour {

    // Store the edge verts to make things seamless
    [HideInInspector]
    public Vector3[] leftVerts;
    [HideInInspector]
    public Vector3[] rightVerts;
    [HideInInspector]
    public Vector3[] frontVerts;

    // Generic position
    [HideInInspector]
    public Int2 pos;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(int numColumns, int numRows) {
        leftVerts = new Vector3[numRows];
        rightVerts = new Vector3[numRows];
        frontVerts = new Vector3[numColumns];
    }
}
