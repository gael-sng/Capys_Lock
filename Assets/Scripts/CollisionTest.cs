using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour {
	public Vector2 force;
	public float torque;

	private Rigidbody2D _mRigid;
	private Vector2 _LastVelocity;
	private Vector2 _ActualVelocity;
	// Use this for initialization
	void Start () {
		_mRigid = GetComponent<Rigidbody2D> ();
		_LastVelocity = _mRigid.velocity;

	}
	
	// Update is called once per frame
	void Update () {
		_LastVelocity = _ActualVelocity;
		_ActualVelocity = _mRigid.velocity;
		if (Input.GetKeyDown (KeyCode.Space)) {
			_mRigid.AddForce (force);
			_mRigid.AddTorque (torque);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		//_mRigid.velocity = Vector2.zero;
		Debug.Log ("Me: " + coll.otherCollider.name + "Actual:" + _mRigid.velocity + " Last:" + _LastVelocity + " Relativity: " + coll.relativeVelocity + 
			"\nDebug: _mRigid.velocity:" + _mRigid.velocity + " coll.colider.name:" + coll.collider.name);
	}
}
