using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

    // Set in editor
    public List<GameObject> obstaclePrefabs;
    public SlopeSliceGenerator sliceGen;
    public GameObject rockPrefab;
    public GameObject player;

    private int numScriptedSlices = 0; // Gen the same obstacle for all slices
    private int scriptIdx; // Which prefab we're generating
    private int scriptScale;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    // New slice spawn
    public void HandleNewSlice(SlopeSlice slice) {
        if (numScriptedSlices == 0) {
            int roll = Random.Range(0, 100);
            if (roll < 10) {
                numScriptedSlices = roll * 2 + 15;
                scriptScale = Random.Range(1, 4);
                scriptIdx = Random.Range(0, obstaclePrefabs.Count);
            }
            for (int i = 0; i < 3; i++) {
                CreateObstacle(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], Random.Range(1, 4), slice);
                if (player.transform.localScale.x > 1) {
                    CreateObstacle(rockPrefab, 1, slice); // Spawn a small rock as well to help the player out
                }
            }
        } else {
            numScriptedSlices--;
            for (int i = 0; i < 3; i++) {
                CreateObstacle(obstaclePrefabs[scriptIdx], scriptScale, slice);
            }
        }
    }

    private void CreateObstacle(GameObject prefab, int scale, SlopeSlice slice) {
        GameObject obstacle = (GameObject)Instantiate(prefab);
        obstacle.transform.parent = slice.transform;
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(0f, sliceGen.sliceWidth);
        pos.z = Random.Range(0f, sliceGen.sliceLength);
        pos.y += sliceGen.heightFunction(pos.x + slice.transform.position.x, pos.z + slice.transform.position.z);
        obstacle.transform.localPosition = pos;
        obstacle.transform.localScale *= scale;
        obstacle.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }

    // Clear all obstacles when recycling. Could optimize this game by using object pooling here instead of destroying
    public void HandleRecycledSlice(SlopeSlice slice) {
        foreach (Obstacle obstacle in slice.gameObject.GetComponentsInChildren<Obstacle>()) {
            Destroy(obstacle.gameObject);
        }
    }
}
