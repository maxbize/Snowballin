using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    private float originalScale;
    private float originalDistance;
    private GameObject player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update() {
        if (player != null) {
            transform.localScale = Vector3.one * originalScale / transform.parent.localScale.magnitude;
            transform.position = player.transform.position + (transform.position - player.transform.position).normalized * originalDistance;
        }
    }

    public void Attach(GameObject player) {
        originalScale = transform.localScale.magnitude;
        originalDistance = (player.transform.position - transform.position).magnitude;
        transform.parent = player.transform;
        this.player = player;
        GetComponent<SphereCollider>().enabled = false;
    }
}
