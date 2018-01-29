using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroBehaviour : MonoBehaviour {
	public float force;

	private Rigidbody2D m_rigid;
	// Use this for initialization
	void Start () {
		m_rigid = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) {
			m_rigid.gravityScale = 1;
			m_rigid.AddForce (transform.up * force);
		}
	}
}
