using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

	// Use this for initialization
	private CameraMovement camScript;
	private Transform myTransform;
	void Awake () {
		myTransform = transform;
		camScript = Camera.main.GetComponent<CameraMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			if(camScript != null)
				camScript.targetPoint(myTransform);
		}

		if(Input.GetMouseButton(1)) {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;
			myTransform.position = mousePosition;
		}
	}
}
