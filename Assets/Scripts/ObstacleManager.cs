using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

    // Set in editor
    public List<GameObject> obstaclePrefabs;
    public SlopeSliceGenerator sliceGen;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    // New slice spawn
    public void HandleNewSlice(SlopeSlice slice) {
        for (int i = 0; i < 3; i++) {
            GameObject obstacle = (GameObject)Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)]);
            obstacle.transform.parent = slice.transform;
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(0f, sliceGen.sliceWidth);
            pos.y = 1f / 2f;
            pos.z = Random.Range(0f, sliceGen.sliceLength);
            obstacle.transform.localPosition = pos;            
            obstacle.transform.localScale *= Random.Range(1, 4);
            obstacle.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }

    // Clear all obstacles when recycling
    public void HandleRecycledSlice(SlopeSlice slice) {
        foreach (Obstacle obstacle in slice.gameObject.GetComponentsInChildren<Obstacle>()) {
            Destroy(obstacle.gameObject);
        }
    }
}
