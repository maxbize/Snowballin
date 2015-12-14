using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    private Vector3 originalScale;
    private float originalDistance;
    private Vector3 originalPlayerScale;
    private Player player;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
    void Update() {
        if (player != null) {
            Vector3 newScale = originalScale;
            newScale.x /= transform.parent.localScale.x;
            newScale.y /= transform.parent.localScale.y;
            newScale.z /= transform.parent.localScale.z;
            transform.localScale = newScale;
            transform.position = player.transform.position + (transform.position - player.transform.position).normalized * originalDistance;
        }
    }

    public void Attach(Player player) {
        this.player = player;
        originalScale = transform.localScale;
        originalDistance = (player.transform.position - transform.position).magnitude - transform.localScale.magnitude / 5;
        transform.parent = player.transform;
        originalPlayerScale = player.targetScale;
        GetComponent<Collider>().enabled = false;
    }


    // Detach if the player lost more scale then when we attached
    public void CheckDetach(bool force) {
        if (player.targetScale.magnitude < originalPlayerScale.magnitude || force) {
            Blast(player.GetComponent<Rigidbody>().velocity.magnitude);
            transform.parent = null;
            player = null;
        }
    }

    // We've detached or been hit by something smaller
    public void Blast(float playerVel) {
        Invoke("Die", 10);
        Vector3 detachedVel = Random.onUnitSphere;
        rb.angularVelocity = detachedVel * 2;
        if (detachedVel.y < 0) {
            detachedVel.y = -detachedVel.y;
        }
        if (detachedVel.z < 0) {
            detachedVel.z = -detachedVel.z;
        }
        detachedVel.z += 3f;
        detachedVel *= Random.Range(1f, 3f);
        detachedVel *= Mathf.Clamp(playerVel / 5, 1, 10);
        rb.velocity = detachedVel;
        rb.isKinematic = false;
        GetComponent<Collider>().enabled = false;
    }

    private void Die() {
        Destroy(gameObject);
    }
}
