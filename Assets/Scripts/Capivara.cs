using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Capivara : MonoBehaviour {

    FlexAttack flexAttack;
    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
		GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        flexAttack = gameManager.GetComponent<FlexAttack>();
        if (flexAttack != null)
            flexAttack.addCapivara(this);
	}
	
	private void OnDestroy() {
		flexAttack.removeCapivara(this);
	}

	public void StartPlay(){
		GetComponent<Destructible> ().enabled = true;
		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1f;
		CircleCollider2D col = gameObject.GetComponent<CircleCollider2D> ();
		col.isTrigger = false;
		col.radius = 1;
        startRolling();
	}

    public void startRolling() {
        if(animator != null)
            animator.SetTrigger("Roll");
    }
}
