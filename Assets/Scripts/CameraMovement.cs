﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

	private Transform myTransform;
	private Vector3 zOffset;


    //Persistent follow variables
    public bool canStopAnimation = false;
	public float smoothTime = 0.5f;
	public float maxSpeed = 10f;
	private Transform perpetualTarget;
	private Vector3 velocity = Vector3.zero;
	private bool shouldFollow;
	private float squaredMaxVelocity;

    //Follow variables
    public float cameraZoomRecover = 2f;
	private bool isFollowingAnimation = false;
	private float followCurrentTime = 0f;
	private Transform followTimeTarget;
	private float followSpeed = 5f;

	//Go to Variables
	private bool isGoAnimation = false;
	private Vector3 goSpeed;
	private float goCurrentTime = 0f;

	//Pinch zoom variables
	[Header("Pinch zoom variables")]
	public bool enablePinchZoom = true;
	public float minimumCameraSize = 5f;
	public float maximumCameraSize = 20f;
	public float zoomSpeed = 0.5f;
	private Slider slider;
	private float initialCameraSize;
	private float relativeInitialDistance = 0f;
	private Camera myCamera;
    private float startZoom;

	// Use this for initialization
	void Awake () {
		myTransform = transform;
		zOffset = new Vector3(0,0,myTransform.position.z);
		myCamera = GetComponent<Camera>();
        startZoom = myCamera.orthographicSize;
	}

    private void Start()
    {
		slider = GameObject.FindObjectOfType<Slider> ();// ("ZoonSlider").GetComponent<Slider>();
        if (slider != null)
        {
            slider.minValue = minimumCameraSize;
            slider.maxValue = maximumCameraSize;
            slider.value = myCamera.orthographicSize;
        }
    }

    // Update is called once per frame
    void Update () {
		if(isFollowingAnimation) {
			if(followCurrentTime > 0 && followTimeTarget != null) {
				myTransform.position = Vector3.Lerp(myTransform.position, 
					followTimeTarget.position + zOffset, Time.deltaTime * followSpeed);
				followCurrentTime -= Time.deltaTime;
                myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, startZoom, Time.deltaTime * cameraZoomRecover);
			}
			else {
				isFollowingAnimation = false;
			}
		}

		if(isGoAnimation) {
			if(goCurrentTime > 0) {
				myTransform.position += goSpeed * Time.deltaTime;
				goCurrentTime -= Time.deltaTime;
                myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, startZoom, Time.deltaTime * cameraZoomRecover);
            }
			else {
				isGoAnimation = false;
			}
		}

		if(shouldFollow) {
			
			Vector3 aux = Vector3.SmoothDamp(myTransform.position, (perpetualTarget.position ),
			ref velocity, smoothTime, maxSpeed);
			aux.z = -10f;
			myTransform.position= aux;
		}

		if(enablePinchZoom && Input.touchCount > 1) {
			Touch touch1 = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);

			if (touch2.phase == TouchPhase.Began) {
				//Calcula distancia relativa
				initialCameraSize = myCamera.orthographicSize;
				relativeInitialDistance = (touch2.position - touch1.position).magnitude;
			}
			if (touch2.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) {
				float newRelativeDistance = (touch2.position - touch1.position).magnitude;
				//Calc

				float dif = newRelativeDistance - relativeInitialDistance;
				float newCameraSize = initialCameraSize - dif * zoomSpeed;

				newCameraSize = Mathf.Clamp(newCameraSize, minimumCameraSize, maximumCameraSize);

				myCamera.orthographicSize = newCameraSize;
                if(slider != null)
                {
                    slider.value = newCameraSize;
                }
				
			}
		}

        if(slider != null)
        {
            myCamera.orthographicSize = slider.value;
        }

	}

	public void followTargetForTime(Transform target, float timeInterval) {
		followCurrentTime = timeInterval;
		followTimeTarget = target;
		isFollowingAnimation = true;
		shouldFollow = false;
    }

    public void goToLocation(Vector3 target, float timeInterval) {
        if (target != null && myTransform != null)
        {
            goSpeed = (target - myTransform.position) / timeInterval;
            goSpeed.z = 0;
            goCurrentTime = timeInterval;
            isGoAnimation = true;
            shouldFollow = false;
            myCamera.orthographicSize = startZoom;
        }
	}

	public void targetPoint(Transform target) {
        if (canStopAnimation || (!isGoAnimation && !isFollowingAnimation))
        {
            perpetualTarget = target;
            velocity = Vector3.zero;
            shouldFollow = true;
            isFollowingAnimation = false;
            isGoAnimation = false;
        }
	}

    public void addOffset(Vector2 direction)
    {
        Vector3 off = new Vector3(direction.x, direction.y, 0);
        myTransform.position += off;
    }

    public void setPosition(Vector2 position) {
        Vector3 pos = new Vector3(position.x, position.y, 0);
        myTransform.position = pos + zOffset;
    }
}
