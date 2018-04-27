using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class Destructible : MonoBehaviour {
	[SerializeField] float _MaxLife;
	[SerializeField] float _ActualLife;
	[SerializeField] float _DamageThreshold;
	private Rigidbody2D _mRigid;
	void Start () {
		//parte que calcula a massa e a vida da tabua proporcionalmente ao tamanho da madeira
		if (name.Contains ("madeira")) {
			_mRigid = GetComponent<Rigidbody2D> ();
			_MaxLife *= (int)char.GetNumericValue (name [7]) * (int)char.GetNumericValue(name[9]);
			_mRigid.mass *= (int)char.GetNumericValue (name [7]) * (int)char.GetNumericValue(name[9]);
		}
		if (_MaxLife <= 0)
			_MaxLife = 100;
		_ActualLife = _MaxLife;
	}

	public void TakeDamage(float damage){
		_ActualLife -= damage;
		if (_ActualLife <= 0) {
			Destroy (this.gameObject);
		}
	}

    public float getMaxLife() {
        return _MaxLife;
    }

    public float getActualLife()
    {
        return _ActualLife;
    }

	void OnCollisionEnter2D(Collision2D coll){
		if (gameObject.CompareTag("Destructible")) {
			// Aplicando dano, forma ainda não final
			float damage = coll.relativeVelocity.magnitude;
			if (damage > _DamageThreshold) {
				if(gameObject.name.Contains("madeira"))Debug.Log (coll.otherCollider.name + " other:" + coll.collider.name + " relativeVelocity:" + coll.relativeVelocity.magnitude);
				TakeDamage (damage);			
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}