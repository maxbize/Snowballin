using UnityEngine;
using System.Collections;

public class FollowTargetOnGround : MonoBehaviour {
    
    // Set in editor
    public GameObject target;

    private Vector3 toGround;

	// Use this for initialization
	void Start () {
        // Copy/pasta hack
        toGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle + 90, 0, 0) * Vector3.forward;
        toGround.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position + toGround * target.transform.localScale.magnitude / 4;
	}
}
