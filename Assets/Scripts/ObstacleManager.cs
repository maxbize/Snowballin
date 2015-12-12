using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour {

    // Set in editor
    public GameObject obstaclePrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // 
    public void HandleNewSlice(SlopeSlice slice) {
        GameObject obstacle = (GameObject)Instantiate(obstaclePrefab);
        obstacle.transform.parent = slice.transform;
        obstacle.transform.localPosition = Vector3.up;
    }

    // Clear all obstacles when recycling
    public void HandleRecycledSlice(SlopeSlice slice) {
        foreach (Obstacle obstacle in slice.gameObject.GetComponentsInChildren<Obstacle>()) {
            Destroy(obstacle.gameObject);
        }
    }
}
