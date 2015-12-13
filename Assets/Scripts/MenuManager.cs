using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    // Set in editor
    public GameObject player;
    public GameObject gui;
    public GameObject controlsUi;

    // Yeah... let's just throw the kitchen sink in here... 3 hours to go til submission!!!
    private bool gameStarted = false;
    private bool gameEnded = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameStarted) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                player.SetActive(true);
                gui.SetActive(false);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                controlsUi.SetActive(!controlsUi.activeSelf);
            }
        } else if (gameEnded) {

        }
	}

    public void HandleVictory() {
        gameEnded = true;
    }
}
