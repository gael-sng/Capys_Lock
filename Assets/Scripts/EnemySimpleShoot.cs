using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleShoot : MonoBehaviour {

    public float maxVelocity = 20f; //We chose an airtime so we can find a unique solution to the problem

    public GameObject shootPrefab;

    public Vector3 target;

    public Transform spawnPosition;

    public Transform rotationTransform;

    public bool shouldRotate;

    [HideInInspector]
    public Transform projectile;

    private Transform targetObj;

    private Quaternion shootRotation = Quaternion.identity;

    private bool isRight = false;

    private float airTime = 0;
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
        isRight = (target - transform.position).x < 0;


        //Convert position from touch to world
        Transform toCalculate = (rotationTransform == null) ? transform : rotationTransform;
        //Vector2 newVelocity = calculateParabola(toCalculate.position, target);
        //rotateTranform(newVelocity);
        Vector2 newVelocity = calculateParabola(spawnPosition.position, target);


        Transform toSpawn = (spawnPosition != null) ? spawnPosition : transform;
        GameObject proj = GameObject.Instantiate(shootPrefab, toSpawn.position, shootRotation, transform.parent);
        Rigidbody2D rb2d = proj.GetComponent<Rigidbody2D>();
        rb2d.velocity = newVelocity;

        projectile = proj.transform;

        if (targetObj != null)
            targetObj.position = target;
        return proj;
    }

    private Vector2 calculateParabola(Vector3 initPos, Vector3 finalPos) {
        target = finalPos;

        float g = Physics2D.gravity.y;
		float dx = (finalPos.x - initPos.x);
		float dy = (finalPos.y - initPos.y);
        
        double to = Mathf.Sqrt((dx*dx+dy*dy)/(maxVelocity*maxVelocity));
		airTime = (float)to;

        return calculateVelocity(to, dx, dy);
    }

    private Vector2 calculateVelocity(double to, float dx, float dy){
        double vx = dx/to;
        double vy = (dy/to) - (Physics.gravity.y * to / 2);
        return new Vector2( (float) vx, (float) vy);  
    }

    public float getFlyTime() {
        //return airTime;
        return airTime;
    }

    private void rotateTranform(Vector2 direction) {
        if (rotationTransform == null || !shouldRotate)
            return;
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;
        float projectTileAngle = -Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;
        if (isRight)
            angle *= -1;
        rotationTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        shootRotation = Quaternion.Euler(new Vector3(0, 0, projectTileAngle));

    }
}
