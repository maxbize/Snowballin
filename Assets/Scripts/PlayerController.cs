using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // Set in editor
    public float moveSpeed;
    public SlopeManager slopeManager;

    private Rigidbody rb;
    public SlopeSlice currentSlice { get; private set; }
    private bool grounded = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckGrounded();
        Move();
	}

    private void CheckGrounded() {
        SlopeSlice slice = slopeManager.FindSlice(transform.position, transform.localScale.magnitude * 1.1f);
        if (slice == null) {
            grounded = false;
        } else {
            grounded = true;
            currentSlice = slice;
        }
    }

    private void Move() {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.RightArrow)) {
            input += Vector3.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            input -= Vector3.right;
        }

        rb.AddForce(input * moveSpeed * rb.mass + Vector3.forward * moveSpeed);
    }

}
