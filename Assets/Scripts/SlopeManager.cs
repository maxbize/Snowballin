using UnityEngine;
using System.Collections;

public class SlopeManager : MonoBehaviour {

    // Set in Editor
    public SlopeSliceGenerator sliceGen;
    public PlayerController player;

    private SlopeSlice playerSlice;
    public Vector3 toGround { get; private set; } // This doesn't really belong here...

	// Use this for initialization
	void Start () {
        toGround = Quaternion.Euler(FindObjectOfType<SlopeSliceGenerator>().slopeAngle + 90, 0, 0) * Vector3.forward;
        sliceGen.MakeMeshSlice(null, null, null, null);
	}
	
	// Update is called once per frame
	void Update () {
	    // There should always be slope in a cone in front of the player
        GenSlice(player.transform.position + Vector3.forward * 10);
    }

    // pos just needs to be somewhere on the slice. Generates a slice if none found
    private void GenSlice(Vector3 pos) {
        if (FindSlice(pos, 100) == null) {
            sliceGen.MakeMeshSlice(null, null, player.currentSlice, null);
        }
    }

    public SlopeSlice FindSlice(Vector3 origin, float distance) {
        RaycastHit hit;
        if (Physics.Raycast(origin, toGround * distance, out hit)) {
            return hit.transform.GetComponent<SlopeSlice>();
        }
        return null;
    }

    // Get the slice that's offset from the focus, if it exists
    private void GetSlice(SlopeSlice focus, Vector3 offset) {

    }

}
