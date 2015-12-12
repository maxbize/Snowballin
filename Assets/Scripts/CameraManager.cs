using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    // Set in editor
    public GameObject focus;
    public float distance;
    public float angle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = Quaternion.Euler(angle, 0, 0) * Vector3.forward;
        offset *= distance;
        transform.position = focus.transform.position + offset;
    }
}
