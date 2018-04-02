using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(BoxCollider2D))]

public class PseudoBlock: MonoBehaviour {
	enum _PseudoStates {Selected, Unselected};

	[SerializeField] _PseudoStates _MyState = _PseudoStates.Unselected;

	private BoxCollider2D _myColl;
	private GameObject _UI;
	private Vector3 _Destination;
	private Vector3 _RelativePosition;
	private int CollisionCount = 0;

	private PseudoHandle _PHandle;


	public List<SpawnTileBehaviour> _ListOfTiles;

	void OnMouseUp(){
		Debug.Log (gameObject.name + " MORRA");
		for (int i = 0; i < _ListOfTiles.Count; i++) {
			//_ListOfTiles [i]._IsOccupied = false;
			_ListOfTiles [i].gameObject.SetActive (true);
		}
		Destroy (gameObject);

	}

	void Start () {
		_myColl = gameObject.GetComponent<BoxCollider2D> ();
		_PHandle = FindObjectOfType<PseudoHandle> ();
		_PHandle._PseudoBlockList.Add(this);
	}

	public void StartPlay(){
		GetComponent<Destructible> ().enabled = true;
		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1f;
		_myColl.isTrigger = false;
		_myColl.size = new Vector2(_myColl.size.x + 0.1f, _myColl.size.y + 0.1f);
		gameObject.tag = "Destructible";
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
		_Destination = transform.position;
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
		float newX;
		float newY;
		if (transform.rotation.eulerAngles.z >= 1) {
			newX = (float)((Math.Round (pos.x))) + (_myColl.size.y + 0.1f) / 2 - 0.5f;
			newY = (float)((Math.Round (pos.y))) + (_myColl.size.x + 0.1f) / 2 - 0.5f;
		} else {
			newX = (float)((Math.Round (pos.x))) + (_myColl.size.x + 0.1f) / 2 - 0.5f;
			newY = (float)((Math.Round (pos.y))) + (_myColl.size.y + 0.1f) / 2 - 0.5f;
		}

		return new Vector3 (newX, newY, pos.z);
	}

	void OnTriggerEnter2D(Collider2D coll){
		CollisionCount++;
	}
	void OnTriggerExit2D(Collider2D coll){
		CollisionCount--;
	}
}