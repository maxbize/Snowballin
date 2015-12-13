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
    public ParticleSystem snowTrail;
    public GameObject impactPrefab;
    public float invulnTime;
    public float maxScale;
    public float minScale;

    public Vector3 targetScale { get; private set; }

    private Rigidbody rb;
    private bool grounded = false;
    private float invulnTimer = 0;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        targetScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (grounded) {
            Grow();
        }
        CheckGrounded();
        Move();
        transform.localScale += (targetScale.magnitude - transform.localScale.magnitude) / 200 * Vector3.one;
        invulnTimer -= Time.deltaTime;

        if (targetScale.x < minScale) {
            targetScale = Vector3.one * minScale;
        }
        if (targetScale.x > maxScale) {
            targetScale = Vector3.one * maxScale;
        }
    }

    private void Grow() {
        targetScale += Vector3.one * growthRate * Time.deltaTime;
    }

    private void CheckGrounded() {
        SlopeSlice slice = slopeManager.FindSlice(transform.position, transform.localScale.y / 2 + 0.1f);
        if (slice == null) {
            grounded = false;
            snowTrail.enableEmission = false;
        } else {
            grounded = true;
            snowTrail.enableEmission = true;
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
        targetScale += Vector3.one * Mathf.Sqrt(obstacle.transform.localScale.magnitude) / 10;
        obstacle.Attach(this);
    }

    private void ImpactObstacle(Obstacle obstacle) {
        ((GameObject)Instantiate(impactPrefab, transform.position, Quaternion.identity)).transform.localScale = transform.localScale / 4;
        if (invulnTimer <= 0) {
            targetScale *= 0.9f;
        }
        invulnTimer = invulnTime; // Being nice :)
        obstacle.Blast(rb.velocity.magnitude);
        foreach (Obstacle childObstacle in GetComponentsInChildren<Obstacle>()) {
            childObstacle.CheckDetach();
        }
        rb.velocity /= 2;
    }
}
