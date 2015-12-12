using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlopeManager : MonoBehaviour {

    // Set in Editor
    public SlopeSliceGenerator sliceGen;
    public PlayerController player;

    private SlopeSlice playerSlice;
    public Vector3 toGround { get; private set; } // This doesn't really belong here...
    public Vector3 alongGround { get; private set; } // This doesn't really belong here...
    private Queue<SlopeSlice> slices = new Queue<SlopeSlice>();

    public enum Dir
    {
        LEFT,
        RIGHT,
        FRONT,
        BACK
    }

	// Use this for initialization
	void Start () {
        toGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle + 90, 0, 0) * Vector3.forward;
        alongGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle, 0, 0) * Vector3.forward;
        slices.Enqueue(sliceGen.MakeMeshSlice(null, null, null, null));
	}
	
	// Update is called once per frame
	void Update () {

        // Check if we reached a new slice and need to generate the next ones
        if (playerSlice != player.currentSlice) {
            playerSlice = player.currentSlice;
            GenSurroundingSlices(playerSlice, 2);
        }
    }

    private void GenSurroundingSlices(SlopeSlice origin, int recursion) {
        if (recursion == 0) {
            return;
        }
        if (origin.frontSlice == null) {
            SlopeSlice leftSlice = origin.leftSlice == null ? null : origin.leftSlice.frontSlice;
            SlopeSlice rightSlice = origin.rightSlice == null ? null : origin.rightSlice.frontSlice;
            GenSlice(leftSlice, origin, rightSlice);
        }
        if (origin.rightSlice == null) {
            SlopeSlice backSlice = origin.backSlice == null ? null : origin.backSlice.rightSlice;
            GenSlice(origin, backSlice, null);
            GenSurroundingSlices(origin.rightSlice, recursion - 1);
        }
        if (origin.leftSlice == null) {
            SlopeSlice backSlice = origin.backSlice == null ? null : origin.backSlice.leftSlice;
            GenSlice(null, backSlice, origin);
            GenSurroundingSlices(origin.leftSlice, recursion - 1);
        }
        
        // Easier to traverse graph if you delay recursion
        GenSurroundingSlices(origin.frontSlice, recursion - 1);
        GenSurroundingSlices(origin.rightSlice, recursion - 1);
        GenSurroundingSlices(origin.leftSlice, recursion - 1);
    }

    // pos just needs to be somewhere on the slice. Generates a slice if none found
    private void GenSlice(SlopeSlice left, SlopeSlice back, SlopeSlice right) {
        SlopeSlice oldestSlice = slices.Peek();
        if ((oldestSlice.transform.position - player.transform.position).magnitude > 20) {
            slices.Dequeue();
        } else {
            oldestSlice = null;
        }
        slices.Enqueue(sliceGen.MakeMeshSlice(oldestSlice, left, back, right));
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
}
