using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Capivara : MonoBehaviour {

	AttackSchedule attackSchedule;

	// Use this for initialization
	void Start () {
		GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
		attackSchedule = gameManager.GetComponent<AttackSchedule>();
		attackSchedule.addCapivara(this);
	}
	
	private void OnDestroy() {
		attackSchedule.removeCapivara(this);
	}

	public void StartPlay(){
		GetComponent<Destructible> ().enabled = true;
		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1f;
		CircleCollider2D col = gameObject.GetComponent<CircleCollider2D> ();
		col.isTrigger = false;
		col.radius = 1;
	}
}
