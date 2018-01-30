using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneShot : MonoBehaviour {

	public GameObject bulletPrefab;
	public float horizontalSpeed = 5f;
	public float throwPosition;
	public float destroyTime = 5f;
	private float positionThreshold = 0.1f;
	private bool hasThrow = false;
	private bool canFly = false;
	private Transform myTransform;
	//Track flying time
	private float startTime;
	private Rigidbody2D rb2D;
	public Vector3 target;


	private void Awake() {
		myTransform = transform;
		positionThreshold = horizontalSpeed * 0.05f;
		rb2D = GetComponent<Rigidbody2D>();
		rb2D.gravityScale = 0;
	}

	public void AttackTarget(Vector3 target) {
		rb2D.velocity = new Vector2(horizontalSpeed,0);
		startTime = Time.time;
		calculateThrow(target);
		canFly = true;
	}
	public void calculateThrow(Vector3 target) {
		this.target = target;
		float dy = (transform.position.y - target.y);
		print("Dy="+dy);
        print("Posicao: " + transform.position);
		float airTime = Mathf.Sqrt(dy * 2 / -Physics2D.gravity.y );
		throwPosition = target.x - horizontalSpeed * airTime;
        print("Alvo: " + throwPosition);
	}

	private void Update() {
		if(canFly && !hasThrow) {
			if(Mathf.Abs( myTransform.position.x - throwPosition) < positionThreshold) {
				GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity, myTransform.parent);
				Rigidbody2D bulletrb2d = bullet.GetComponent<Rigidbody2D>();
				bulletrb2d.velocity = new Vector2(horizontalSpeed, 0);
				hasThrow = true;
			}
		}
		if(canFly && (Time.time - startTime) >= destroyTime) {
			print("Destruiu. Tempo inicial: "+ startTime + " Tempo atual:" + Time.time);
			Destroy(gameObject);
		}
		/* DEBUG
		if(canSpawnMore && Input.GetMouseButtonDown(0)) {
			GameObject newCopy = Instantiate(gameObject, myTransform.position, Quaternion.identity);
			Rigidbody2D rb2d = newCopy.GetComponent<Rigidbody2D>();
			rb2d.velocity = new Vector2(horizontalSpeed,0);
			rb2d.gravityScale = 0;
			PlaneShot ps = newCopy.GetComponent<PlaneShot>();
			Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			ps.calculateThrow(target);
			ps.canSpawnMore = false;
		}
		*/
	}

	public float getFlyTime(){
		return (target.x - myTransform.position.x)/horizontalSpeed;
	}
}
