﻿using UnityEngine;
using System.Collections;

public class LerpToTarget : MonoBehaviour {

    // Set in editor
    public float lerpRate;
    public float threshold;

    [HideInInspector]
    public Vector3 target;

	// Use this for initialization
	void Start () {
        lerpRate *= Random.Range(0.2f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 toTarget = target - transform.position;
        transform.position += toTarget * lerpRate * Mathf.Clamp(Time.deltaTime, 0, 0.1f);
        if (toTarget.magnitude < threshold) {
            transform.position = target;
        }
	}
}
