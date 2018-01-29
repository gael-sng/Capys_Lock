using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class Destructible : MonoBehaviour {
	[SerializeField] float _MaxLife = 100;
	[SerializeField] float _ActualLife;
	[SerializeField] float _DamageThreshold = 10;

	void Start () {
		_ActualLife = _MaxLife;
		
	}

	public void TakeDamage(float damage){
		_ActualLife -= damage;
		if (_ActualLife <= 0) {
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Destructible") {
			// Aplicando dano, forma ainda não final
			float damage = coll.relativeVelocity.magnitude * coll.rigidbody.mass;
			if(damage > _DamageThreshold)coll.gameObject.GetComponent<Destructible> ().TakeDamage (damage);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}