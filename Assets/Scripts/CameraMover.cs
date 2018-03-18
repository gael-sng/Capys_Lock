using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    private CameraMovement camScript;
    private Vector3 initialTouchPosition;
    private Vector3 initialCameraPosition;

    private void Start()
    {
        camScript = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            initialTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialTouchPosition.z = 0;
            initialCameraPosition = camScript.transform.position;
            initialCameraPosition.z = 0;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0;
            Vector3 offset = currentPosition - initialTouchPosition;
            offset.z = 0;
            Vector2 newCameraPosition = initialCameraPosition - offset;

            camScript.setPosition(newCameraPosition);
        }
        
	}
}
