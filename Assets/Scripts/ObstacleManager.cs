using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

    // Set in editor
    public List<GameObject> obstaclePrefabs;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    // 
    public void HandleNewSlice(SlopeSlice slice) {
        GameObject obstacle = (GameObject)Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)]);
        obstacle.transform.parent = slice.transform;
        obstacle.transform.localPosition = Vector3.up / 2;
        obstacle.transform.localScale *= Random.Range(1, 4);
    }

    // Clear all obstacles when recycling
    public void HandleRecycledSlice(SlopeSlice slice) {
        foreach (Obstacle obstacle in slice.gameObject.GetComponentsInChildren<Obstacle>()) {
            Destroy(obstacle.gameObject);
        }
    }
}
