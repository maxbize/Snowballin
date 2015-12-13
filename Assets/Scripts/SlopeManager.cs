using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlopeManager : MonoBehaviour {

    // Set in Editor
    public SlopeSliceGenerator sliceGen;
    public Player player;
    public ObstacleManager obstacleManager;

    private SlopeSlice playerSlice;
    public Vector3 toGround { get; private set; } // This doesn't really belong here...
    public Vector3 alongGround { get; private set; } // This doesn't really belong here...
    private Queue<SlopeSlice> slices = new Queue<SlopeSlice>();
    private Dictionary<Int2, SlopeSlice> sliceMap = new Dictionary<Int2, SlopeSlice>();
    private Rigidbody playerRb;

	// Use this for initialization
	void Start () {
        toGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle + 90, 0, 0) * Vector3.forward;
        alongGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle, 0, 0) * Vector3.forward;
        playerRb = player.GetComponent<Rigidbody>();

        GenSlice(new Int2(0, 0));
	}
	
	// Update is called once per frame
	void Update () {

        // Check if we reached a new slice and need to generate the next ones
        if (playerSlice != player.currentSlice) {
            playerSlice = player.currentSlice;
            int recursion = Mathf.Clamp(Mathf.CeilToInt(playerRb.velocity.magnitude / 8), 2, 5);
            GenSurroundingSlices(playerSlice.pos, recursion);
            //Debug.Log("Generated at player speed " + playerRb.velocity.magnitude + " with recursion " + recursion);
        }
    }

    private void GenSurroundingSlices(Int2 origin, int recursion) {
        if (recursion == 0) {
            return;
        }
        if (FindSlice(origin + Int2.front) == null) {
            GenSlice(origin + Int2.front);
        }
        if (FindSlice(origin + Int2.right) == null) {
            GenSlice(origin + Int2.right);
        }
        if (FindSlice(origin + Int2.left) == null) {
            GenSlice(origin + Int2.left);
        }

        
        // Easier to traverse graph if you delay recursion
        GenSurroundingSlices(origin + Int2.front, recursion - 1);
        GenSurroundingSlices(origin + Int2.right, recursion - 1);
        GenSurroundingSlices(origin + Int2.left, recursion - 1);
    }

    // pos just needs to be somewhere on the slice. Generates a slice if none found
    private void GenSlice(Int2 pos) {
        SlopeSlice oldestSlice = null;
        if (slices.Count > 0) {
            oldestSlice = slices.Peek();
            if ((oldestSlice.transform.position - player.transform.position).magnitude > 20) {
                slices.Dequeue();
                sliceMap.Remove(oldestSlice.pos);
                obstacleManager.HandleRecycledSlice(oldestSlice);
            } else {
                oldestSlice = null;
            }
        }
        SlopeSlice newSlice = sliceGen.MakeMeshSlice(oldestSlice, FindSlice(pos + Int2.left), FindSlice(pos + Int2.back), FindSlice(pos + Int2.right));
        slices.Enqueue(newSlice);
        sliceMap[pos] = newSlice;
        newSlice.pos = pos;

        obstacleManager.HandleNewSlice(newSlice);
    }

    public SlopeSlice FindSlice(Vector3 origin, float distance) {
        RaycastHit hit;
        if (Physics.Raycast(origin, toGround * distance, out hit)) {
            return hit.transform.GetComponent<SlopeSlice>();
        } else if (Physics.Raycast(origin, -1 * (toGround * distance), out hit)) {
            return hit.transform.GetComponent<SlopeSlice>();
        }
        return null;
    }

    public SlopeSlice FindSlice(Int2 pos) {
        if (sliceMap.ContainsKey(pos)) {
            return sliceMap[pos];
        }
        return null;
    }
}
