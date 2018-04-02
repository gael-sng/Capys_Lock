using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    private CameraMovement camScript;
    private Vector3 initialTouchPosition;
    private Vector3 initialCameraPosition;
    private Vector3 screenInitialPosition;

    private void Start()
    {
        camScript = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            screenInitialPosition = Input.mousePosition;
            initialCameraPosition = camScript.transform.position;
            initialCameraPosition.z = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 worldCurrentScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 worldInitialTouch = Camera.main.ScreenToWorldPoint(screenInitialPosition);
            Vector3 delta = worldCurrentScreen - worldInitialTouch;
            Vector2 newCameraPosition = initialCameraPosition - delta;

            camScript.setPosition(newCameraPosition);
        }
        
	}
}
