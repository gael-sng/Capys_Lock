using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTileBehaviour : MonoBehaviour {
	private SpriteRenderer _Sprite;
	private SpawnGridBehaviour _SpawnGrid;

	void Start () {
		_Sprite = GetComponent<SpriteRenderer> ();
		_SpawnGrid = transform.parent.GetComponent<SpawnGridBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		//Verificar se o mouse esta arrastando em cima do botão
		if(Input.GetMouseButtonUp(0))UnSelect();
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
			_Sprite.color = Color.gray;
		}
	}

	private void UnSelect(){
		_Sprite.color = Color.white;
	}
}
