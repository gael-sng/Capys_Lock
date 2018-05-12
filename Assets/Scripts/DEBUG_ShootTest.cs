using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_ShootTest : MonoBehaviour {

	public EnemySimpleShoot Enemy;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			position.z = 0;
			if(Enemy != null)
				Enemy.ShootAtTarget(position);
			transform.position = position;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Rigidbody2D rb2d = other.GetComponent<Rigidbody2D>();
		if(rb2d != null){
			Debug.Log("Alvo com velocidade: "+ rb2d.velocity.magnitude);
		}
	}
}
