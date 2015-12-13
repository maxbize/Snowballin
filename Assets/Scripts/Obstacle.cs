using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    private float originalScale;
    private float originalDistance;
    private Vector3 originalPlayerScale;
    private Player player;
    private Vector3 detachedVel; // Velocity when detached. Maybe switch to instantiating a RB at some point

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
    void Update() {
        if (player != null) {
            transform.localScale = Vector3.one * originalScale / transform.parent.localScale.magnitude;
            transform.position = player.transform.position + (transform.position - player.transform.position).normalized * originalDistance;
        } else if (detachedVel != Vector3.zero) {
            detachedVel += Physics.gravity * Time.deltaTime;
            transform.position += detachedVel * Time.deltaTime;
        }
    }

    public void Attach(Player player) {
        this.player = player;
        originalScale = transform.localScale.magnitude;
        originalDistance = (player.transform.position - transform.position).magnitude;
        transform.parent = player.transform;
        GetComponent<SphereCollider>().enabled = false;
        originalPlayerScale = player.targetScale;
    }


    // Detach if the player lost more scale then when we attached
    public void CheckDetach() {
        if (player.targetScale.magnitude < originalPlayerScale.magnitude) {
            transform.parent = null;
            player = null;
            Blast();
        }
    }

    // We've detached or been hit by something smaller
    public void Blast() {
        Invoke("Die", 5);
        detachedVel = Random.onUnitSphere;
        if (detachedVel.y < 0) {
            detachedVel.y = -detachedVel.y;
        }
        if (detachedVel.z < 0) {
            detachedVel.z = -detachedVel.z;
        }
        detachedVel.z += 3f;
        detachedVel *= Random.Range(3f, 5f);
    }

    private void Die() {
        Destroy(gameObject);
    }
}
