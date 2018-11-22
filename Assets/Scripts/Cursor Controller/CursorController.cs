using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        ControlCursor();

    }

    void ControlCursor() {
	    if (Input.GetKeyDown(KeyCode.Tab)) {
		    Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
		    Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
	    }
    }

}
