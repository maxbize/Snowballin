using UnityEngine;
using System.Collections;

public class LerpToTarget : MonoBehaviour {

    // Set in editor
    public float lerpRate;
    public float threshold;

    [HideInInspector]
    public Vector3 target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 toTarget = target - transform.position;
        transform.position += toTarget * lerpRate;
        if (toTarget.magnitude < threshold) {
            transform.position = target;
        }
	}
}
