using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMissile : MonoBehaviour {

	private Transform myTransform;
	private Rigidbody2D rb2D;

	// Use this for initialization
	void Awake () {
        //myTransform = transform.Find("Sprite");
        myTransform = transform;
		rb2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 velocity = rb2D.velocity;
		float angle = Mathf.Atan2(velocity.y, velocity.x);
		angle *= Mathf.Rad2Deg;
		myTransform.rotation = Quaternion.Euler(0,0,angle);
	}
}
