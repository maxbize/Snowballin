using UnityEngine;
using System.Collections;

/*
 * Player can grow by rolling down the hill and collecting objects that are smaller
 * Hitting a larger object causes player to lose mass
 */
public class Player : MonoBehaviour {

    // Set in editor
    public float moveSpeed;
    public SlopeManager slopeManager;
    public float growthRate;

    private Rigidbody rb;
    public SlopeSlice currentSlice { get; private set; }
    private bool grounded = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (grounded) {
            Grow();
        }
        CheckGrounded();
        Move();
	}

    private void Grow() {
        transform.localScale += Vector3.one * growthRate * Time.deltaTime;
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

        rb.AddForce((input * moveSpeed * rb.mass + Vector3.forward * moveSpeed) * Time.deltaTime);
    }

}
