using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {

    // Set in editor
    public float speed;
    public float height;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = Vector3.forward * speed; // Easier to play with changing in update
        transform.position += Vector3.down * (GetDistanceToGround() - height);
	}

    private float GetDistanceToGround() {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, Vector3.down, 100);
        foreach (RaycastHit hit in hits) {
            if (hit.collider.GetComponent<SlopeSlice>() != null) {
                return hit.distance;
            }
        }
        return 0;
    }
}
