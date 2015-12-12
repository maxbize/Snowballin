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

	// Use this for initialization
	void Start () {
        toGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle + 90, 0, 0) * Vector3.forward;
        alongGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle, 0, 0) * Vector3.forward;
        slices.Enqueue(sliceGen.MakeMeshSlice(null, null, null, null));
	}
	
	// Update is called once per frame
	void Update () {
	    // There should always be slope in a cone in front of the player
        GenSlice(player.transform.position + alongGround * sliceGen.sliceLength);
        GenSlice(player.transform.position + Vector3.right * sliceGen.sliceWidth);
        GenSlice(player.transform.position - Vector3.right * sliceGen.sliceWidth);
    }

    // pos just needs to be somewhere on the slice. Generates a slice if none found
    private void GenSlice(Vector3 pos) {
        if (FindSlice(pos, 100) == null) {
            SlopeSlice left = FindSlice(pos - Vector3.right * sliceGen.sliceWidth, 100);
            SlopeSlice back = FindSlice(pos - alongGround * sliceGen.sliceLength, 100);
            if (back == null) {
                back = FindSlice(pos - alongGround * sliceGen.sliceLength * 0.5f, 100);
            }
            SlopeSlice right = FindSlice(pos + Vector3.right * sliceGen.sliceWidth, 100);

            SlopeSlice oldestSlice = slices.Peek();
            if ((oldestSlice.transform.position - player.transform.position).magnitude > 20) {
                slices.Dequeue();
            } else {
                oldestSlice = null;
            }
            slices.Enqueue(sliceGen.MakeMeshSlice(oldestSlice, left, back, right));
        }
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
