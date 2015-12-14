using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    // Set in editor
    public GameObject player;
    public GameObject mainMenuUi;
    public GameObject controlsUi;
    public GameObject endGameUi;
    public Text countText;

    // Yeah... let's just throw the kitchen sink in here... 3 hours to go til submission!!!
    private bool gameStarted = false;
    private bool gameEnded = false;
    private bool allObstaclesBlasted = false;
    private int numRocks = 0;
    private int numCrystals = 0;
    private int numTrees = 0;
    private float blastSpeed;
    private Player playerScript;

	// Use this for initialization
	void Start () {
        mainMenuUi.SetActive(true);
        endGameUi.SetActive(false);
        playerScript = player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameStarted) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                gameStarted = true;
                player.SetActive(true);
                mainMenuUi.SetActive(false);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                controlsUi.SetActive(!controlsUi.activeSelf);
            }
        } else if (gameEnded) {
            countText.text = string.Format("Rocks: {0}\nCrystals: {1}\nTrees: {2}", numRocks, numCrystals, numTrees);
        }
	}

    public void HandleVictory() {
        if (!gameEnded) {
            blastSpeed = 5f / player.GetComponentsInChildren<Obstacle>().Length;
            endGameUi.SetActive(true);
            gameEnded = true;
            Debug.Log("Handling Victory");
            BlastNext();
        }
    }

    private void BlastNext() {
        Obstacle next = player.GetComponentInChildren<Obstacle>();
        if (next != null) {
            if (next.name.Contains("Rock")) {
                numRocks++;
            } else if (next.name.Contains("Crystal")) {
                numCrystals++;
            } else if (next.name.Contains("Tree")) {
                numTrees++;
            } else {
                Debug.LogError("Update me, fool!");
            }
            next.CheckDetach(true);
            Invoke("BlastNext", blastSpeed);
            playerScript.PlayGrabSound();
        } else {
            allObstaclesBlasted = true;
            Debug.Log("All obstacles blasted");
        }
    }
}
