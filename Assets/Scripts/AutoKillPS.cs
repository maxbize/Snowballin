using UnityEngine;
using System.Collections;

public class AutoKillPS : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        Invoke("Die", ps.startLifetime); // Needs to be updated for other Particle Systems
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Die() {
        Destroy(gameObject);
    }
}
