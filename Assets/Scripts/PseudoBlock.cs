using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(BoxCollider2D))]

public class PseudoBlock: MonoBehaviour {

	private BoxCollider2D _myColl;
	private bool _WillDie;

	//private PseudoHandle _PHandle;

	public SpawnGridBehaviour _MyGrid;

	//Lista de tiles em que o bloco foi spawnado em cima
	public List<SpawnTileBehaviour> _ListOfTiles;


	void Start () {
		_WillDie = false;
		_myColl = gameObject.GetComponent<BoxCollider2D> ();
		//_PHandle = FindObjectOfType<PseudoHandle> ();
		//_PHandle._PseudoBlockList.Add(this);
	}
	void OnMouseDown(){
		_WillDie = true;
	}

	void OnMouseExit(){
		_WillDie = false;
	}

	void OnMouseUp(){
		if (_WillDie) {
			for (int i = 0; i < _ListOfTiles.Count; i++) {
				//_ListOfTiles [i]._IsOccupied = false;
				_ListOfTiles [i].gameObject.SetActive (true);
				//liberando espaço no contador de blocos ja que esta sendo deletado o bloco atual
				_MyGrid.DecressActualNumberOfBlocks ();
			}
			Destroy (gameObject);
		}
	}

	public void StartPlay(){
		GetComponent<Destructible> ().enabled = true;
		gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1f;
		_myColl.isTrigger = false;
		//_myColl.size = new Vector2(_myColl.size.x + 0.1f, _myColl.size.y + 0.1f);
		gameObject.tag = "Destructible";
		this.enabled = false;
	}
}