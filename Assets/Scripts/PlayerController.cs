using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // Set in editor
    public float moveSpeed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.RightArrow)) {
            input += Vector3.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            input -= Vector3.right;
        }
        Move(input);
	}

    private void Move(Vector3 dir) {
        rb.AddForce(dir * moveSpeed * rb.mass);
    }

}
