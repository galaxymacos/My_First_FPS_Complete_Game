using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject FpsPlayer;

    private CursorController cursorController;

    // Use this for initialization
    void Start () {
        cursorController = gameObject.GetComponent<CursorController>();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale == 1) Time.timeScale = 0;
            else
            {
                Time.timeScale = 1;
            }

            cursorController.ToggleCursor();
            pauseScreen.SetActive(!pauseScreen.activeSelf);
            FpsPlayer.SetActive(!FpsPlayer.activeSelf);
            
        }
	}
}
