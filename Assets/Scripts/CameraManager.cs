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
        float ballToCamAngle = 1f * focus.transform.localScale.magnitude + 215f;
        Vector3 offset = Quaternion.Euler(ballToCamAngle, 0, 0) * Vector3.forward;

        float viewScaler = 0.05f * focus.transform.localScale.magnitude + 0.5f; // Increase to make the ball take more screen space
        offset *= focus.transform.localScale.magnitude / (Mathf.Tan(Camera.main.fieldOfView / 2 * Mathf.Deg2Rad) * viewScaler);
        transform.position = focus.transform.position + offset;

        float cameraAngle = 0.8f * focus.transform.localScale.magnitude + 25;
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
    }
}
