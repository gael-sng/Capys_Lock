using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAmmo : MonoBehaviour {

	public float maxExplosionForce = 10f;
	public float maxExplosionDamage = 5f;
	public float explosionRange = 2f;
	private Transform myTransform;

	private void Awake() {
		myTransform = transform;
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag.Equals("Destructible")) {
			Collider2D[] hits;
			hits = Physics2D.OverlapCircleAll(myTransform.position, explosionRange);
			foreach(Collider2D hit in hits) {
				print("Explodiu: " +hit.gameObject.name);
				if(hit.gameObject.tag.Equals("Destructible")) {
					Vector3 direction = hit.transform.position - myTransform.position;
					float distance = direction.magnitude;
					float realForce = maxExplosionForce - (maxExplosionForce/explosionRange) * distance;
					float realDamage = maxExplosionDamage - (maxExplosionDamage/explosionRange) * distance;

					Destructible dest = hit.GetComponent<Destructible>();
					if(dest != null)
						dest.TakeDamage(realDamage);

					Rigidbody2D rb2d = hit.GetComponent<Rigidbody2D>();
					if(rb2d != null)
						rb2d.AddForce(direction.normalized * realForce, ForceMode2D.Impulse);
				}
			}
			Destroy(gameObject);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawSphere(transform.position, explosionRange);
	}

}
