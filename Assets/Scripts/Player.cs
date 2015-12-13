using UnityEngine;
using System.Collections;

/*
 * Player can grow by rolling down the hill and collecting objects that are smaller
 * Hitting a larger object causes player to lose mass
 */
public class Player : MonoBehaviour {

    // Set in editor
    public float forwardSpeed;
    public float forwardDrag;
    public float sideSpeed;
    public float sideDrag;
    public SlopeManager slopeManager;
    public float growthRate;

    public Vector3 targetScale { get; private set; }

    private Rigidbody rb;
    public SlopeSlice currentSlice { get; private set; }
    private bool grounded = false;
    private float minScale;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        minScale = transform.localScale.x;
        targetScale = transform.localScale;
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
        targetScale += Vector3.one * growthRate * Time.deltaTime;
        transform.localScale += (targetScale.magnitude - transform.localScale.magnitude) / 10 * Vector3.one;
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

    // For simplicity we assume z is forward, even though it should be z rotated down by the slope angle
    private void Move() {
        Vector3 force = Vector3.zero;
        if (Input.GetKey(KeyCode.RightArrow)) {
            force += Vector3.right * sideSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            force -= Vector3.right * sideSpeed;
        }
        force -= rb.velocity.x * rb.velocity.x * sideDrag * Vector3.right * Mathf.Sign(rb.velocity.x);

        force += (forwardSpeed - rb.velocity.z * rb.velocity.z * forwardDrag) * Vector3.forward;

        rb.AddForce(force * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle != null) {
            float scaleDif = obstacle.gameObject.transform.localScale.magnitude - targetScale.magnitude;
            // Scale != size. This only works if we import things to Unity to be size 1 at scale 1
            if (scaleDif > 0) {
                ImpactObstacle(obstacle);
            } else {
                AbsorbObstacle(obstacle);
            }
        }
    }

    private void AbsorbObstacle(Obstacle obstacle) {
        targetScale += Vector3.one * obstacle.transform.localScale.magnitude / 10;
        obstacle.Attach(this);
    }

    private void ImpactObstacle(Obstacle obstacle) {
        targetScale -= Vector3.one * obstacle.transform.localScale.magnitude / 5;
        if (targetScale.x < minScale) {
            targetScale = Vector3.one * minScale;
        }
        obstacle.Blast(rb.velocity.magnitude);
        foreach (Obstacle childObstacle in GetComponentsInChildren<Obstacle>()) {
            childObstacle.CheckDetach();
        }
        rb.velocity /= 2;
    }
}
