using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTileBehaviour : MonoBehaviour {
	private SpriteRenderer _Sprite;
	private SpawnGridBehaviour _SpawnGrid;
	public bool _IsOccupied;
	void Start () {
		_IsOccupied = false;
		_Sprite = gameObject.GetComponentInChildren<SpriteRenderer> ();
		_SpawnGrid = transform.parent.GetComponent<SpawnGridBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		//Verificar se o mouse esta arrastando em cima do botão
		//if(Input.GetMouseButtonUp(0))UnSelect();
	}

	void OnMouseDown(){
		//Começar a selecionar os botões
		Select();
	}

	void OnMouseEnter(){
		if (_SpawnGrid.IsSeleting()) {
				Select ();
		}
	}

	void OnMouseUp(){
		//Verificar se os botões selecionados vão spawnar
		UnSelect ();
	}

	private void Select(){
		if (_SpawnGrid._SelectTile (this)) {
			_Sprite.color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
		}
	}

	public void UnSelect(){
		_Sprite.color = new Color(1f,1f,1f,0.5f);
	}

}
