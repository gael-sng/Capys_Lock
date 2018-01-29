using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]

public class PseudoBlock: MonoBehaviour {
	enum _PseudoStates {Selected, Unselected};

	[SerializeField] _PseudoStates _MyState = _PseudoStates.Unselected;

	private GameObject _UI;
	private Vector3 _Destination;
	private Vector3 _RelativePosition;
	private int CollisionCount = 0;

	public void StartPlay(){
		GetComponent<Destructible> ().enabled = true;
		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1f;
		BoxCollider2D col = gameObject.GetComponent<BoxCollider2D> ();
		col.isTrigger = false;
		col.size = Vector2.one;
		this.enabled = false;
	}

	public void _SetUI(GameObject newUI){
		_UI = newUI;
	}

	public void _Select(){
		_MyState = _PseudoStates.Selected;
		_Destination = transform.position;
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch(0);
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

			_RelativePosition = transform.position - touchPosition;
		}
		_UI.SetActive (false);
	}

	public void _UnSelect(){
		_MyState = _PseudoStates.Unselected;
		_UI.SetActive (true);
	}

	public bool _CanUnSelect(){
		if (CollisionCount == 0 && _MyState == _PseudoStates.Selected)
			return true;
		Debug.Log ("NÂO PODE");
		return false;
	}

	void Update(){
		if(_MyState == _PseudoStates.Selected)_Move ();
	}


	void _Move(){
		if (Input.touchCount == 1  && Input.GetTouch(0).fingerId == 0){
			Touch touch = Input.GetTouch(0);
			Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
			if(touch.phase == TouchPhase.Began) 
				_RelativePosition =  transform.position - touchPosition;
			if (touch.phase == TouchPhase.Moved){
				_Destination = touchPosition + _RelativePosition;
			}
			transform.position = _MakeSnap(_Destination);
		}
	}

	Vector3 _MakeSnap(Vector3 pos){
		//Arredondando a posição;
		float newX = (float)( (Math.Round(pos.x))) + transform.localScale.x/2 - 0.5f;
		float newY = (float)( (Math.Round(pos.y))) + transform.localScale.y/2 - 0.5f;

		return new Vector3 (newX, newY, pos.z);
	}

	void OnTriggerEnter2D(Collider2D coll){
		Debug.Log ("eu '" + gameObject.name + "' colidi com o '" + coll.gameObject.name + "'");
		CollisionCount++;
	}
	void OnTriggerExit2D(Collider2D coll){
		Debug.Log ("eu '" + gameObject.name + "' descolidi com o '" + coll.gameObject.name + "'");
		CollisionCount--;
	}
}