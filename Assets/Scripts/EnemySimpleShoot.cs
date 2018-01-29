using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleShoot : MonoBehaviour {

    public float airTime = 1f; //We chose an airtime so we can find a unique solution to the problem

    public GameObject shootPrefab;

    public Vector3 target;

    private Transform targetObj;

	// Use this for initialization
	void Start () {
        targetObj = transform.Find("Target");
	}
	
    //Debug and test
    /*
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)) { 
            //Instanciate projectile
            GameObject proj = GameObject.Instantiate(shootPrefab, transform.position, Quaternion.identity,transform.parent);
            Rigidbody2D rb2d = proj.GetComponent<Rigidbody2D>();
            //Convert position from touch to world
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            Vector2 newVelocity = calculateParabola(transform.position, target);
            rb2d.velocity = newVelocity;
            if (target != null)
                targetObj.position = target;
        }
	}
    */

    /// <summary>
    /// Shoot the projectile at a given target
    /// </summary>
    /// <param name="target">Position which is going to shoot</param>
    /// <return> The gameobject of the projectile </return>
    public GameObject ShootAtTarget(Vector3 target) {
        //Instanciate projectile
        GameObject proj = GameObject.Instantiate(shootPrefab, transform.position, Quaternion.identity,transform.parent);
        Rigidbody2D rb2d = proj.GetComponent<Rigidbody2D>();
        //Convert position from touch to world
        Vector2 newVelocity = calculateParabola(transform.position, target);
        rb2d.velocity = newVelocity;
        if (targetObj != null)
            targetObj.position = target;
        return proj;
    }

    private Vector2 calculateParabola(Vector3 initPos, Vector3 finalPos) {
        target = finalPos;

        float g = Physics2D.gravity.y;
        float vx = (finalPos.x - initPos.x)/airTime;
        float vy = (finalPos.y - initPos.y) / airTime - (g / 2) * airTime;

        return new Vector2(vx, vy);
    }

    public float getFlyTime() {
        return airTime;
    }
}
