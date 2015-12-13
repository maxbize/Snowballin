using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    // Set in editor
    public GameObject player;
    public GameObject gui;
    public GameObject controlsUi;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (player.activeSelf) {
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            player.SetActive(true);
            gui.SetActive(false);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            controlsUi.SetActive(!controlsUi.activeSelf);
        }
	}
}
